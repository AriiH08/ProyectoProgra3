using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Data.Models
{
    public partial class ClinicaMedicaDbContext : DbContext
    {
        public ClinicaMedicaDbContext()
        {
        }

        public ClinicaMedicaDbContext(DbContextOptions<ClinicaMedicaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cita> Citas { get; set; }
        public virtual DbSet<Doctore> Doctores { get; set; }
        public virtual DbSet<Laboratorio> Laboratorios { get; set; }
        public virtual DbSet<Medicamento> Medicamentos { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
        public virtual DbSet<PersonalAdministrativo> PersonalAdministrativos { get; set; }
        public virtual DbSet<PruebasLaboratorio> PruebasLaboratorios { get; set; }
        public virtual DbSet<Receta> Recetas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-EI0DHR3\\SQLEXPRESS;Initial Catalog=ClinicaMedica;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.CitaId).HasName("PK__Citas__F0E2D9D2E7A76D66");

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasDefaultValue("Programada");
                entity.Property(e => e.FechaCita).HasColumnType("datetime");
                entity.Property(e => e.MotivoConsulta).HasMaxLength(255);

                // Relación con Usuario
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Citas_Usuarios");

                // Relación con Doctor
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_Citas_Doctor");

                // Relación con Paciente
                entity.HasOne(d => d.Paciente)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.PacienteId)
                    .HasConstraintName("FK_Citas_Paciente");

                // Relación con Personal Administrativo
                entity.HasOne(d => d.Personal)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.PersonalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citas_Personal");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuarioId).HasName("PK_Usuarios");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(255);

                // Relación con Rol
                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Usuarios_Roles");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.RolId).HasName("PK_Roles");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Otras configuraciones existentes
            modelBuilder.Entity<Doctore>(entity =>
            {
                entity.HasKey(e => e.DoctorId).HasName("PK__Doctores__2DC00EBF9715F277");

                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.Especialidad).HasMaxLength(100);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.HasKey(e => e.LaboratorioId).HasName("PK__Laborato__5BC4219046FC3BC2");

                entity.Property(e => e.Direccion).HasMaxLength(200);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasKey(e => e.MedicamentoId).HasName("PK__Medicame__003D65D3CB9A9488");

                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.PacienteId).HasName("PK__Paciente__9353C01FA31F437A");

                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            modelBuilder.Entity<PersonalAdministrativo>(entity =>
            {
                entity.HasKey(e => e.PersonalId).HasName("PK__Personal__283437F3EC003AE1");

                entity.ToTable("PersonalAdministrativo");

                entity.Property(e => e.Cargo).HasMaxLength(100);
                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            modelBuilder.Entity<PruebasLaboratorio>(entity =>
            {
                entity.HasKey(e => e.PruebaId).HasName("PK__PruebasL__E93DDB5C938A0040");

                entity.ToTable("PruebasLaboratorio");

                entity.Property(e => e.FechaPrueba).HasColumnType("datetime");
                entity.Property(e => e.TipoPrueba).HasMaxLength(100);

                entity.HasOne(d => d.Laboratorio)
                    .WithMany(p => p.PruebasLaboratorios)
                    .HasForeignKey(d => d.LaboratorioId)
                    .HasConstraintName("FK_PruebasLaboratorio_Laboratorio");

                entity.HasOne(d => d.Paciente)
                    .WithMany(p => p.PruebasLaboratorios)
                    .HasForeignKey(d => d.PacienteId)
                    .HasConstraintName("FK_PruebasLaboratorio_Paciente");
            });

            modelBuilder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => e.RecetaId).HasName("PK__Recetas__03D077D871BB6668");

                entity.HasOne(d => d.Cita)
                    .WithMany(p => p.Receta)
                    .HasForeignKey(d => d.CitaId)
                    .HasConstraintName("FK_Recetas_Cita");

                entity.HasOne(d => d.Medicamento)
                    .WithMany(p => p.Receta)
                    .HasForeignKey(d => d.MedicamentoId)
                    .HasConstraintName("FK_Recetas_Medicamento");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
