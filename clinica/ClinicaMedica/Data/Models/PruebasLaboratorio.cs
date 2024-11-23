using System;
using System.Collections.Generic;

namespace ClinicaMedica.Data.Models;

public partial class PruebasLaboratorio
{
    public int PruebaId { get; set; }

    public int PacienteId { get; set; }

    public int LaboratorioId { get; set; }

    public string TipoPrueba { get; set; } = null!;

    public DateTime FechaPrueba { get; set; }

    public string? Resultado { get; set; }

    public virtual Laboratorio Laboratorio { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
