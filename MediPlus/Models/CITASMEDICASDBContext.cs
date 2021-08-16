using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MediPlus.Models
{
    public partial class CITASMEDICASDBContext : DbContext
    {
        public CITASMEDICASDBContext()
        {
        }

        public CITASMEDICASDBContext(DbContextOptions<CITASMEDICASDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cita> Citas { get; set; }
        public virtual DbSet<Codigosvalidacion> Codigosvalidacions { get; set; }
        public virtual DbSet<Especialidade> Especialidades { get; set; }
        public virtual DbSet<Medico> Medicos { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Secretarium> Secretaria { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-12SVVHHH\\SQLEXPRESS;Database=CITASMEDICASDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("CITAS");

                entity.Property(e => e.Comentario).IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCita).HasColumnType("date");

                entity.Property(e => e.MotivoDeConsulta)
                    .IsUnicode(false)
                    .HasColumnName("Motivo_de_consulta");

                entity.HasOne(d => d.IdMedicoNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.IdMedico)
                    .HasConstraintName("fk_medico_consulta");

                entity.HasOne(d => d.IdPacientesNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.IdPacientes)
                    .HasConstraintName("fk_gestion_pacientes");
            });

            modelBuilder.Entity<Codigosvalidacion>(entity =>
            {
                entity.ToTable("CODIGOSVALIDACION");

                entity.HasIndex(e => e.Codigo, "UQ__CODIGOSV__06370DAC4DADD383")
                    .IsUnique();

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Codigosvalidacions)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CODIGOSVA__Usuar__403A8C7D");
            });

            modelBuilder.Entity<Especialidade>(entity =>
            {
                entity.ToTable("ESPECIALIDADES");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Medico>(entity =>
            {
                entity.ToTable("MEDICOS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Oficina)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.EspecialidadNavigation)
                    .WithMany(p => p.Medicos)
                    .HasForeignKey(d => d.Especialidad)
                    .HasConstraintName("FK__MEDICOS__Especia__45F365D3");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Medicos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MEDICOS__IdUsuar__46E78A0C");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.ToTable("PACIENTES");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Cedula)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.LugarDeNacimiento)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Lugar_de_nacimiento");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Sexo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PACIENTES__IdUsu__4316F928");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLES");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Secretarium>(entity =>
            {
                entity.ToTable("SECRETARIA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Oficina)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdDoctorNavigation)
                    .WithMany(p => p.Secretaria)
                    .HasForeignKey(d => d.IdDoctor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SECRETARI__IdDoc__4E88ABD4");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Secretaria)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SECRETARI__IdUsu__4D94879B");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIOS");

                entity.HasIndex(e => e.Email, "UQ__USUARIOS__A9D10534E477E492")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Confirmado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK__USUARIOS__RolId__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
