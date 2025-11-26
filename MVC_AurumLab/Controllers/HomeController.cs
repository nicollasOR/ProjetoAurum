using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;

namespace MVC_AurumLab.Controllers
{
    public class HomeController : Controller
    {
       public IActionResult Index()
       {
           return View();
       }

        public IActionResult VerificarAcesso()
        {
            var usuarioID = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
    
}