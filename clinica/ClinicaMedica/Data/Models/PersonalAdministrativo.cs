using System;
using System.Collections.Generic;

namespace ClinicaMedica.Data.Models;

public partial class PersonalAdministrativo
{
    public int PersonalId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Cargo { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
