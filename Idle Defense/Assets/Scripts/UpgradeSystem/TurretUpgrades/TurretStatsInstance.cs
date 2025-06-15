using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.TurretUpgrades
{
    /// <summary>
    /// This class is used to store and upgrade the stats of a turret instance.
    /// </summary>
    [System.Serializable]
    public class TurretStatsInstance
    {
        public bool IsUnlocked;
        public TurretType TurretType;   // add at top – nothing else changes
        public Dictionary<TurretStatType, TurretStat> Stats = new();

        [Header("Base Stats")]
        //DO NOT TOUCH AT RUNTIME
        public float BaseDamage;
        public float BaseFireRate;
        public float BaseCritChance;
        public float BaseCritDamage;

        public float CriticalDamageMultiplier;
        public int CriticalDamageMultiplierLevel;
        public float CriticalDamageMultiplierUpgradeAmount;
        public float CriticalDamageMultiplierUpgradeBaseCost;
        public float CriticalDamageCostExponentialMultiplier;

        public float ExplosionRadius;
        public int ExplosionRadiusLevel;
        public float ExplosionRadiusUpgradeAmount;
        public float ExplosionRadiusUpgradeBaseCost;

        public float SplashDamage;
        public int SplashDamageLevel;
        public float SplashDamageUpgradeAmount;
        public float SplashDamageUpgradeBaseCost;

        public float PierceChance;
        public int PierceChanceLevel;
        public float PierceChanceUpgradeAmount;
        public float PierceChanceUpgradeBaseCost;

        public float PierceDamageFalloff;
        public int PierceDamageFalloffLevel;
        public float PierceDamageFalloffUpgradeAmount;
        public float PierceDamageFalloffUpgradeBaseCost;

        public int PelletCount;
        public int PelletCountLevel;
        public int PelletCountUpgradeAmount;
        public float PelletCountUpgradeBaseCost;

        public float DamageFalloffOverDistance;
        public int DamageFalloffOverDistanceLevel;
        public float DamageFalloffOverDistanceUpgradeAmount;
        public float DamageFalloffOverDistanceUpgradeBaseCost;

        public float KnockbackStrength;
        public int KnockbackStrengthLevel;
        public float KnockbackStrengthUpgradeAmount;
        public float KnockbackStrengthUpgradeBaseCost;
        public float KnockbackStrengthCostExponentialMultiplier;

        public float PercentBonusDamagePerSec;
        public int PercentBonusDamagePerSecLevel;
        public float PercentBonusDamagePerSecUpgradeAmount;
        public float PercentBonusDamagePerSecUpgradeBaseCost;

        public float SlowEffect;
        public int SlowEffectLevel;
        public float SlowEffectUpgradeAmount;
        public float SlowEffectUpgradeBaseCost;

        public float RotationSpeed;
        public float AngleThreshold;

        public TurretStatsInstance(TurretInfoSO source)
        {
            IsUnlocked = source.IsUnlocked;
            BaseDamage = source.Damage;
            BaseFireRate = source.FireRate;
            BaseCritChance = source.CriticalChance;
            BaseCritDamage = source.CriticalDamageMultiplier;

            Stats[TurretStatType.Damage] = new TurretStat
            {
                Value = source.Damage,
                Level = source.DamageLevel,
                UpgradeAmount = source.DamageUpgradeAmount,
                BaseCost = source.DamageUpgradeBaseCost,
                ExponentialCostMultiplier = source.DamageCostExponentialMultiplier
            };
            Stats[TurretStatType.FireRate] = new TurretStat
            {
                Value = source.FireRate,
                Level = source.FireRateLevel,
                UpgradeAmount = source.FireRateUpgradeAmount,
                BaseCost = source.FireRateUpgradeBaseCost,
                ExponentialCostMultiplier = source.FireRateCostExponentialMultiplier
            };
            Stats[TurretStatType.CriticalChance] = new TurretStat
            {
                Value = source.CriticalChance,
                Level = source.CriticalChanceLevel,
                UpgradeAmount = source.CriticalChanceUpgradeAmount,
                BaseCost = source.CriticalChanceUpgradeBaseCost,
                ExponentialCostMultiplier = source.CriticalChanceCostExponentialMultiplier
            };
            Stats[TurretStatType.CriticalDamage] = new TurretStat
            {
                Value = source.CriticalDamageMultiplier,
                Level = source.CriticalDamageMultiplierLevel,
                UpgradeAmount = source.CriticalDamageMultiplierUpgradeAmount,
                BaseCost = source.CriticalDamageMultiplierUpgradeBaseCost,
                ExponentialCostMultiplier = source.CriticalDamageCostExponentialMultiplier
            };
            Stats[TurretStatType.ExplosionRadius] = new TurretStat
            {
                Value = source.ExplosionRadius,
                Level = source.ExplosionRadiusLevel,
                UpgradeAmount = source.ExplosionRadiusUpgradeAmount,
                BaseCost = source.ExplosionRadiusUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.SplashDamage] = new TurretStat
            {
                Value = source.SplashDamage,
                Level = source.SplashDamageLevel,
                UpgradeAmount = source.SplashDamageUpgradeAmount,
                BaseCost = source.SplashDamageUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.PierceChance] = new TurretStat
            {
                Value = source.PierceChance,
                Level = source.PierceChanceLevel,
                UpgradeAmount = source.PierceChanceUpgradeAmount,
                BaseCost = source.PierceChanceUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.PierceDamageFalloff] = new TurretStat
            {
                Value = source.PierceDamageFalloff,
                Level = source.PierceDamageFalloffLevel,
                UpgradeAmount = source.PierceDamageFalloffUpgradeAmount,
                BaseCost = source.PierceDamageFalloffUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.PelletCount] = new TurretStat
            {
                Value = source.PelletCount,
                Level = source.PelletCountLevel,
                UpgradeAmount = source.PelletCountUpgradeAmount,
                BaseCost = source.PelletCountUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.DamageFalloffOverDistance] = new TurretStat
            {
                Value = source.DamageFalloffOverDistance,
                Level = source.DamageFalloffOverDistanceLevel,
                UpgradeAmount = source.DamageFalloffOverDistanceUpgradeAmount,
                BaseCost = source.DamageFalloffOverDistanceUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.KnockbackStrength] = new TurretStat
            {
                Value = source.KnockbackStrength,
                Level = source.KnockbackStrengthLevel,
                UpgradeAmount = source.KnockbackStrengthUpgradeAmount,
                BaseCost = source.KnockbackStrengthUpgradeBaseCost,
                ExponentialCostMultiplier = source.KnockbackStrengthCostExponentialMultiplier
            };
            Stats[TurretStatType.PercentBonusDamagePerSec] = new TurretStat
            {
                Value = source.PercentBonusDamagePerSec,
                Level = source.PercentBonusDamagePerSecLevel,
                UpgradeAmount = source.PercentBonusDamagePerSecUpgradeAmount,
                BaseCost = source.PercentBonusDamagePerSecUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };
            Stats[TurretStatType.SlowEffect] = new TurretStat
            {
                Value = source.SlowEffect,
                Level = source.SlowEffectLevel,
                UpgradeAmount = source.SlowEffectUpgradeAmount,
                BaseCost = source.SlowEffectUpgradeBaseCost,
                ExponentialCostMultiplier = 1f
            };

            RotationSpeed = source.RotationSpeed;
            AngleThreshold = source.AngleThreshold;
        }

        public TurretStatsInstance() { }//Used to load from DTO
    }
}