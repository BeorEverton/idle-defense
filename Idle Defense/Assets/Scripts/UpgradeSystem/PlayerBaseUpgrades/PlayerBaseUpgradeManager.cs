using Assets.Scripts.Enums;
using Assets.Scripts.PlayerBase;
using Assets.Scripts.Structs;
using Assets.Scripts.Systems;
using Assets.Scripts.Systems.Audio;
using Assets.Scripts.Systems.Currency;
using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.PlayerBaseUpgrades
{
    public class PlayerBaseUpgradeManager : MonoBehaviour
    {
        private Dictionary<PlayerUpgradeType, PlayerBaseUpgrade> _playerUpgrades;

        private void Start()
        {
            InitializeUpgrades();
        }

        private void InitializeUpgrades()
        {
            _playerUpgrades = new Dictionary<PlayerUpgradeType, PlayerBaseUpgrade>
            {
                [PlayerUpgradeType.MaxHealth] = new()
                {
                    GetCurrentValue = p => p.MaxHealth,
                    Upgrade = (p, a) =>
                    {
                        float newMax = p.MaxHealth;
                        for (int i = 0; i < a; i++)
                        {
                            newMax *= 1f + p.Stats[PlayerBaseStatType.MaxHealth].UpgradeAmount;
                        }

                        PlayerBaseStat stat = p.Stats[PlayerBaseStatType.MaxHealth];
                        stat.UpgradeAmount += a;
                        p.Stats[PlayerBaseStatType.MaxHealth] = stat;

                        p.MaxHealth = Mathf.CeilToInt(newMax);
                    },
                    GetLevel = p => p.Stats[PlayerBaseStatType.MaxHealth].Level,
                    GetUpgradeAmount = p => p.Stats[PlayerBaseStatType.MaxHealth].UpgradeAmount,
                    GetBaseCost = p => p.Stats[PlayerBaseStatType.MaxHealth].BaseCost,
                    GetMaxValue = p => float.MaxValue,
                    GetMinValue = p => 0f,
                    GetCost = (p, a) => GetCost(p, PlayerUpgradeType.MaxHealth, a),
                    //GetAmount = p => GetMaxAmount(p.MaxHealthUpgradeBaseCost, 1.1f, p.MaxHealthLevel),
                    GetDisplayStrings = (p, a) =>
                    {
                        float current = p.MaxHealth;
                        float pct = p.Stats[PlayerBaseStatType.MaxHealth].UpgradeAmount;

                        float projected = current;
                        for (int i = 0; i < a; i++)
                        {
                            projected *= 1f + pct;
                        }

                        int bonus = Mathf.CeilToInt(projected - current);

                        GetCost(p, PlayerUpgradeType.MaxHealth, a, out float cost, out int amount);

                        return (
                            $"{UIManager.AbbreviateNumber(current)}",
                            $"+{UIManager.AbbreviateNumber(bonus)}",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{UIManager.AbbreviateNumber(amount)}X"
                        );
                    }


                },
                [PlayerUpgradeType.RegenAmount] = new()
                {
                    GetCurrentValue = p => p.RegenAmount,
                    Upgrade = (p, a) =>
                    {
                        PlayerBaseStat stat = p.Stats[PlayerBaseStatType.RegenAmount];
                        stat.UpgradeAmount += a;
                        p.Stats[PlayerBaseStatType.RegenAmount] = stat;

                        p.RegenAmount += (p.Stats[PlayerBaseStatType.RegenAmount].UpgradeAmount * a);
                    },
                    GetLevel = p => p.Stats[PlayerBaseStatType.RegenAmount].Level,
                    GetUpgradeAmount = p => p.Stats[PlayerBaseStatType.RegenAmount].UpgradeAmount,
                    GetBaseCost = p => p.Stats[PlayerBaseStatType.RegenAmount].BaseCost,
                    GetMaxValue = p => float.MaxValue,
                    GetMinValue = p => 0f,
                    GetCost = (p, a) => GetCost(p, PlayerUpgradeType.RegenAmount, a),
                    //GetAmount = p => GetMaxAmount(p.RegenAmountUpgradeBaseCost, 1.1f, p.RegenAmountLevel),
                    GetDisplayStrings = (p, a) =>
                    {
                        float current = p.RegenAmount;
                        float bonus = GetBonusAmount(p, PlayerUpgradeType.RegenAmount);
                        GetCost(p, PlayerUpgradeType.RegenAmount, a, out float cost, out int amount);

                        return (
                                $"{current:F2}",
                                $"+{bonus:F2}",
                                $"${UIManager.AbbreviateNumber(cost)}",
                                $"{amount:F0}X");
                    }
                },
                [PlayerUpgradeType.RegenInterval] = new()
                {
                    GetCurrentValue = p => p.RegenInterval,
                    Upgrade = (p, a) =>
                    {
                        PlayerBaseStat stat = p.Stats[PlayerBaseStatType.RegenInterval];
                        stat.UpgradeAmount += a;
                        p.Stats[PlayerBaseStatType.RegenInterval] = stat;

                        _playerUpgrades.TryGetValue(PlayerUpgradeType.RegenInterval, out PlayerBaseUpgrade upgrade);

                        p.RegenInterval = Mathf.Max(p.RegenInterval - p.Stats[PlayerBaseStatType.RegenInterval].UpgradeAmount * a,
                            upgrade == null ? 0.5f : upgrade.GetMinValue(p));
                    },
                    GetLevel = p => p.Stats[PlayerBaseStatType.RegenInterval].Level,
                    GetUpgradeAmount = p => p.Stats[PlayerBaseStatType.RegenInterval].UpgradeAmount,
                    GetBaseCost = p => p.Stats[PlayerBaseStatType.RegenInterval].BaseCost,
                    GetMaxValue = p => float.MaxValue,
                    GetMinValue = p => 0.5f,
                    GetCost = (p, a) => GetCost(p, PlayerUpgradeType.RegenInterval, a),
                    //GetAmount = p => GetMaxAmount(p.RegenIntervalUpgradeBaseCost, 1.1f, p.RegenIntervalLevel),
                    GetDisplayStrings = (p, a) =>
                    {
                        float current = p.RegenInterval;
                        float bonus = GetBonusAmount(p, PlayerUpgradeType.RegenInterval);
                        GetCost(p, PlayerUpgradeType.RegenInterval, a, out float cost, out int amount);

                        if (current <= 0.5f)
                            return ($"{current:F2}s", "Max", "", "0");

                        return ($"{current:F2}s",
                            $"-{bonus:F2}s",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount:F0}X");
                    }
                }
            };
        }

        public float GetPlayerBaseUpgradeCost(PlayerBaseStatsInstance stats, PlayerUpgradeType type, int amount) =>
            !_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade) ? 0f : upgrade.GetCost(stats, amount);

        //public int GetPlayerBaseAvailableUpgradeAmount(PlayerBaseStatsInstance stats, PlayerUpgradeType type) =>
        //    !_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade) ? 0 : upgrade.GetAmount(stats);

        private float GetBonusAmount(PlayerBaseStatsInstance stats, PlayerUpgradeType type)
        {
            int amount = MultipleBuyOption.Instance.GetBuyAmount();

            float upgradeAmount = !_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade)
                ? 0f
                : upgrade.GetUpgradeAmount(stats);

            if (upgrade == null)
                return 1f;

            //if (amount == 9999)
            //    amount = upgrade.GetAmount(stats) == 0
            //        ? 1
            //        : upgrade.GetAmount(stats);

            return upgradeAmount * amount;
        }

        public void UpgradePlayerBaseStat(PlayerBaseStatsInstance stats, PlayerUpgradeType type, PlayerUpgradeButton button)
        {
            if (!_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade))
                return;

            int amount = MultipleBuyOption.Instance.GetBuyAmount();
            GetCost(stats, type, amount, out float cost, out int maxAmount);


            if (upgrade.GetMaxValue != null && upgrade.GetCurrentValue(stats) >= upgrade.GetMaxValue(stats))
            {
                UpdateUpgradeDisplay(stats, type, button);
                return;
            }

            if (CanSpend((ulong)cost))
            {
                SessionCurrencyManager.Instance.Spend((ulong)cost);

                upgrade.Upgrade(stats, amount);

                AudioManager.Instance.Play("Upgrade");

                UpdateUpgradeDisplay(stats, type, button);
                PlayerBaseManager.Instance.UpdatePlayerBaseAppearance();

                if (type == PlayerUpgradeType.MaxHealth)
                    PlayerBaseManager.Instance.InvokeHealthChangedEvents();
            }
        }

        private void GetCost(PlayerBaseStatsInstance stats, PlayerUpgradeType type, int inAmount, out float cost, out int outAmount)
        {
            if (!_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade))
            {
                cost = 0;
                outAmount = 0;
                return;
            }

            const float multiplier = 1.1f;
            int currentLevel = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            //int maxAmount = GetMaxAmount(baseCost, multiplier, currentLevel);

            //outAmount = inAmount == 9999
            //    ? maxAmount == 0
            //        ? 1
            //        : maxAmount
            //    : inAmount;

            outAmount = inAmount;

            cost = RecursiveCost(baseCost, multiplier, currentLevel, outAmount);
        }

        private float GetCost(PlayerBaseStatsInstance stats, PlayerUpgradeType type, int inAmount)
        {
            if (!_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade))
            {
                return 0f;
            }

            int currentLevel = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            const float multiplier = 1.1f;

            //if (inAmount == 9999)
            //    inAmount = GetMaxAmount(baseCost, multiplier, currentLevel);

            return RecursiveCost(baseCost, multiplier, currentLevel, inAmount);
        }

        //private int GetMaxAmount(float baseCost, float multiplier, int currentLevel)
        //{
        //    int amount = 0;
        //    float totalCost = 0f;
        //    float money = GameManager.Instance.Money;

        //    while (true)
        //    {
        //        float cost = baseCost * Mathf.Pow(multiplier, currentLevel + amount);
        //        if (totalCost + cost > money)
        //            break;
        //        totalCost += cost;
        //        amount++;
        //    }

        //    return amount;
        //}

        private float RecursiveCost(float baseCost, float multiplier, int level, int amount)
        {
            if (amount <= 0)
                return 0f;

            float cost = baseCost * Mathf.Pow(multiplier, level);
            return cost + RecursiveCost(baseCost, multiplier, level + 1, amount - 1);
        }

        public void UpdateUpgradeDisplay(PlayerBaseStatsInstance stats, PlayerUpgradeType type, PlayerUpgradeButton button)
        {
            if (!_playerUpgrades.TryGetValue(type, out PlayerBaseUpgrade upgrade) || stats == null)
                return;

            int amount = MultipleBuyOption.Instance.GetBuyAmount();

            (string value, string bonus, string cost, string count) = upgrade.GetDisplayStrings(stats, amount);
            button.UpdateStats(value, bonus, cost, count);
        }

        private bool CanSpend(ulong cost) => SessionCurrencyManager.Instance.CanSpend(cost);
    }

    public enum PlayerUpgradeType
    {
        MaxHealth,
        RegenAmount,
        RegenInterval
    }
}