using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using Assets.Scripts.WaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        public Action<CurrencyEnum> OnCurrencyChanged;

        [SerializeField] private List<CurrencySO> _currencies;

        private readonly Dictionary<CurrencyEnum, CurrencyInstance> _currencyInstances = new();
        private readonly CurrencyHoldManager _holdManager = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            InitializeCurrencies();

            EnemySpawner.Instance.OnEnemyDeath += EnemySpawner_OnEnemyDeath;

            PlayerBaseManager.Instance.OnWaveFailed += PlayerBaseManager_OnWaveFailed;
        }

        private void EnemySpawner_OnEnemyDeath(object sender, EnemySpawner.OnEnemyDeathEventArgs e)
        {
            AddCurrency(CurrencyEnum.Session, e.CoinDropAmount);
        }

        private void PlayerBaseManager_OnWaveFailed(object sender, EventArgs e)
        {
            _holdManager.ReleaseTo(_currencyInstances, OnCurrencyChanged);
        }

        private void InitializeCurrencies()
        {
            _currencies.ForEach(currency =>
            {
                if (!_currencyInstances.TryGetValue(currency.CurrencyType, out CurrencyInstance _))
                {
                    _currencyInstances[currency.CurrencyType] = new CurrencyInstance
                    {
                        IsPermanent = currency.IsPermanent,
                        Currency = currency.Currency
                    };
                }
                else
                {
                    Debug.Log($"Currencytype {currency} is already added");
                }
            });
        }

        private void AddCurrency(CurrencyEnum currencyType, ulong amount)
        {
            if (!_currencyInstances.TryGetValue(currencyType, out CurrencyInstance instance))
            {
                Debug.LogError($"Currencytype {currencyType} not found");
                return;
            }

            instance.Currency += amount;

            OnCurrencyChanged?.Invoke(currencyType);
        }

        public ulong GetCurrencyAmount(CurrencyEnum currencyType) =>
            _currencyInstances.TryGetValue(currencyType, out CurrencyInstance instance) ? instance.Currency : 0;

        public bool CanSpend(CurrencyEnum currencyType, ulong amount)
        {
            if (_currencyInstances.TryGetValue(currencyType, out CurrencyInstance instance))
                return instance.Currency >= amount;

            Debug.LogError($"Currencytype {currencyType} not found");
            return false;
        }

        public void SpendCurrency(CurrencyEnum currencyType, ulong amount)
        {
            if (!CanSpend(currencyType, amount))
                return;

            if (!_currencyInstances.TryGetValue(currencyType, out CurrencyInstance instance))
                return;

            instance.Currency -= amount;
            OnCurrencyChanged?.Invoke(currencyType);
        }

        public void ResetAll()
        {
            _currencyInstances.Clear();
            _holdManager.Clear();

            InitializeCurrencies();
        }
    }
}