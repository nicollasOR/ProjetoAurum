using System;
using System.Collections.Generic;

namespace MVC_AurumLab.Models;

public partial class RegraPerfil
{
    public int IdRegra { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
