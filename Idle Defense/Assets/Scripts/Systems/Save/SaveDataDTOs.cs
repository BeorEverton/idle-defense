#nullable enable
using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using Assets.Scripts.Structs;
using Assets.Scripts.UpgradeSystem.PlayerBaseUpgrades;
using Assets.Scripts.UpgradeSystem.TurretUpgrades;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Systems.Save
{
    public static class SaveDataDTOs
    {
        public static GameDataDTO CreateGameDataDTO(int waveNumber, ulong money)
        {
            return new GameDataDTO
            {
                WaveNumber = waveNumber,
                Money = money,
                TutorialStep = GameTutorialManager.Instance != null ? GameTutorialManager.Instance._currentStep : 0
            };
        }

        public static PlayerInfoDTO CreatePlayerInfoDTO(PlayerBaseStatsInstance player)
        {
            return new PlayerInfoDTO
            {
                MaxHealth = player.MaxHealth,
                RegenAmount = player.RegenAmount,
                RegenInterval = player.RegenInterval,
                MaxHealthUpgradeAmount = player.Stats[PlayerBaseStatType.MaxHealth].UpgradeAmount,
                MaxHealthUpgradeBaseCost = player.Stats[PlayerBaseStatType.MaxHealth].BaseCost,
                MaxHealthLevel = player.Stats[PlayerBaseStatType.MaxHealth].Level,
                RegenAmountUpgradeAmount = player.Stats[PlayerBaseStatType.RegenAmount].UpgradeAmount,
                RegenAmountUpgradeBaseCost = player.Stats[PlayerBaseStatType.RegenAmount].BaseCost,
                RegenAmountLevel = player.Stats[PlayerBaseStatType.RegenAmount].Level,
                RegenIntervalUpgradeAmount = player.Stats[PlayerBaseStatType.RegenInterval].UpgradeAmount,
                RegenIntervalUpgradeBaseCost = player.Stats[PlayerBaseStatType.RegenInterval].BaseCost,
                RegenIntervalLevel = player.Stats[PlayerBaseStatType.RegenInterval].Level
            };
        }

        public static TurretBaseInfoDTO? CreateTurretBaseInfoDTO(TurretStatsInstance? turret)
        {
            if (turret == null)
                return null;

            return new TurretBaseInfoDTO
            {
                BaseDamage = turret.BaseDamage,
                BaseFireRate = turret.BaseFireRate,
                BaseCritChance = turret.BaseCritChance,
                BaseCritDamage = turret.BaseCritDamage,
                DamageCostExponentialMultiplier = turret.Stats[TurretStatType.Damage].ExponentialCostMultiplier,
                FireRateCostExponentialMultiplier = turret.Stats[TurretStatType.FireRate].ExponentialCostMultiplier,
                CriticalChanceCostExponentialMultiplier = turret.Stats[TurretStatType.CriticalChance].ExponentialCostMultiplier,
                CriticalDamageCostExponentialMultiplier = turret.Stats[TurretStatType.CriticalDamage].ExponentialCostMultiplier,
            };
        }

        public static TurretInfoDTO? CreateTurretInfoDTO(TurretStatsInstance? turret)
        {
            if (turret == null)
                return null;

            return new TurretInfoDTO
            {
                IsUnlocked = turret.IsUnlocked,

                Damage = turret.Stats[TurretStatType.Damage].Value,
                DamageLevel = turret.Stats[TurretStatType.Damage].Level,
                DamageUpgradeAmount = turret.Stats[TurretStatType.Damage].UpgradeAmount,
                DamageUpgradeBaseCost = turret.Stats[TurretStatType.Damage].BaseCost,

                FireRate = turret.Stats[TurretStatType.FireRate].Value,
                FireRateLevel = turret.Stats[TurretStatType.FireRate].Level,
                FireRateUpgradeAmount = turret.Stats[TurretStatType.FireRate].UpgradeAmount,
                FireRateUpgradeBaseCost = turret.Stats[TurretStatType.FireRate].BaseCost,

                CriticalChance = turret.Stats[TurretStatType.CriticalChance].Value,
                CriticalChanceLevel = turret.Stats[TurretStatType.CriticalChance].Level,
                CriticalChanceUpgradeAmount = turret.Stats[TurretStatType.CriticalChance].UpgradeAmount,
                CriticalChanceUpgradeBaseCost = turret.Stats[TurretStatType.CriticalChance].BaseCost,

                CriticalDamageMultiplier = turret.Stats[TurretStatType.CriticalDamage].Value,
                CriticalDamageMultiplierLevel = turret.Stats[TurretStatType.CriticalDamage].Level,
                CriticalDamageMultiplierUpgradeAmount = turret.Stats[TurretStatType.CriticalDamage].UpgradeAmount,
                CriticalDamageMultiplierUpgradeBaseCost = turret.Stats[TurretStatType.CriticalDamage].BaseCost,

                ExplosionRadius = turret.Stats[TurretStatType.ExplosionRadius].Value,
                ExplosionRadiusLevel = turret.Stats[TurretStatType.ExplosionRadius].Level,
                ExplosionRadiusUpgradeAmount = turret.Stats[TurretStatType.ExplosionRadius].UpgradeAmount,
                ExplosionRadiusUpgradeBaseCost = turret.Stats[TurretStatType.ExplosionRadius].BaseCost,

                SplashDamage = turret.Stats[TurretStatType.SplashDamage].Value,
                SplashDamageLevel = turret.Stats[TurretStatType.SplashDamage].Level,
                SplashDamageUpgradeAmount = turret.Stats[TurretStatType.SplashDamage].UpgradeAmount,
                SplashDamageUpgradeBaseCost = turret.Stats[TurretStatType.SplashDamage].BaseCost,

                PierceChance = turret.Stats[TurretStatType.PierceChance].Value,
                PierceChanceLevel = turret.Stats[TurretStatType.PierceChance].Level,
                PierceChanceUpgradeAmount = turret.Stats[TurretStatType.PierceChance].UpgradeAmount,
                PierceChanceUpgradeBaseCost = turret.Stats[TurretStatType.PierceChance].BaseCost,

                PierceDamageFalloff = turret.Stats[TurretStatType.PierceDamageFalloff].Value,
                PierceDamageFalloffLevel = turret.Stats[TurretStatType.PierceDamageFalloff].Level,
                PierceDamageFalloffUpgradeAmount = turret.Stats[TurretStatType.PierceDamageFalloff].UpgradeAmount,
                PierceDamageFalloffUpgradeBaseCost = turret.Stats[TurretStatType.PierceDamageFalloff].BaseCost,

                PelletCount = (int)turret.Stats[TurretStatType.PelletCount].Value,
                PelletCountLevel = turret.Stats[TurretStatType.PelletCount].Level,
                PelletCountUpgradeAmount = (int)turret.Stats[TurretStatType.PelletCount].UpgradeAmount,
                PelletCountUpgradeBaseCost = turret.Stats[TurretStatType.PelletCount].BaseCost,

                DamageFalloffOverDistance = turret.Stats[TurretStatType.DamageFalloffOverDistance].Value,
                DamageFalloffOverDistanceLevel = turret.Stats[TurretStatType.DamageFalloffOverDistance].Level,
                DamageFalloffOverDistanceUpgradeAmount = turret.Stats[TurretStatType.DamageFalloffOverDistance].UpgradeAmount,
                DamageFalloffOverDistanceUpgradeBaseCost = turret.Stats[TurretStatType.DamageFalloffOverDistance].BaseCost,

                PercentBonusDamagePerSec = turret.Stats[TurretStatType.PercentBonusDamagePerSec].Value,
                PercentBonusDamagePerSecLevel = turret.Stats[TurretStatType.PercentBonusDamagePerSec].Level,
                PercentBonusDamagePerSecUpgradeAmount = turret.Stats[TurretStatType.PercentBonusDamagePerSec].UpgradeAmount,
                PercentBonusDamagePerSecUpgradeBaseCost = turret.Stats[TurretStatType.PercentBonusDamagePerSec].BaseCost,

                SlowEffect = turret.Stats[TurretStatType.SlowEffect].Value,
                SlowEffectLevel = turret.Stats[TurretStatType.SlowEffect].Level,
                SlowEffectUpgradeAmount = turret.Stats[TurretStatType.SlowEffect].UpgradeAmount,
                SlowEffectUpgradeBaseCost = turret.Stats[TurretStatType.SlowEffect].BaseCost,

                RotationSpeed = turret.RotationSpeed,
                AngleThreshold = turret.AngleThreshold
            };
        }

        public static StatsDTO CreateStatsDTO()
        {
            return new StatsDTO
            {
                TotalDamage = StatsManager.Instance.TotalDamage,
                MaxZone = StatsManager.Instance.MaxZone,
                TotalZonesSecured = StatsManager.Instance.TotalZonesSecured,
                EnemiesKilled = StatsManager.Instance.EnemiesKilled,
                BossesKilled = StatsManager.Instance.BossesKilled,
                MoneySpent = StatsManager.Instance.MoneySpent,
                UpgradeAmount = StatsManager.Instance.UpgradeAmount,
                TotalDamageTaken = StatsManager.Instance.TotalDamageTaken,
                TotalHealthRepaired = StatsManager.Instance.TotalHealthRepaired,
                MissionsFailed = StatsManager.Instance.MissionsFailed,
                SpeedBoostClicks = StatsManager.Instance.SpeedBoostClicks,
                MachineGunDamage = StatsManager.Instance.MachineGunDamage,
                ShotgunDamage = StatsManager.Instance.ShotgunDamage,
                SniperDamage = StatsManager.Instance.SniperDamage,
                MissileLauncherDamage = StatsManager.Instance.MissileLauncherDamage,
                LaserDamage = StatsManager.Instance.LaserDamage
            };
        }
    }

    public static class LoadDataDTOs
    {
        public static PlayerBaseStatsInstance CreatePlayerBaseSO(PlayerInfoDTO playerInfo)
        {
            PlayerBaseStatsInstance playerStats = new()
            {
                MaxHealth = playerInfo.MaxHealth,
                RegenAmount = playerInfo.RegenAmount,
                RegenInterval = playerInfo.RegenInterval,

                Stats =
                {
                    [PlayerBaseStatType.MaxHealth] = new PlayerBaseStat
                    {
                        UpgradeAmount = playerInfo.MaxHealth,
                        BaseCost = playerInfo.MaxHealthUpgradeBaseCost,
                        Level = playerInfo.MaxHealthLevel
                    },
                    [PlayerBaseStatType.RegenAmount] = new PlayerBaseStat
                    {
                        UpgradeAmount = playerInfo.RegenAmount,
                        BaseCost = playerInfo.RegenAmountUpgradeBaseCost,
                        Level = playerInfo.RegenAmountLevel
                    },
                    [PlayerBaseStatType.RegenInterval] = new PlayerBaseStat
                    {
                        UpgradeAmount = playerInfo.RegenInterval,
                        BaseCost = playerInfo.RegenIntervalUpgradeBaseCost,
                        Level = playerInfo.RegenIntervalLevel
                    }
                }
            };

            return playerStats;
        }

        public static TurretStatsInstance CreateTurretStatsInstance(TurretInfoDTO turret, TurretBaseInfoDTO baseInfo)
        {
            return new TurretStatsInstance
            {
                IsUnlocked = turret.IsUnlocked,
                BaseDamage = baseInfo.BaseDamage,
                BaseFireRate = baseInfo.BaseFireRate,
                BaseCritChance = baseInfo.BaseCritChance,
                BaseCritDamage = baseInfo.BaseCritDamage,

                Stats =
                {
                    [TurretStatType.Damage] = new TurretStat
                    {
                        Value = turret.Damage,
                        UpgradeAmount = turret.DamageUpgradeAmount,
                        BaseCost = turret.DamageUpgradeBaseCost,
                        Level = turret.DamageLevel,
                        ExponentialCostMultiplier = baseInfo.DamageCostExponentialMultiplier
                    },
                    [TurretStatType.FireRate] = new TurretStat
                    {
                        Value = turret.FireRate,
                        UpgradeAmount = turret.FireRateUpgradeAmount,
                        BaseCost = turret.FireRateUpgradeBaseCost,
                        Level = turret.FireRateLevel,
                        ExponentialCostMultiplier = baseInfo.FireRateCostExponentialMultiplier
                    },
                    [TurretStatType.CriticalChance] = new TurretStat
                    {
                        Value = turret.CriticalChance,
                        UpgradeAmount = turret.CriticalChanceUpgradeAmount,
                        BaseCost = turret.CriticalChanceUpgradeBaseCost,
                        Level = turret.CriticalChanceLevel,
                        ExponentialCostMultiplier = baseInfo.CriticalChanceCostExponentialMultiplier
                    },
                    [TurretStatType.CriticalDamage] = new TurretStat
                    {
                        Value = turret.CriticalDamageMultiplier,
                        UpgradeAmount = turret.CriticalDamageMultiplierUpgradeAmount,
                        BaseCost = turret.CriticalDamageMultiplierUpgradeBaseCost,
                        Level = turret.CriticalDamageMultiplierLevel,
                        ExponentialCostMultiplier = baseInfo.CriticalDamageCostExponentialMultiplier
                    },
                    [TurretStatType.ExplosionRadius] = new TurretStat
                    {
                        Value = turret.ExplosionRadius,
                        UpgradeAmount = turret.ExplosionRadiusUpgradeAmount,
                        BaseCost = turret.ExplosionRadiusUpgradeBaseCost,
                        Level = turret.ExplosionRadiusLevel,
                        ExponentialCostMultiplier = baseInfo.ExplosionRadiusCostExponentialMultiplier
                    },
                },

                SplashDamage = turret.SplashDamage,
                SplashDamageLevel = turret.SplashDamageLevel,
                SplashDamageUpgradeAmount = turret.SplashDamageUpgradeAmount,
                SplashDamageUpgradeBaseCost = turret.SplashDamageUpgradeBaseCost,
                PierceChance = turret.PierceChance,
                PierceChanceLevel = turret.PierceChanceLevel,
                PierceChanceUpgradeAmount = turret.PierceChanceUpgradeAmount,
                PierceChanceUpgradeBaseCost = turret.PierceChanceUpgradeBaseCost,
                PierceDamageFalloff = turret.PierceDamageFalloff,
                PierceDamageFalloffLevel = turret.PierceDamageFalloffLevel,
                PierceDamageFalloffUpgradeAmount = turret.PierceDamageFalloffUpgradeAmount,
                PierceDamageFalloffUpgradeBaseCost = turret.PierceDamageFalloffUpgradeBaseCost,
                PelletCount = turret.PelletCount,
                PelletCountLevel = turret.PelletCountLevel,
                PelletCountUpgradeAmount = turret.PelletCountUpgradeAmount,
                PelletCountUpgradeBaseCost = turret.PelletCountUpgradeBaseCost,
                DamageFalloffOverDistance = turret.DamageFalloffOverDistance,
                DamageFalloffOverDistanceLevel = turret.DamageFalloffOverDistanceLevel,
                DamageFalloffOverDistanceUpgradeAmount = turret.DamageFalloffOverDistanceUpgradeAmount,
                DamageFalloffOverDistanceUpgradeBaseCost = turret.DamageFalloffOverDistanceUpgradeBaseCost,
                PercentBonusDamagePerSec = turret.PercentBonusDamagePerSec,
                PercentBonusDamagePerSecLevel = turret.PercentBonusDamagePerSecLevel,
                PercentBonusDamagePerSecUpgradeAmount = turret.PercentBonusDamagePerSecUpgradeAmount,
                PercentBonusDamagePerSecUpgradeBaseCost = turret.PercentBonusDamagePerSecUpgradeBaseCost,
                SlowEffect = turret.SlowEffect,
                SlowEffectLevel = turret.SlowEffectLevel,
                SlowEffectUpgradeAmount = turret.SlowEffectUpgradeAmount,
                SlowEffectUpgradeBaseCost = turret.SlowEffectUpgradeBaseCost,
                RotationSpeed = turret.RotationSpeed,
                AngleThreshold = turret.AngleThreshold
            };
        }
    }
}

[Serializable]
public class GameDataDTO
{
    public int WaveNumber;
    public ulong Money;
    public int TutorialStep;
}

[Serializable]
public class PlayerInfoDTO
{
    public int MaxHealth;
    public float RegenAmount;
    public float RegenInterval;

    public float MaxHealthUpgradeAmount;
    public ulong MaxHealthUpgradeBaseCost;
    public int MaxHealthLevel;

    public float RegenAmountUpgradeAmount;
    public ulong RegenAmountUpgradeBaseCost;
    public int RegenAmountLevel;

    public float RegenIntervalUpgradeAmount;
    public ulong RegenIntervalUpgradeBaseCost;
    public int RegenIntervalLevel;
}

[Serializable]
public class TurretBaseInfoDTO
{
    public float BaseDamage;
    public float BaseFireRate;
    public float BaseCritChance;
    public float BaseCritDamage;
    public float DamageCostExponentialMultiplier;
    public float FireRateCostExponentialMultiplier;
    public float CriticalChanceCostExponentialMultiplier;
    public float CriticalDamageCostExponentialMultiplier;
    public float ExplosionRadiusCostExponentialMultiplier;
    public float SplashDamageCostExponentialMultiplier;
    public float PierceChanceCostExponentialMultiplier;
    public float PierceDamageFalloffCostExponentialMultiplier;
    public float PelletCountCostExponentialMultiplier;
    public float DamageFalloffOverDistanceCostExponentialMultiplier;
    public float PercentBonusDamagePerSecCostExponentialMultiplier;
    public float SlowEffectCostExponentialMultiplier;
    public float KnockbackStrengthCostExponentialMultiplier;
}

[Serializable]
public class TurretInfoDTO
{
    public bool IsUnlocked;

    public float Damage;
    public int DamageLevel;
    public float DamageUpgradeAmount;
    public float DamageUpgradeBaseCost;

    public float FireRate;
    public int FireRateLevel;
    public float FireRateUpgradeAmount;
    public float FireRateUpgradeBaseCost;

    public float CriticalChance;
    public int CriticalChanceLevel;
    public float CriticalChanceUpgradeAmount;
    public float CriticalChanceUpgradeBaseCost;

    public float CriticalDamageMultiplier;
    public int CriticalDamageMultiplierLevel;
    public float CriticalDamageMultiplierUpgradeAmount;
    public float CriticalDamageMultiplierUpgradeBaseCost;

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
}

[Serializable]
public class TurretInventoryDTO
{
    public List<TurretStatsInstance> Owned;
    public List<int> EquippedIds;
    public List<TurretType> UnlockedTypes;
    public List<bool> SlotPurchased;
}

[Serializable]
public class StatsDTO
{
    public double TotalDamage;
    public int MaxZone;
    public int TotalZonesSecured;
    public int EnemiesKilled;
    public int BossesKilled;
    public double MoneySpent;
    public int UpgradeAmount;
    public double TotalDamageTaken;
    public double TotalHealthRepaired;
    public int MissionsFailed;
    public int SpeedBoostClicks;

    public double MachineGunDamage;
    public double ShotgunDamage;
    public double SniperDamage;
    public double MissileLauncherDamage;
    public double LaserDamage;
}