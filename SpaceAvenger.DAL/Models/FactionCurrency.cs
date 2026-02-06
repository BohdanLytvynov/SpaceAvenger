using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class FactionCurrency : EntityBase
    {
        public int FactionId { get; set; }
        public Faction Faction { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public bool IsPrimary { get; set; }

        public float ExchangeRateModifier { get; set; } = 1.0f;
    }
}