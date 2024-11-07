using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodDeliveryServer.Data.Configurations
{
    public class DeilveryConfiguration : UserConfiguration<Delivery>
    {
        public override void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);

            base.Configure(builder);
        }
    }
}
