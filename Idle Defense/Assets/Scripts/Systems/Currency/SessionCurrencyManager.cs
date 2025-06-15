using System;
using UnityEngine;

namespace Assets.Scripts.Systems.Currency
{
    public class SessionCurrencyManager : MonoBehaviour
    {
        public static SessionCurrencyManager Instance { get; private set; }

        public event EventHandler OnSessionCurrencyChanged;
        public int SessionCurrency { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            //TODO: Subscribe to events that updates the session currency and call AddSessionCurrency(amount)

            //TODO: Subscribe to event that resets the game and call ResetAll()

            PlayerBaseManager.Instance.OnWaveFailed += PlayerBaseManager_OnWaveFailed;

        }

        public bool CanSpend(int amount) => SessionCurrency >= amount;

        public void SpendSessionCurrency(int amount)
        {
            if (!CanSpend(amount))
                return;

            SessionCurrency -= amount;

            OnSessionCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AddSessionCurrency(int amount)
        {
            SessionCurrency += amount;

            OnSessionCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PlayerBaseManager_OnWaveFailed(object sender, EventArgs e)
        {
            ResetSessionCurrency();
        }

        private void ResetSessionCurrency()
        {
            SessionCurrency = 0;
        }

        private void ResetAll()
        {
            SessionCurrency = 0;
        }
    }
}