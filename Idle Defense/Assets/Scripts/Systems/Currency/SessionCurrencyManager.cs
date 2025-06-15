using Assets.Scripts.Systems.Audio;
using Assets.Scripts.UI;
using Assets.Scripts.WaveSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.Systems.Currency
{
    public class SessionCurrencyManager : MonoBehaviour
    {
        public static SessionCurrencyManager Instance { get; private set; }

        public Action<ulong> OnSessionCurrencyChanged;
        public ulong SessionCurrency { get; private set; }

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
            EnemySpawner.Instance.OnEnemyDeath += OnEnemyDeath;


            //TODO: Subscribe to event that resets the game and call ResetAll()

            PlayerBaseManager.Instance.OnWaveFailed += PlayerBaseManager_OnWaveFailed;

        }

        public bool CanSpend(ulong amount) => SessionCurrency >= amount;

        public void Spend(ulong amount)
        {
            if (!CanSpend(amount))
            {
                AudioManager.Instance.Play("No Money");
                return;
            }

            SessionCurrency -= amount;

            OnSessionCurrencyChanged?.Invoke(SessionCurrency);

            StatsManager.Instance.MoneySpent += amount;
            StatsManager.Instance.UpgradeAmount++;
        }

        private void OnEnemyDeath(object sender, EnemySpawner.OnEnemyDeathEventArgs e) => AddSessionCurrency(e.CoinDropAmount);

        private void AddSessionCurrency(ulong amount)
        {
            SessionCurrency += amount;

            OnSessionCurrencyChanged?.Invoke(SessionCurrency);
        }

        private void PlayerBaseManager_OnWaveFailed(object sender, EventArgs e)
        {
            ResetSessionCurrency();
        }

        public void LoadMoney(ulong amount)
        {
            SessionCurrency = amount;
            UIManager.Instance.UpdateMoney(SessionCurrency);
        }

        private void ResetSessionCurrency()
        {
            SessionCurrency = 0;
        }

        private void ResetAll()
        {
            SessionCurrency = 0;
            OnSessionCurrencyChanged?.Invoke(SessionCurrency);
        }
    }
}