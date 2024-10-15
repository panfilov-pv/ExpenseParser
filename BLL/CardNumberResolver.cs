using BLL.Abstraction;
using Shared.Configuration;

namespace BLL
{
    public class CardNumberResolver : ICardNumberResolver
    {
        private readonly IConfig _config;

        public CardNumberResolver(IConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.KnownCardNumbers == null)
            {
                throw new ArgumentNullException(nameof(config.KnownCardNumbers));
            }

            _config = config;
        }

        private CardNumberResolver() { }

        public int? ResolveCardNumber(string cardNumberToResolve)
        {
            int? result = null;

            if (string.IsNullOrEmpty(cardNumberToResolve))
            {
                return result;
            }

            int holderIndex = 0;

            foreach (var knownCardHolder in _config.KnownCardNumbers)
            {
                if (knownCardHolder.Value.Contains(cardNumberToResolve))
                {
                    return holderIndex;
                }

                holderIndex++;
            }

            return result;

        }
    }
}
