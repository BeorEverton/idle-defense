using System;
using UnityEngine;

namespace Assets.Scripts.Systems.Currency
{
    public class PermanentCurrencyManager : MonoBehaviour
    {
        public static PermanentCurrencyManager Instance { get; private set; }
        public event EventHandler OnPermanentCurrencyChanged;
        public event EventHandler OnCurrencyOnHoldChanged;
        public int PermanentCurrency { get; private set; }

        public int CurrencyOnHold { get; private set; } //Currency that is being held till player dies

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            //TODO: Subscribe to events that updates the permanent currency and call AddPermanentCurrencyOnHold(amount)

            //TODO: Subscribe to event that resets the game and call ResetAll()

            PlayerBaseManager.Instance.OnWaveFailed += PlayerBaseManager_OnWaveFailed;
        }

        public bool CanSpend(int amount) => PermanentCurrency >= amount;

        public void SpendPermanentCurrency(int amount)
        {
            if (!CanSpend(amount))
                return;

            PermanentCurrency -= amount;

            OnPermanentCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerBaseManager_OnWaveFailed(object sender, EventArgs e)
        {
            AddHeldCurrencyToPermanentCurrency();
        }

        private void AddPermanentCurrencyOnHold(int amount)
        {
            CurrencyOnHold += amount;
            OnCurrencyOnHoldChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AddHeldCurrencyToPermanentCurrency()
        {
            PermanentCurrency += CurrencyOnHold;
            CurrencyOnHold = 0;

            OnPermanentCurrencyChanged?.Invoke(this, EventArgs.Empty);
            OnCurrencyOnHoldChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ResetAll()
        {
            PermanentCurrency = 0;
            CurrencyOnHold = 0;

            OnPermanentCurrencyChanged?.Invoke(this, EventArgs.Empty);
            OnCurrencyOnHoldChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}