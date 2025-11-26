using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;



namespace MVC_AurumLab.Controllers
{
    
    public class LoginController : Controller
    {
        
        private readonly AppDbContext _contexto;

        public LoginController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(string email, string senha)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "Preencha todos os campos";
                return View("Index");
            }

            byte[] senhaDigitadaHash =  DescriptografarHash.GerarHashBytes(senha);
            
            var usuarios = _contexto.Usuarios.FirstOrDefault
            (
                usuarios => usuarios.Email == email
                );

                if(usuarios == null)
            {
                ViewBag.Erro = "Email ou senha incorretos";
                return View("Index", "Login");
            }

            if(!(usuarios.Senha.SequenceEqual(senhaDigitadaHash)))
            {
                ViewBag.Erro = "Email ou senha incorretos";
                return View("Index");
            }

            HttpContext.Session.SetString("UsuarioNome", usuarios.NomeCompleto);
            HttpContext.Session.SetInt32("UsuarioId", usuarios.IdUsuario);
            
            return RedirectToAction("Dashboard", "Dashboard");

        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}