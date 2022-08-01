

using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Project.BillingProcessing.Customer.Infra.ConfigurationMap
{
    public class CustomerEntityConfigurationMap : IEntityTypeConfiguration<Project.BillingProcessing.Customer.Domain.CustomerEntity.Customer>
    {
        public void Configure(EntityTypeBuilder<Domain.CustomerEntity.Customer> builder)
        {
            builder.ToTable("customer");
            builder.HasKey(o => o.Id);


            builder.Property(o => o.Name)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(o => o.State)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .IsRequired();
            builder.Property(o => o.Identification)
                .HasColumnType("bigint")
                .HasMaxLength(15)
               .IsRequired();

        
        }
    }
}
