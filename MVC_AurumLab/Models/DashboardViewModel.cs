using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_AurumLab.Models
{
    public class DashboardViewModel 
    {
        public int totalDispositivos {get; set;}
        public int totalAtivos {get;set;}

        public int totalManutencao {get; set;}

        public int totalInoperantes {get;set;}

        public string NomeUsuario {get;set;}

        public string FotoUsuario {get;set;}

        public List<ItemAgrupado> DispositivosPorTipo {get;set;}
        public List<LocalDispositivo> Locais {get;set;}

        // public DashboardViewModel(
        // int totalDispositivosCONSTRUTOR, int totalAtivosCONSTRUTOR, 
        // int totalManutencaoCONSTRUTOR, int totalInoperantesCONSTRUTOR, 
        // string NomeUsuarioCONSTRUTOR, string FotoUsuarioCONSTRUTOR)
        // {
            
        // }
    }
}