using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class DetalleOrdenCompra
{
    public int IdOrden { get; set; }

    public int IdMaterial { get; set; }

    public decimal CantidadPedida { get; set; }

    public decimal PrecioUnitarioCompra { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual OrdenCompra IdOrdenNavigation { get; set; } = null!;
}
