namespace ClinicaMedica.Data.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        public int RolId { get; set; }

        // Navegación hacia el rol
        public Rol Rol { get; set; } = null!;

        // Navegación hacia las citas del usuario
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
