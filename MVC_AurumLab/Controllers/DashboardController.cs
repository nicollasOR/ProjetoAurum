using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;


namespace MVC_AurumLab.Controllers
{
    public class DashboardController : Controller
    {
        
        private readonly AppDbContext _contexto;

        public DashboardController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UsuarioId") == null)
            { // validade se existe um login realizado 
                return RedirectToAction("Index", "Login");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            
            var usuario = _contexto.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == usuarioId);
            //pegando o usuario por completo
            // vou procurar por cada usuario a partir do id
            // para ver se o usuarioId eh igual ao usuarioId do banco

            // Tipos Dispositivos - JOIN + Agrupamento
            //consultar a tabela dispositivo atraves da viewmodel
            
            //SELECT * FROM Dispositivos
            var dispositivosTipo = _contexto.Dispositivos.
            Join(
                
                _contexto.TipoDispositivos, // JOIN TipoDispositivos)
                dispositivo => dispositivo.IdTipoDispositivo, // dispositivo.IdTipoDispositivo
                TipoDispositivo => TipoDispositivo.IdTipoDispositivo,
                (dispositivo, tipoDispositivo => new{dispositivo, tipoDispositivo.Nome})
                //Para cada par encontrado - um dispositivo e seu tipoDispositivo correspondente - monta um objeto:
                // dispositivo -> objeto completo
                /*

                nome -> nome do tipo do dispositivo
                {
                dispositivo = (objeto Dispositivo inteiro)
                 Nome = "Sensor
                } (exemplo)
                
                */
                )
            return View("Index");
        }










    }
}