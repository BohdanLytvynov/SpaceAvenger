using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class CommanderWallet : EntityBase
    {
        public decimal Amount { get; set; }

        public Commander Commander { get; set; }
        public int CommanderId { get; set; }

        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
    }
}
