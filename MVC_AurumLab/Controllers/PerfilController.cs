using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Logging;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;

namespace MVC_AurumLab.Controllers
{
    public class PerfilController : Controller
    {
        
        private readonly AppDbContext _contexto;

        public PerfilController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

[HttpGet]
        public IActionResult Index()
{

int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

if(usuarioId == null)
{
return RedirectToAction("Index", "Login");
}

var usuario = _contexto.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == usuarioId);

var viewModel = new PerfilViewModel
{
IdUsuario = usuario.IdUsuario,
NomeCompleto = usuario.NomeCompleto,
NomeUsuario = usuario.NomeUsuario ?? "Usuário",
Email = usuario.Email,

//

Regras = _contexto.RegraPerfils.ToList(),


// se existir foto, converte a foto para string
// se nao existir, retorna null
FotoBase64 = usuario.Foto != null
? Convert.ToBase64String(usuario.Foto)
: null 


};

return View(viewModel);

}

[HttpPost]

public IActionResult Index(PerfilViewModel model)
        {
            var usuario = _contexto.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == model.IdUsuario);

        if(usuario == null)
        {
            return RedirectToAction("Index", "Login");
        }

        if(!string.IsNullOrWhiteSpace(model.NovaSenha))
        {
                if(model.NovaSenha != model.ConfirmarSenha)
                {
                    ViewBag.Erro = "As senhas sao sao iguais";

                    // quando o post(a atualizacao que estamos fazendo) da erro e volta pra View, a lista de regras nao vem preenchida.
                    // ela eh um select que puxamos com a lista de regras que estamos puxando do banco
                    model.Regras = _contexto.RegraPerfils.ToList();
                    return View(model);

                }

                usuario.Senha = DescriptografarHash.GerarHashBytes(model.NovaSenha);
        }

        usuario.NomeCompleto = model.NomeCompleto;
        usuario.NomeUsuario = model.NomeUsuario;
        usuario.Email = model.Email;
        usuario.RegraId = model.RegraId;

        _contexto.SaveChangesAsync();

        //ViewBag morre no Redirect
        //TempData sobrevive a um Redirect

        TempData["Sucesso"] = "Perfil Atualizado com Sucesso!";
        return RedirectToAction("Index");
        }

        // Post 

        [HttpPost]
        public IActionResult AtualizarFoto(int IdUsuario, IFormFile foto)
        {
            //IFormFile => Representa uma rquivo enviado pelo formulario no html
            // quando o formulario e enviado, o navegador envia o arquivo e o MVC converte para um objeto IFormFile

            if(foto == null || foto.Length == 0)
            {
                return RedirectToAction("Index");
            }
            var usuario = _contexto.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == IdUsuario);

            if(usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                usuario.Foto = ms.ToArray();
            }
            _contexto.SaveChangesAsync();
            TempData["Sucesso"] = "Foto atualizada com sucesso!";
            return RedirectToAction("Index");
        }


    }
}