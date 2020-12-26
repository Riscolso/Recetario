using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Recetario.BaseDatos
{
    public partial class ContextoBD : IdentityDbContext<Actor, IdentityRole<int>, int>
    {
        public ContextoBD()
        {
        }

        public ContextoBD(DbContextOptions<ContextoBD> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actor { get; set; }
        public virtual DbSet<Etiqueta> Etiqueta { get; set; }
        public virtual DbSet<Ingrediente> Ingrediente { get; set; }
        public virtual DbSet<Lleva> Lleva { get; set; }
        public virtual DbSet<Paso> Paso { get; set; }
        public virtual DbSet<Receta> Receta { get; set; }
        public virtual DbSet<Usa> Usa { get; set; }
        public virtual DbSet<Visualizacion> Visualizacion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("actor");

                entity.Property(e => e.Id)
                    .HasColumnName("idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FechaNac).HasColumnType("date");

                entity.Property(e => e.NombreActor)
                    .IsRequired()
                    .HasColumnType("varchar(55)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Etiqueta>(entity =>
            {
                entity.HasKey(e => e.IdEtiqueta)
                    .HasName("PRIMARY");

                entity.ToTable("etiqueta");

                entity.Property(e => e.IdEtiqueta)
                    .HasColumnName("idEtiqueta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Etiqueta1)
                    .IsRequired()
                    .HasColumnName("Etiqueta")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Ingrediente>(entity =>
            {
                entity.HasKey(e => e.IdIngrediente)
                    .HasName("PRIMARY");

                entity.ToTable("ingrediente");

                entity.Property(e => e.IdIngrediente)
                    .HasColumnName("idIngrediente")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Lleva>(entity =>
            {
                entity.HasKey(e => new { e.RecetaIdReceta, e.RecetaActorIdActor, e.IngredienteIdIngrediente })
                    .HasName("PRIMARY");

                entity.ToTable("lleva");

                entity.HasIndex(e => e.IngredienteIdIngrediente)
                    .HasName("fk_Receta_has_Ingrediente_Ingrediente1_idx");

                entity.HasIndex(e => new { e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("fk_Receta_has_Ingrediente_Receta1_idx");

                entity.Property(e => e.RecetaIdReceta)
                    .HasColumnName("Receta_idReceta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecetaActorIdActor)
                    .HasColumnName("Receta_Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IngredienteCrudo)
                    .IsRequired()
                    .HasColumnName("IngredienteCrudo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IngredienteIdIngrediente)
                    .HasColumnName("Ingrediente_idIngrediente")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IngredienteIdIngredienteNavigation)
                    .WithMany(p => p.Lleva)
                    .HasForeignKey(d => d.IngredienteIdIngrediente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Receta_has_Ingrediente_Ingrediente1");

                entity.HasOne(d => d.Receta)
                    .WithMany(p => p.Lleva)
                    .HasForeignKey(d => new { d.RecetaIdReceta, d.RecetaActorIdActor })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Receta_has_Ingrediente_Receta1");
            });

            modelBuilder.Entity<Paso>(entity =>
            {
                entity.HasKey(e => new {e.NoPaso, e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("PRIMARY");

                entity.ToTable("paso");

                entity.Property(e => e.NoPaso).HasColumnType("int(11)");

                entity.HasIndex(e => new { e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("fk_Paso_Receta1_idx");

                entity.Property(e => e.RecetaActorIdActor)
                    .HasColumnName("Receta_Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecetaIdReceta)
                    .HasColumnName("Receta_idReceta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Texto)
                    .HasColumnType("varchar(600)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TiempoTemporizador).HasColumnType("int(11)");

                entity.HasOne(d => d.Receta)
                    .WithMany(p => p.Paso)
                    .HasForeignKey(d => new { d.RecetaIdReceta, d.RecetaActorIdActor })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Paso_Receta1");
            });

            modelBuilder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => new { e.IdReceta, e.ActorIdActor })
                    .HasName("PRIMARY");

                entity.ToTable("receta");

                entity.HasIndex(e => e.ActorIdActor)
                    .HasName("fk_Receta_Actor1_idx");

                entity.Property(e => e.IdReceta)
                    .HasColumnName("idReceta")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ActorIdActor)
                    .HasColumnName("Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProcentajePromedio).HasColumnType("int(11)");

                entity.Property(e => e.TiempoPrep)
                    .IsRequired()
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.ActorIdActorNavigation)
                    .WithMany(p => p.Receta)
                    .HasForeignKey(d => d.ActorIdActor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Receta_Actor1");
            });

            modelBuilder.Entity<Usa>(entity =>
            {
                entity.HasKey(e => new { e.RecetaIdReceta, e.RecetaActorIdActor, e.EtiquetaIdEtiqueta })
                    .HasName("PRIMARY");

                entity.ToTable("usa");

                entity.HasIndex(e => e.EtiquetaIdEtiqueta)
                    .HasName("fk_Receta_has_Etiqueta_Etiqueta1_idx");

                entity.HasIndex(e => new { e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("fk_Receta_has_Etiqueta_Receta1_idx");

                entity.Property(e => e.RecetaIdReceta)
                    .HasColumnName("Receta_idReceta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecetaActorIdActor)
                    .HasColumnName("Receta_Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EtiquetaIdEtiqueta)
                    .HasColumnName("Etiqueta_idEtiqueta")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.EtiquetaIdEtiquetaNavigation)
                    .WithMany(p => p.Usa)
                    .HasForeignKey(d => d.EtiquetaIdEtiqueta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Receta_has_Etiqueta_Etiqueta1");

                entity.HasOne(d => d.Receta)
                    .WithMany(p => p.Usa)
                    .HasForeignKey(d => new { d.RecetaIdReceta, d.RecetaActorIdActor })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Receta_has_Etiqueta_Receta1");
            });

            modelBuilder.Entity<Visualizacion>(entity =>
            {
                entity.HasKey(e => new { e.ActorIdActor, e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("PRIMARY");

                entity.ToTable("visualizacion");

                entity.HasIndex(e => e.ActorIdActor)
                    .HasName("fk_Actor_has_Receta_Actor1_idx");

                entity.HasIndex(e => new { e.RecetaIdReceta, e.RecetaActorIdActor })
                    .HasName("fk_Actor_has_Receta_Receta1_idx");

                entity.Property(e => e.ActorIdActor)
                    .HasColumnName("Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecetaIdReceta)
                    .HasColumnName("Receta_idReceta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PorCocinar).HasColumnType("tinyint(4)");

                entity.Property(e => e.RecetaActorIdActor)
                    .HasColumnName("Receta_Actor_idActor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProcentajeCompl).HasColumnType("int(11)");

                entity.HasOne(d => d.ActorIdActorNavigation)
                    .WithMany(p => p.Visualizacion)
                    .HasForeignKey(d => d.ActorIdActor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Actor_has_Receta_Actor1");

                entity.HasOne(d => d.Receta)
                    .WithMany(p => p.Visualizacion)
                    .HasForeignKey(d => new { d.RecetaIdReceta, d.RecetaActorIdActor })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Actor_has_Receta_Receta1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
