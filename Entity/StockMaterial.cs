using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class StockMaterial
{
    public int IdMaterial { get; set; }

    public decimal CantidadDisponible { get; set; }

    public decimal StockMinimo { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;
}
