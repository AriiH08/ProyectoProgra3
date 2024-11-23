using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Data.Models;

public partial class Laboratorio
{
    public int LaboratorioId { get; set; }

    [Display(Name = "NombreLab")]
    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<PruebasLaboratorio> PruebasLaboratorios { get; set; } = new List<PruebasLaboratorio>();
}
