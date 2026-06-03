using System;
using System.Collections.Generic;

namespace MiWebApi.Entity;

public partial class PagoEmpleado
{
    public int IdPagoEmpleado { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal MontoNeto { get; set; }

    public string PeriodoMesAnio { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
