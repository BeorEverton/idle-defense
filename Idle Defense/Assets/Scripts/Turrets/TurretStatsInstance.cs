using Assets.Scripts.SO;

namespace Assets.Scripts.Turrets
{
    /// <summary>
    /// This class is used to store and upgrade the stats of a turret instance.
    /// </summary>
    [System.Serializable]
    public class TurretStatsInstance
    {
        public bool IsUnlocked;

        public float Damage;
        public float DamageLevel;
        public float DamageUpgradeAmount;
        public float DamageUpgradeBaseCost;

        public float FireRate;
        public float FireRateLevel;
        public float FireRateUpgradeAmount;
        public float FireRateUpgradeBaseCost;

        public float CriticalChance;
        public float CriticalChanceLevel;
        public float CriticalChanceUpgradeAmount;
        public float CriticalChanceUpgradeBaseCost;

        public float CriticalDamageMultiplier;
        public float CriticalDamageMultiplierLevel;
        public float CriticalDamageMultiplierUpgradeAmount;
        public float CriticalDamageMultiplierUpgradeBaseCost;

        public float ExplosionRadius;
        public float ExplosionRadiusLevel;
        public float ExplosionRadiusUpgradeAmount;
        public float ExplosionRadiusUpgradeBaseCost;

        public float SplashDamage;
        public float SplashDamageLevel;
        public float SplashDamageUpgradeAmount;
        public float SplashDamageUpgradeBaseCost;

        public float PierceChance;
        public int PierceChanceLevel;
        public float PierceChanceUpgradeAmount;
        public float PierceChanceUpgradeBaseCost;

        public float PierceDamageFalloff;
        public float PierceDamageFalloffLevel;
        public float PierceDamageFalloffUpgradeAmount;
        public float PierceDamageFalloffUpgradeBaseCost;

        public int PelletCount;
        public int PelletCountLevel;
        public int PelletCountUpgradeAmount;
        public float PelletCountUpgradeBaseCost;

        public float DamageFalloffOverDistance;
        public float DamageFalloffOverDistanceLevel;
        public float DamageFalloffOverDistanceUpgradeAmount;
        public float DamageFalloffOverDistanceUpgradeBaseCost;

        public float PercentBonusDamagePerSec;
        public float PercentBonusDamagePerSecLevel;
        public float PercentBonusDamagePerSecUpgradeAmount;
        public float PercentBonusDamagePerSecUpgradeBaseCost;

        public float SlowEffect;
        public float SlowEffectLevel;
        public float SlowEffectUpgradeAmount;
        public float SlowEffectUpgradeBaseCost;

        public float RotationSpeed;
        public float AngleThreshold;

        public TurretStatsInstance(TurretInfoSO source)
        {
            IsUnlocked = source.IsUnlocked;

            Damage = source.Damage;
            DamageLevel = source.DamageLevel;
            DamageUpgradeAmount = source.DamageUpgradeAmount;
            DamageUpgradeBaseCost = source.DamageUpgradeBaseCost;

            FireRate = source.FireRate;
            FireRateLevel = source.FireRateLevel;
            FireRateUpgradeAmount = source.FireRateUpgradeAmount;
            FireRateUpgradeBaseCost = source.FireRateUpgradeBaseCost;

            CriticalChance = source.CriticalChance;
            CriticalChanceLevel = source.CriticalChanceLevel;
            CriticalChanceUpgradeAmount = source.CriticalChanceUpgradeAmount;
            CriticalChanceUpgradeBaseCost = source.CriticalChanceUpgradeBaseCost;

            CriticalDamageMultiplier = source.CriticalDamageMultiplier;
            CriticalDamageMultiplierLevel = source.CriticalDamageMultiplierLevel;
            CriticalDamageMultiplierUpgradeAmount = source.CriticalDamageMultiplierUpgradeAmount;
            CriticalDamageMultiplierUpgradeBaseCost = source.CriticalDamageMultiplierUpgradeBaseCost;

            ExplosionRadius = source.ExplosionRadius;
            ExplosionRadiusLevel = source.ExplosionRadiusLevel;
            ExplosionRadiusUpgradeAmount = source.ExplosionRadiusUpgradeAmount;
            ExplosionRadiusUpgradeBaseCost = source.ExplosionRadiusUpgradeBaseCost;

            SplashDamage = source.SplashDamage;
            SplashDamageLevel = source.SplashDamageLevel;
            SplashDamageUpgradeAmount = source.SplashDamageUpgradeAmount;
            SplashDamageUpgradeBaseCost = source.SplashDamageUpgradeBaseCost;

            PierceChance = source.PierceChance;
            PierceChanceLevel = source.PierceChanceLevel;
            PierceChanceUpgradeAmount = source.PierceChanceUpgradeAmount;
            PierceChanceUpgradeBaseCost = source.PierceChanceUpgradeBaseCost;

            PierceDamageFalloff = source.PierceDamageFalloff;
            PierceDamageFalloffLevel = source.PierceDamageFalloffLevel;
            PierceDamageFalloffUpgradeAmount = source.PierceDamageFalloffUpgradeAmount;
            PierceDamageFalloffUpgradeBaseCost = source.PierceDamageFalloffUpgradeBaseCost;

            PelletCount = source.PelletCount;
            PelletCountLevel = source.PelletCountLevel;
            PelletCountUpgradeAmount = source.PelletCountUpgradeAmount;
            PelletCountUpgradeBaseCost = source.PelletCountUpgradeBaseCost;

            DamageFalloffOverDistance = source.DamageFalloffOverDistance;
            DamageFalloffOverDistanceLevel = source.DamageFalloffOverDistanceLevel;
            DamageFalloffOverDistanceUpgradeAmount = source.DamageFalloffOverDistanceUpgradeAmount;
            DamageFalloffOverDistanceUpgradeBaseCost = source.DamageFalloffOverDistanceUpgradeBaseCost;

            PercentBonusDamagePerSec = source.PercentBonusDamagePerSec;
            PercentBonusDamagePerSecLevel = source.PercentBonusDamagePerSecLevel;
            PercentBonusDamagePerSecUpgradeAmount = source.PercentBonusDamagePerSecUpgradeAmount;
            PercentBonusDamagePerSecUpgradeBaseCost = source.PercentBonusDamagePerSecUpgradeBaseCost;

            SlowEffect = source.SlowEffect;
            SlowEffectLevel = source.SlowEffectLevel;
            SlowEffectUpgradeAmount = source.SlowEffectUpgradeAmount;
            SlowEffectUpgradeBaseCost = source.SlowEffectUpgradeBaseCost;

            RotationSpeed = source.RotationSpeed;
            AngleThreshold = source.AngleThreshold;
        }

        public TurretStatsInstance() //Used to load from DTO
        {
        }

        public int GetUpgradeCost(string statName)
        {
            return statName switch
            {
                nameof(DamageLevel) => (int)(DamageLevel * DamageUpgradeBaseCost),
                nameof(FireRateLevel) => (int)(FireRateLevel * FireRateUpgradeBaseCost),
                nameof(CriticalChanceLevel) => (int)(CriticalChanceLevel * CriticalChanceUpgradeBaseCost),
                nameof(CriticalDamageMultiplierLevel) => (int)(CriticalDamageMultiplierLevel * CriticalDamageMultiplierUpgradeBaseCost),
                nameof(ExplosionRadiusLevel) => (int)(ExplosionRadiusLevel * ExplosionRadiusUpgradeBaseCost),
                nameof(SplashDamageLevel) => (int)(SplashDamageLevel * SplashDamageUpgradeBaseCost),
                nameof(PierceChanceLevel) => (int)(PierceChanceLevel * PierceChanceUpgradeBaseCost),
                nameof(PierceDamageFalloffLevel) => (int)(PierceDamageFalloffLevel * PierceDamageFalloffUpgradeBaseCost),
                nameof(PelletCountLevel) => (int)(PelletCountLevel * PelletCountUpgradeBaseCost),
                nameof(DamageFalloffOverDistanceLevel) => (int)(DamageFalloffOverDistanceLevel * DamageFalloffOverDistanceUpgradeBaseCost),
                nameof(PercentBonusDamagePerSecLevel) => (int)(PercentBonusDamagePerSecLevel * PercentBonusDamagePerSecUpgradeBaseCost),
                nameof(SlowEffectLevel) => (int)(SlowEffectLevel * SlowEffectUpgradeBaseCost),
                _ => 0
            };
        }
    }
}