using System;
using System.Collections.Generic;

namespace ClinicaMedica.Data.Models;

public partial class Medicamento
{
    public int MedicamentoId { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
