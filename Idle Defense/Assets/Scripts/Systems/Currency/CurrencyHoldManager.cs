using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Systems.Currency
{
    public class CurrencyHoldManager
    {
        private readonly Dictionary<CurrencyEnum, ulong> _currenciesOnHold = new();

        public void Add(CurrencyEnum currency, ulong amount)
        {
            if (!_currenciesOnHold.TryAdd(currency, amount))
            {
                _currenciesOnHold[currency] += amount;
            }
        }

        public void ReleaseTo(Dictionary<CurrencyEnum, CurrencyInstance> targetCurrencies,
            Action<CurrencyEnum> onCurrencyChanged)
        {
            foreach (KeyValuePair<CurrencyEnum, ulong> kvp in _currenciesOnHold)
            {
                if (!targetCurrencies.TryGetValue(kvp.Key, out CurrencyInstance instance))
                    continue;

                instance.Currency += kvp.Value;
                onCurrencyChanged?.Invoke(kvp.Key);
            }

            _currenciesOnHold.Clear();
        }

        public void Clear()
        {
            _currenciesOnHold.Clear();
        }
    }
}
