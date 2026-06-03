using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class SeguimientoObra
{
    public int IdSeguimiento { get; set; }

    public int IdObra { get; set; }

    public DateTime Fecha { get; set; }

    public string DescripcionAvance { get; set; } = null!;

    public int PorcentajeAvance { get; set; }

    public string? ImgProgreso { get; set; }

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
