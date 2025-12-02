using System.Security.Policy;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_AurumLab.Data;
using MVC_AurumLab.Models;
using MVC_AurumLab.Services;


namespace MVC_AurumLab.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly AppDbContext _contexto;

        public DispositivosController(AppDbContext contexto)
        {
            _contexto = contexto;
        }
        
        //variavel?  == null (variavel comeca nulo)
        public IActionResult Index(string? busca = null, int? tipoId = null, int? localId = null)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuárioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }


            //u => abreviacao da funcao para auxiliar na representacao do usuario no banco
            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);
            //Bloqueia o acesso se nao for professor(regraId = 2)
            // se for diferente de regra 2m nao deixa o usuario acessar e volta para dashboard

            if (usuario.RegraId != 2)
            {
                //ViewBag nao tanka trazer todo o objeto
                //TempData aguenta trazer todo o objeto


                TempData["Erro"] = $"Acesso permitido somente para professores";
                return RedirectToAction("Dashboard", "Dashboard");

            }

            var selectBusca = _contexto.Dispositivos
            //Include funciona ocmo join, so que mais simples e menor preciso
            //uso o join qauando quero criar agrupamento de itens 
            .Include(dispositivo => dispositivo.IdTipoDispositivoNavigation)
            .Include(dispositivo => dispositivo.IdLocalNavigation)
            .AsQueryable();
            // agora podemos colocar where, por exemplo

            if (!string.IsNullOrEmpty(busca))
            {
                selectBusca = selectBusca.Where(dispositivo => dispositivo.Nome.Contains(busca));
                //select de busca vai armazeanar a materia e 
                //where verifica se existe dispositivo]

            }

            if (tipoId.HasValue)
        {
                //verific se o id do tipo do select selecionado eh igual ao tipo cadastrado no banco para o dispositivo
                selectBusca = selectBusca.Where(dispositivo => dispositivo.IdTipoDispositivo == tipoId.Value);
        }
        if(localId.HasValue)
            {
                selectBusca = selectBusca.Where(dispositivo => dispositivo.IdLocal == localId.Value);
            }

        DispositivosViewModel viewModel = new DispositivosViewModel
        {
            NomeUsuario = usuario.NomeUsuario ?? "Usuario",
            FotoUsuario = usuario.Foto != null
            ? $"data:image/*;base64,{Convert.ToBase64String(usuario.Foto)}"
            :"/assets/img/img-perfil.png",

            //Alguem esqueceu de puxar os dispositivos
            Dispositivos = selectBusca.ToList(),

            //puxa os dispositivos de acorod com a model e mostra na tela de acordo com o selectBusca
            Tipos = _contexto.TipoDispositivos.ToList(),
            Locais = _contexto.LocalDispositivos.ToList(),



            Busca = busca,
            TipoIdSelecionado = tipoId,
            LocalIdSelecionado = localId
        };







            return View(viewModel);
        }

        //fulano@aurum

        [HttpGet]
        //mostrando a visualizacao
        public IActionResult Editar(int id)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuárioId");

        if(usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dispositivo = _contexto.Dispositivos.FirstOrDefault(dis => dis.IdDispositivo == id);

            if(dispositivo == null)
            {
                return NotFound();
            }


            EditarDispositivosViewModel vm = new EditarDispositivosViewModel
            {
                IdDispositivo = dispositivo.IdDispositivo,
                Nome = dispositivo.Nome,
                IdTipoDispositivo = dispositivo.IdTipoDispositivo,
                IdLocal = dispositivo.IdLocal,
                DataUltimaManutencao = dispositivo.DataUltimaManutencao,

                Tipos = _contexto.TipoDispositivos.ToList(),
                Locais = _contexto.LocalDispositivos.ToList(),


            };

            return View("Editar", vm);
        }

        [HttpPost]
        public IActionResult Editar(EditarDispositivosViewModel vm)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuárioId");

            if(usuarioId == null)
            {
                 return RedirectToAction("Index", "Login");
            }

            var dispositivo = _contexto.Dispositivos.FirstOrDefault(d => d.IdDispositivo == vm.IdDispositivo);

            if(dispositivo == null)
            {
                return NotFound();
            }

            dispositivo.Nome = vm.Nome;
            //Alguem esqueceu de colocar o tipoDispositivo ne?
            dispositivo.IdTipoDispositivo = vm.IdTipoDispositivo;
            dispositivo.IdLocal = vm.IdLocal;
            dispositivo.DataUltimaManutencao = vm.DataUltimaManutencao;

            _contexto.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            
            var dispositivo = _contexto.Dispositivos.FirstOrDefault(dispo => dispo.IdDispositivo == id);

            if( dispositivo == null)
            {
                return NotFound();
            }

            _contexto.Dispositivos.Remove(dispositivo);

            _contexto.SaveChangesAsync();

            TempData["Sucesso"] = "Dispositivo excluido com sucesso";
            return RedirectToAction("Index");

        }
    }
}
