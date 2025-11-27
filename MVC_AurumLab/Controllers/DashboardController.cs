using System.Security.Policy;
using System.Text.RegularExpressions;
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

        public IActionResult Dashboard() //Dashboard
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
                (dispositivo, tipoDispositivo) => new{dispositivo, tipoDispositivo.Nome}
                //Para cada par encontrado - um dispositivo e seu tipoDispositivo correspondente - monta um objeto:
                // dispositivo -> objeto completo
                /*

                nome -> nome do tipo do dispositivo
                {
                dispositivo = (objeto Dispositivo inteiro)
                 Nome = "Sensor
                } (exemplo)
                
                */
                ).GroupBy(item => item.Nome) // agrupa os dispositivos por nome
                .Select(grupo => new ItemAgrupado // cria a lista de item agrupado para 
                                                   // retornar somente nome e quantidade
                {
                Nome = grupo.Key, // key  = nome do tipo
                Quantidade = grupo.Count() // Count() = retira quantidade de dispositivos daquele tipo
                })
                .ToList(); // executa a consulta no banco e transforma e m lista


                // lista de locais

                var locais = _contexto.LocalDispositivos
                .OrderBy(local => local.Nome)
                .ToList(); // buscar os locais cadastrados e ordernar pelo nome

                // VIEW MODEL
                // Cria a ViewModel com todas as informacoes que a pagina precisa
                DashboardViewModel viewModel = new DashboardViewModel
                {
                    NomeUsuario = usuario?.NomeUsuario ?? "Usuario", // if(usuario != null) { NomeUsuario = usuario.NomeUsuario} 
                                                                     // else{ return "Usuario" //!retorna o NomeUsuario como padrao}
                    FotoUsuario = "/assets/img/img-perfil.png",
                    totalDispositivos = _contexto.Dispositivos.Count(),
                    totalAtivos = _contexto.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Operando"),
                    totalManutencao = _contexto.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Em manutencao"),
                    totalInoperantes = _contexto.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Inoperante"),

                    DispositivosPorTipo = dispositivosTipo,
                    Locais = locais
                };

            return View(viewModel);
        
        }
    }
}