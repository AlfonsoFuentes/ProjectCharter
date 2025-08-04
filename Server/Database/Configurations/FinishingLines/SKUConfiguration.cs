using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.FinishlinLines;

namespace Server.Database.Configurations.FinishingLines
{


    internal class SKUConfiguration : IEntityTypeConfiguration<SKU>
    {
        public void Configure(EntityTypeBuilder<SKU> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
          
            builder.Property(e => e.Name).HasMaxLength(200).IsRequired();


            // Relación: SKU - ScheduleItem
            builder.HasOne(e => e.Product)
                   .WithMany(e => e.SKUs)
                   .HasForeignKey(e => e.ProductId)
                
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.LineSpeeds)
                   .WithOne(e => e.Sku)
                   .HasForeignKey(e => e.SkuId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class BackboneConfiguration : IEntityTypeConfiguration<Backbone>
    {
        public void Configure(EntityTypeBuilder<Backbone> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
            builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
     

           

            builder.HasMany(e => e.ProductComponents)
                 .WithOne(e => e.Backbone)
                 .HasForeignKey(e => e.BackboneId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict); // No borrar en cascada desde backbone

            // Relaciones
            builder.HasMany(e => e.BIGWIPTanks)
                   .WithOne(e => e.Backbone)
                   .HasForeignKey(e => e.BackboneId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); // No borrar en cascada desde backbone

            builder.HasMany(e => e.MixerBackbones)
         .WithOne(e => e.Backbone)
         .HasForeignKey(e => e.BackboneId)
         .IsRequired()
         .OnDelete(DeleteBehavior.Restrict); // No borrar en cascada desde backbone
        }
    }
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
     
            builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

            
           

            builder.HasMany(e => e.Components)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class ProductionLineConfiguration : IEntityTypeConfiguration<ProductionLine>
    {
        public void Configure(EntityTypeBuilder<ProductionLine> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

            // Propiedades


            // Relaciones
            builder.HasMany(e => e.LineSpeeds)
                   .WithOne(e => e.ProductionLine)
                   .HasForeignKey(e => e.LineId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.WIPTanks)
                  .WithOne(e => e.Line)
                  .HasForeignKey(e => e.LineId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict); // No borrar en cascada desde backbone
        }
    }

    internal class LineSpeedConfiguration : IEntityTypeConfiguration<LineSpeed>
    {
        public void Configure(EntityTypeBuilder<LineSpeed> builder)
        {
            // Llave primaria compuesta
            builder.HasKey(e => e.Id);

            // Relación: ProductionLine - LineSpeed
            builder.HasOne(e => e.ProductionLine)
                   .WithMany(e => e.LineSpeeds)
                   .HasForeignKey(e => e.LineId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación: SKU - LineSpeed
            builder.HasOne(e => e.Sku)
                   .WithMany()
                   .HasForeignKey(e => e.SkuId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    internal class MixerConfiguration : IEntityTypeConfiguration<Mixer>
    {
        public void Configure(EntityTypeBuilder<Mixer> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
            builder.Property(e => e.Name).HasMaxLength(200).IsRequired();


            // Relaciones
            builder.HasMany(e => e.Capabilities)
                   .WithOne(e => e.Mixer)
                   .HasForeignKey(e => e.MixerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    internal class MixerBackboneConfiguration : IEntityTypeConfiguration<MixerBackbone>
    {
        public void Configure(EntityTypeBuilder<MixerBackbone> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Mixer)
                   .WithMany(e => e.Capabilities)
                   .HasForeignKey(e => e.MixerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Backbone)
                   .WithMany(e => e.MixerBackbones)
                   .HasForeignKey(e => e.BackboneId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    internal class ProductionPlanConfiguration : IEntityTypeConfiguration<ProductionPlan>
    {
        public void Configure(EntityTypeBuilder<ProductionPlan> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.InitialLevelBigWips)
                             .WithOne(e => e.ProductionPlan)
                             .HasForeignKey(e => e.ProductionPlanId)
                             .IsRequired()
                             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.LineAssignments)
                   .WithOne(e => e.ProductionPlan)
                   .HasForeignKey(e => e.PlanId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    internal class ProductionLineAssignmentConfiguration : IEntityTypeConfiguration<ProductionLineAssignment>
    {
        public void Configure(EntityTypeBuilder<ProductionLineAssignment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.ProductionScheduleItems)
                   .WithOne(e => e.ProductionLineAssignment)
                   .HasForeignKey(e => e.ProductionLineAssignmentId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.ProductionLine)
                   .WithMany()
                   .HasForeignKey(e => e.LineId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.InitialLevelWips)
                       .WithOne(e => e.ProductionLineAssignment)
                       .HasForeignKey(e => e.ProductionLineAssignmentId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);
        }
    }

   
    internal class ProductionScheduleItemConfiguration : IEntityTypeConfiguration<ProductionScheduleItem>
    {
        public void Configure(EntityTypeBuilder<ProductionScheduleItem> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);

           

           

            // Relación: SKU - ScheduleItem
            builder.HasOne(e => e.SKU)
                   .WithMany(e=>e.ProductionScheduleItems)
                   .HasForeignKey(e => e.SkuId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
    internal class InitialLevelWipConfiguration : IEntityTypeConfiguration<InitialLevelWip>
    {
        public void Configure(EntityTypeBuilder<InitialLevelWip> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);


            // Relación: SKU - ScheduleItem
            builder.HasOne(e => e.WIPTankLine)
                   .WithMany(e => e.InitialLevelWips)
                   .HasForeignKey(e => e.WIPTankLineId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
    internal class InitialLevelBigWipConfiguration : IEntityTypeConfiguration<InitialLevelBigWip>
    {
        public void Configure(EntityTypeBuilder<InitialLevelBigWip> builder)
        {
            // Llave primaria
            builder.HasKey(e => e.Id);


            // Relación: SKU - ScheduleItem
            builder.HasOne(e => e.BIGWIPTank)
                   .WithMany(e => e.InitialLevelBigWips)
                   .HasForeignKey(e => e.BIGWIPTankId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}