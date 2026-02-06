using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Currency : LoreEntity
    {
        public string ShortName { get; set; }
        public string ResourceKey { get; set; }

        public ICollection<FactionCurrency> FactionCurrency { get; set; } 
            = new List<FactionCurrency>();

        public ICollection<CommanderWallet> CommanderWallets { get; set; }
            = new List<CommanderWallet>();
    }
}
