using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence.Mapping
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.MerchantId)
                .IsRequired();

            builder.OwnsOne(x => x.Card);

            builder.OwnsOne(m => m.Card, a =>
            {
                a.Property(p => p.CardholderName)
                    .HasColumnName("CardholderName")
                    .IsRequired();

                a.Property(p => p.CardNumber)
                    .HasColumnName("CardNumber")
                    .IsRequired();

                a.Property(p => p.Cvv)
                    .HasColumnName("Cvv")
                    .IsRequired();

                a.Property(p => p.ExpirationDate)
                    .HasColumnName("ExpirationDate")
                    .IsRequired();
            });

            builder.Property(x => x.Amount)
                .HasPrecision(9, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .IsRequired();

            builder.Property(x => x.PaymentStatus)
                .IsRequired();

            builder.Ignore(x => x.Notifications);
        }
    }
}
