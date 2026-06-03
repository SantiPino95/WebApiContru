using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class HerramientasAsignada
{
    public int IdObra { get; set; }

    public int IdHerramienta { get; set; }

    public DateTime FechaSalida { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public virtual Herramienta IdHerramientaNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
