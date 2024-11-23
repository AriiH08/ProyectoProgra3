using System;
using System.Collections.Generic;

namespace ClinicaMedica.Data.Models;

public partial class Receta
{
    public int RecetaId { get; set; }

    public int CitaId { get; set; }

    public int MedicamentoId { get; set; }

    public int Cantidad { get; set; }

    public virtual Cita Cita { get; set; } = null!;

    public virtual Medicamento Medicamento { get; set; } = null!;
}
