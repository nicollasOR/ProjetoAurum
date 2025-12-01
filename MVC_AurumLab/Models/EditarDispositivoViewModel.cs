using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_AurumLab.Models
{
    public class EditarDispositivosViewModel
    {
        public int IdDispositivo { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int IdTipoDispositivo { get; set; }

        public int IdLocal { get; set; }

        public DateOnly? DataUltimaManutencao { get; set; }

        // listas para armazenar o valor dos selects
        public List<TipoDispositivo> Tipos { get; set; } = new();
        public List<LocalDispositivo> Locais { get; set; } = new();
    }
}