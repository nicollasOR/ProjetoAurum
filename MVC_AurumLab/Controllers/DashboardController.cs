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










    }
}