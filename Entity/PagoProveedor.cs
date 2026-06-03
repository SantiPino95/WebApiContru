using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class PagoProveedor
{
    public int IdPagoProveedor { get; set; }

    public int IdProveedor { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
