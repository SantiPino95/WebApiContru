using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class MaterialObra
{
    public int IdObra { get; set; }

    public int IdMaterial { get; set; }

    public decimal CantidadConsumida { get; set; }

    public DateTime FechaConsumo { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
