using Assets.Scripts.Enums;
using Assets.Scripts.Structs;
using Assets.Scripts.Systems.Audio;
using Assets.Scripts.Systems.Currency;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.TurretUpgrades
{
    public class TurretUpgradeManager : MonoBehaviour
    {
        public static event Action OnAnyTurretUpgraded;

        [Header("Cost Scaling Settings")]
        [SerializeField] private float quadraticFactor = 0.1f;
        [SerializeField] private float exponentialPower = 1.15f;

        private Dictionary<TurretStatType, TurretUpgrade> _turretUpgrades;

        private void Start()
        {
            InitializeUpgrades();
        }

        #region UpgradeInitialization
        private void InitializeUpgrades()
        {
            _turretUpgrades = new Dictionary<TurretStatType, TurretUpgrade>
            {
                [TurretStatType.Damage] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.Damage].Value,
                    UpgradeTurret = (t, a) =>
                    {
                        float exponent = Mathf.Pow(t.Stats[TurretStatType.Damage].UpgradeAmount, t.Stats[TurretStatType.Damage].Level);

                        TurretStat stat = t.Stats[TurretStatType.Damage];
                        stat.Level += a;
                        stat.Value = t.BaseDamage * exponent + t.Stats[TurretStatType.Damage].Level;
                        t.Stats[TurretStatType.Damage] = stat; // Update the stat in the dictionary
                    },
                    GetLevel = t => t.Stats[TurretStatType.Damage].Level,
                    GetBaseStat = t => t.BaseDamage,
                    GetBaseCost = t => t.Stats[TurretStatType.Damage].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.Damage].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.Damage].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetExponentialCost(t, TurretStatType.Damage, a),
                    //GetAmount = t => GetMaxAmount(t.DamageUpgradeBaseCost, t.DamageCostExponentialMultiplier, t.DamageLevel),
                    GetDisplayStrings = (t, a) =>
                        {
                            float currentDamage = t.Stats[TurretStatType.Damage].Value;
                            float currentLevel = t.Stats[TurretStatType.Damage].Level;
                            float newLevel = currentLevel + a;

                            float projectedExponent = Mathf.Pow(t.Stats[TurretStatType.Damage].UpgradeAmount, newLevel);
                            float projectedDamage = t.BaseDamage * projectedExponent + newLevel;

                            float bonus = projectedDamage - currentDamage;

                            GetExponentialCost(t, TurretStatType.Damage, a, out float cost, out int amount);

                            return (UIManager.AbbreviateNumber(currentDamage),
                                $"+{UIManager.AbbreviateNumber(bonus)}",
                                $"${UIManager.AbbreviateNumber(cost)}",
                                $"{amount}X");
                        }
                },
                [TurretStatType.FireRate] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.FireRate].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.FireRate, t),
                    GetLevel = t => t.Stats[TurretStatType.FireRate].Level,
                    GetBaseStat = t => t.BaseFireRate,
                    GetBaseCost = t => t.Stats[TurretStatType.FireRate].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.FireRate].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.FireRate].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetExponentialCost(t, TurretStatType.FireRate, a),
                    //GetAmount = t => GetMaxAmount(t.FireRateUpgradeBaseCost, t.FireRateCostExponentialMultiplier, t.FireRateLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float currentFireRate = t.Stats[TurretStatType.FireRate].Value;
                        float bonusFireRate = GetBonusAmount(t, TurretStatType.FireRate);
                        GetExponentialCost(t, TurretStatType.FireRate, a, out float cost, out int amount);

                        return (
                            $"{currentFireRate:F2}/s",
                            $"+{bonusFireRate:F2}/s",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.CriticalChance] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.CriticalChance].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.CriticalChance, t),
                    GetLevel = t => t.Stats[TurretStatType.CriticalChance].Level,
                    GetBaseStat = t => t.BaseCritChance,
                    GetBaseCost = t => t.Stats[TurretStatType.CriticalChance].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.CriticalChance].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.CriticalChance].ExponentialCostMultiplier,
                    GetMaxValue = t => 50f,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetExponentialCost(t, TurretStatType.CriticalChance, a),
                    //GetAmount = t => GetMaxAmount(t.CriticalChanceUpgradeBaseCost, t.CriticalChanceCostExponentialMultiplier, t.CriticalChanceLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.CriticalChance].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.CriticalChance);
                        GetExponentialCost(t, TurretStatType.CriticalChance, a, out float cost, out int amount);

                        if (current >= 50f)
                            return ($"{(int)current}%", "Max", "", "0X");

                        return (
                            $"{(int)current}%",
                            $"+{(int)bonus}%",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.CriticalDamage] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.CriticalDamage].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.CriticalDamage, t),
                    GetLevel = t => t.Stats[TurretStatType.CriticalDamage].Level,
                    GetBaseStat = t => t.BaseCritDamage,
                    GetBaseCost = t => t.Stats[TurretStatType.CriticalDamage].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.CriticalDamage].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.CriticalDamage].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetExponentialCost(t, TurretStatType.CriticalDamage, a),
                    //GetAmount = t => GetMaxAmount(t.CriticalDamageMultiplierUpgradeBaseCost, t.CriticalDamageMultiplier, t.CriticalDamageMultiplierLevel),
                    GetDisplayStrings = (t, a) =>
                {
                    float current = t.Stats[TurretStatType.CriticalDamage].Value;
                    float bonus = GetBonusAmount(t, TurretStatType.CriticalDamage);
                    GetExponentialCost(t, TurretStatType.CriticalDamage, a, out float cost, out int amount);

                    return (
                        $"{(int)current}%",
                        $"+{(int)bonus}%",
                        $"${UIManager.AbbreviateNumber(cost)}",
                        $"{amount}X"
                    );
                }
                },
                [TurretStatType.ExplosionRadius] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.ExplosionRadius].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.ExplosionRadius, t),
                    GetLevel = t => t.Stats[TurretStatType.ExplosionRadius].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.ExplosionRadius].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.ExplosionRadius].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.ExplosionRadius].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.ExplosionRadius].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.ExplosionRadius, a),
                    //GetAmount = t => GetMaxAmount(t.ExplosionRadiusUpgradeBaseCost, 1.1f, t.ExplosionRadiusLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.ExplosionRadius].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.ExplosionRadius);
                        GetHybridCost(t, TurretStatType.ExplosionRadius, a, out float cost, out int amount);

                        if (t.Stats[TurretStatType.ExplosionRadius].Value >= 5f)
                            return ($"{current:F1}", "Max", "", "0X");

                        return (
                            $"{current:F1}",
                            $"+{bonus:F1}",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.SplashDamage] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.SplashDamage].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.SplashDamage, t),
                    GetLevel = t => t.Stats[TurretStatType.SplashDamage].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.SplashDamage].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.SplashDamage].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.SplashDamage].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.SplashDamage].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.SplashDamage, a),
                    //GetAmount = t => GetMaxAmount(t.SplashDamageUpgradeBaseCost, exponentialPower, t.SplashDamageLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.SplashDamage].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.SplashDamage);
                        GetHybridCost(t, TurretStatType.SplashDamage, a, out float cost, out int amount);

                        return (
                            UIManager.AbbreviateNumber(current),
                            $"+{UIManager.AbbreviateNumber(bonus)}",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.PierceChance] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.PierceChance].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.PierceChance, t),

                    GetLevel = t => t.Stats[TurretStatType.PierceChance].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.PierceChance].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.PierceChance].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.PierceChance].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.PierceChance].ExponentialCostMultiplier,
                    GetMaxValue = t => 100f,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.PierceChance, a),
                    //GetAmount = t => GetMaxAmount(t.PierceChanceUpgradeBaseCost, exponentialPower, t.PierceChanceLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.PierceChance].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.PierceChance);
                        GetHybridCost(t, TurretStatType.PierceChance, a, out float cost, out int amount);

                        if (current >= 100f)
                            return ($"{current:F1}%", "Max", "", "0X");

                        return (
                            $"{current:F1}%",
                            $"+{bonus:F1}%",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.PierceDamageFalloff] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.PierceDamageFalloff].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.PierceDamageFalloff, t),
                    GetLevel = t => t.Stats[TurretStatType.PierceDamageFalloff].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.PierceDamageFalloff].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.PierceDamageFalloff].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.PierceDamageFalloff].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.PierceDamageFalloff].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.PierceDamageFalloff, a),
                    //GetAmount = t => GetMaxAmount(t.PierceDamageFalloffUpgradeBaseCost, exponentialPower, t.PierceDamageFalloffLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float currentFalloff = t.Stats[TurretStatType.PierceDamageFalloff].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.PierceDamageFalloff);
                        GetHybridCost(t, TurretStatType.PierceDamageFalloff, a, out float cost, out int amount);

                        return (
                            $"{currentFalloff:F1}%",
                            $"+{bonus:F1}%",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.PelletCount] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.PelletCount].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.PelletCount, t),
                    GetLevel = t => t.Stats[TurretStatType.PelletCount].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.PelletCount].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.PelletCount].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.PelletCount].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.PelletCount].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.PelletCount, a),
                    //GetAmount = t => GetMaxAmount(t.PelletCountUpgradeBaseCost, exponentialPower, t.PelletCountLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.PelletCount].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.PelletCount);
                        GetHybridCost(t, TurretStatType.PelletCount, a, out float cost, out int amount);

                        return (
                            UIManager.AbbreviateNumber(current),
                            $"+{UIManager.AbbreviateNumber(bonus)}",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.DamageFalloffOverDistance] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.DamageFalloffOverDistance].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.DamageFalloffOverDistance, t),
                    GetLevel = t => t.Stats[TurretStatType.DamageFalloffOverDistance].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.DamageFalloffOverDistance].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.DamageFalloffOverDistance].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.DamageFalloffOverDistance].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.DamageFalloffOverDistance].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.DamageFalloffOverDistance, a),
                    //GetAmount = t => GetMaxAmount(t.DamageFalloffOverDistanceUpgradeBaseCost, exponentialPower, t.DamageFalloffOverDistanceLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.DamageFalloffOverDistance].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.DamageFalloffOverDistance);
                        GetHybridCost(t, TurretStatType.DamageFalloffOverDistance, a, out float cost, out int amount);

                        if (current <= 0f)
                            return ($"{current:F1}%", "Max", "", "0X");

                        return (
                                $"{current:F1}%",
                                $"-{bonus:F1}%",
                                $"${UIManager.AbbreviateNumber(cost)}",
                                $"{amount}X"
                            );
                    }
                },
                [TurretStatType.KnockbackStrength] = new()
                {
                    GetCurrentValue = t => t.Stats[TurretStatType.KnockbackStrength].Value,
                    UpgradeTurret = (t, a) => UpgradeTurret(a, TurretStatType.KnockbackStrength, t),
                    GetLevel = t => t.Stats[TurretStatType.KnockbackStrength].Level,
                    GetBaseStat = t => t.Stats[TurretStatType.KnockbackStrength].Value,
                    GetBaseCost = t => t.Stats[TurretStatType.KnockbackStrength].BaseCost,
                    GetUpgradeAmount = t => t.Stats[TurretStatType.KnockbackStrength].UpgradeAmount,
                    GetCostMultiplier = t => t.Stats[TurretStatType.KnockbackStrength].ExponentialCostMultiplier,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.KnockbackStrength, a),
                    //GetAmount = t => GetMaxAmount(t.KnockbackStrengthUpgradeBaseCost, t.KnockbackStrengthCostExponentialMultiplier, t.KnockbackStrengthLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.Stats[TurretStatType.KnockbackStrength].Value;
                        float bonus = GetBonusAmount(t, TurretStatType.KnockbackStrength);
                        GetHybridCost(t, TurretStatType.KnockbackStrength, a, out float cost, out int amount);

                        return (
                            $"{current:F1}",
                            $"+{bonus:F1}",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.PercentBonusDamagePerSec] = new()
                {
                    GetCurrentValue = t => t.PercentBonusDamagePerSec,
                    UpgradeTurret = (t, a) =>
                    {
                        t.PercentBonusDamagePerSecLevel += a;
                        t.PercentBonusDamagePerSec += (t.PercentBonusDamagePerSecUpgradeAmount * a);
                    },
                    GetLevel = t => t.PercentBonusDamagePerSecLevel,
                    GetBaseStat = t => t.PercentBonusDamagePerSec,
                    GetBaseCost = t => t.PercentBonusDamagePerSecUpgradeBaseCost,
                    GetUpgradeAmount = t => t.PercentBonusDamagePerSecUpgradeAmount,
                    GetCostMultiplier = t => 0f,
                    GetMaxValue = t => float.MaxValue,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.PercentBonusDamagePerSec, a),
                    //GetAmount = t => GetMaxAmount(t.PercentBonusDamagePerSecUpgradeBaseCost, exponentialPower, t.PercentBonusDamagePerSecLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.PercentBonusDamagePerSec;
                        float bonus = GetBonusAmount(t, TurretStatType.PercentBonusDamagePerSec);
                        GetHybridCost(t, TurretStatType.PercentBonusDamagePerSec, a, out float cost, out int amount);

                        return (
                            $"{current:F1}%",
                            $"+{bonus:F1}%",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                },
                [TurretStatType.SlowEffect] = new()
                {
                    GetCurrentValue = t => t.SlowEffect,
                    UpgradeTurret = (t, a) =>
                    {
                        t.SlowEffectLevel += a;
                        t.SlowEffect += (t.SlowEffectUpgradeAmount * a);
                    },
                    GetLevel = t => t.SlowEffectLevel,
                    GetBaseStat = t => t.SlowEffect,
                    GetBaseCost = t => t.SlowEffectUpgradeBaseCost,
                    GetUpgradeAmount = t => t.SlowEffectUpgradeAmount,
                    GetCostMultiplier = t => 0f,
                    GetMaxValue = t => 100f,
                    GetMinValue = t => 0f,
                    GetCost = (t, a) => GetHybridCost(t, TurretStatType.SlowEffect, a),
                    //GetAmount = t => GetMaxAmount(t.SlowEffectUpgradeBaseCost, exponentialPower, t.SlowEffectLevel),
                    GetDisplayStrings = (t, a) =>
                    {
                        float current = t.SlowEffect;
                        float bonus = GetBonusAmount(t, TurretStatType.SlowEffect);
                        GetHybridCost(t, TurretStatType.SlowEffect, a, out float cost, out int amount);

                        if (current >= 100f)
                            return ($"{current:F1}%", "Max", "", "0X");

                        return (
                            $"{current:F1}%",
                            $"+{bonus:F1}%",
                            $"${UIManager.AbbreviateNumber(cost)}",
                            $"{amount}X"
                        );
                    }
                }
            };
        }

        private void UpgradeTurret(int levelAmount, TurretStatType type, TurretStatsInstance turret)
        {
            TurretStat stat = turret.Stats[TurretStatType.CriticalDamage];
            stat.Level += levelAmount;
            stat.Value += (turret.Stats[type].UpgradeAmount * levelAmount);
            turret.Stats[TurretStatType.CriticalDamage] = stat; // Update the stat in the dictionary
        }
        #endregion

        public void UpgradeTurretStat(TurretStatsInstance turret, TurretStatType type, TurretUpgradeButton button, int amount)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade))
                return;

            float cost = upgrade.GetCost(turret, amount);

            if (upgrade.GetMaxValue != null && upgrade.GetCurrentValue(turret) >= upgrade.GetMaxValue(turret))
            {
                UpdateUpgradeDisplay(turret, type, button);
                return;
            }

            if (upgrade.GetMinValue != null && upgrade.GetCurrentValue(turret) < upgrade.GetMinValue(turret))
            {
                UpdateUpgradeDisplay(turret, type, button);
                return;
            }

            if (CanSpend((ulong)cost))
            {
                SessionCurrencyManager.Instance.Spend((ulong)cost);

                upgrade.UpgradeTurret(turret, amount);

                AudioManager.Instance.Play("Upgrade");

                button._baseTurret.UpdateTurretAppearance();
                UpdateUpgradeDisplay(turret, type, button);
                button.UpdateInteractableState();

                OnAnyTurretUpgraded?.Invoke();
            }
        }

        private bool CanSpend(ulong cost) => SessionCurrencyManager.Instance.CanSpend((ulong)cost);

        public float GetTurretUpgradeCost(TurretStatsInstance turret, TurretStatType type, int amount) =>
            !_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade) ? 0f : upgrade.GetCost(turret, amount);

        // Used to show Max UpgradeAmount of upgrades available
        //public int GetTurretAvailableUpgradeAmount(TurretStatsInstance turret, TurretStatType type) =>
        //    !_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade) ? 0 : upgrade.GetAmount(turret);

        private int GetMaxAmount(float baseCost, float multiplier, int currentLevel)
        {
            int amount = 0;
            //float totalCost = 0f;
            //float money = SessionCurrencyManager.Instance.SessionCurrency;

            //while (true)
            //{
            //    float cost = baseCost * Mathf.Pow(multiplier, currentLevel + amount);
            //    if (Mathf.Floor(totalCost + cost) > money)
            //        break;
            //    totalCost += cost;
            //    amount++;
            //}

            return amount;
        }

        private float GetBonusAmount(TurretStatsInstance stats, TurretStatType type)
        {
            int amount = MultipleBuyOption.Instance.GetBuyAmount();

            float upgradeAmount = !_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade)
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

        private void GetHybridCost(TurretStatsInstance stats, TurretStatType type, int inAmount, out float cost, out int outAmount)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade))
            {
                cost = 0f;
                outAmount = 0;
                return;
            }

            int level = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            float multiplier = upgrade.GetCostMultiplier(stats);
            float maxLevel = upgrade.GetMaxValue(stats);
            int maxAmount = GetMaxAmount(baseCost, multiplier, level);

            outAmount = inAmount == 9999
                ? maxAmount == 0
                    ? 1
                    : maxAmount
                : inAmount;

            if (level + outAmount > maxLevel)
                outAmount = (int)maxLevel - level;

            if (level < outAmount)
                cost = Mathf.Floor(RecursiveHybridCost(baseCost, level, outAmount));

            cost = Mathf.Floor(baseCost * Mathf.Pow(exponentialPower, level));
        }

        private float GetHybridCost(TurretStatsInstance stats, TurretStatType type, int inAmount)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade))
                return 0f;

            int level = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            float multiplier = upgrade.GetCostMultiplier(stats);
            float maxLevel = upgrade.GetMaxValue(stats);

            int maxAmount = inAmount == 9999
                ? GetMaxAmount(baseCost, multiplier, level)
                : inAmount;

            if (level + maxAmount > maxLevel)
                maxAmount = (int)maxLevel - level;

            return level < maxLevel
                ? Mathf.Floor(RecursiveHybridCost(baseCost, level, maxAmount))
                : baseCost * Mathf.Pow(exponentialPower, level);
        }

        private void GetExponentialCost(TurretStatsInstance stats, TurretStatType type, int inAmount, out float cost, out int outAmount)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade))
            {
                cost = 0f;
                outAmount = 0;
                return;
            }

            int level = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            float multiplier = upgrade.GetCostMultiplier(stats);
            int maxAmount = GetMaxAmount(baseCost, multiplier, level);

            outAmount = inAmount == 9999
                ? maxAmount == 0
                    ? 1
                    : maxAmount
                : inAmount;

            cost = Mathf.Floor(RecursiveExponentialCost(baseCost, multiplier, level, outAmount));
        }

        private float GetExponentialCost(TurretStatsInstance stats, TurretStatType type, int inAmount)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade))
                return 0f;

            int level = upgrade.GetLevel(stats);
            float baseCost = upgrade.GetBaseCost(stats);
            float multiplier = upgrade.GetCostMultiplier(stats);

            int maxAmount = inAmount == 9999
                ? GetMaxAmount(baseCost, multiplier, level)
                : inAmount;

            return Mathf.Floor(RecursiveExponentialCost(baseCost, multiplier, level, maxAmount));
        }

        private float RecursiveHybridCost(float baseCost, int level, int amount)
        {
            if (amount <= 0)
                return 0f;

            float cost = baseCost * (1f + level * level * quadraticFactor);

            return cost + RecursiveHybridCost(baseCost, level + 1, amount - 1);
        }

        private float RecursiveExponentialCost(float baseCost, float multiplier, int level, int amount)
        {
            if (amount <= 0)
                return 0f;

            float cost = baseCost * Mathf.Pow(multiplier, level);

            return cost + RecursiveExponentialCost(baseCost, multiplier, level + 1, amount - 1);
        }

        public void UpdateUpgradeDisplay(TurretStatsInstance turret, TurretStatType type, TurretUpgradeButton button)
        {
            if (!_turretUpgrades.TryGetValue(type, out TurretUpgrade upgrade) || turret == null)
                return;

            int amount = MultipleBuyOption.Instance.GetBuyAmount();

            (string value, string bonus, string cost, string count) = upgrade.GetDisplayStrings(turret, amount);
            button.UpdateStats(value, bonus, cost, count);
        }
    }
}