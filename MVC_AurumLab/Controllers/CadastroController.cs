using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;

namespace MVC_AurumLab.Controllers
{
    public class CadastroController : Controller
    {

        private readonly AppDbContext _contexto;

        public CadastroController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] // Ele manda enquanto enviarmos ou tivermos um botao como submit

        public IActionResult Criar(string nome, string email, 
                                   string senha, string confirmar)
        {
            if(string.IsNullOrWhiteSpace(nome)  || string.IsNullOrWhiteSpace(confirmar) 
            || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "Preencha todos os cantos!";
                return View("Index");
            }
            
            
            
            if(senha != confirmar)
            {
                    ViewBag.Erro = "As senhas nao coincidem";
                    return View("Index");
            }

            if(_contexto.Usuarios.Any(usuarios => usuarios.Email == email))
                // O any seleciona o usuario, uma tabela do banco, e confere no banco de dados 
                // o que teria de igual
                //FirstOrDefault - traz o objeto por completo - ex> nome, foto
                //Any - Serve oara vakd=udar se exuste esse dado(no caso o oemail)

            {
                ViewBag.Erro = "Email ja cadastrado!";
                return View("Index");
            }

            byte[] hash = DescriptografarHash.GerarHashBytes(senha);
            Usuario usuario = new Usuario()
            {
                NomeCompleto = nome,
                Email = email,
                Senha = hash,
                Foto = null,
                RegraId = 1
            };

                //Salvar no banco agora
            _contexto.Usuarios.Add(usuario);
            _contexto.SaveChangesAsync();

            return RedirectToAction("Index", "Login");

            
        }
    }
}