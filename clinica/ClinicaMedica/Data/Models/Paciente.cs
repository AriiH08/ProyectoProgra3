using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Data.Models;

public partial class Paciente
{
    public int PacienteId { get; set; }
    [Display(Name = "NombrePaciente")]
    public string Nombre { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? HistorialMedico { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<PruebasLaboratorio> PruebasLaboratorios { get; set; } = new List<PruebasLaboratorio>();
}
