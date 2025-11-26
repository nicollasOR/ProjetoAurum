using System;
using System.Collections.Generic;

namespace MVC_AurumLab.Models;

public partial class LocalDispositivo
{
    public int IdLocal { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Dispositivo> Dispositivos { get; set; } = new List<Dispositivo>();
}
