namespace ClinicaMedica.Data.Models
{
    public partial class Cita
    {
        public int CitaId { get; set; }
        public int PacienteId { get; set; }
        public int DoctorId { get; set; }
        public int PersonalId { get; set; }
        public DateTime FechaCita { get; set; }
        public string? Estado { get; set; }
        public string? MotivoConsulta { get; set; }

        // Relación con Usuario
        public int UsuarioId { get; set; } // Clave foránea hacia la tabla Usuarios
        public virtual Usuario Usuario { get; set; } = null!; // Navegación hacia el Usuario

        // Relaciones existentes
        public virtual Doctore Doctor { get; set; } = null!;
        public virtual Paciente Paciente { get; set; } = null!;
        public virtual PersonalAdministrativo Personal { get; set; } = null!;
        public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
    }
}
