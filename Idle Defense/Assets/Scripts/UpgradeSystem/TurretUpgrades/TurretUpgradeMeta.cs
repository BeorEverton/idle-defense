using System;

namespace Assets.Scripts.UpgradeSystem.TurretUpgrades
{
    [Serializable]
    public class TurretUpgradeMeta : IUpgradeMeta
    {
        public string Type;
        public string DisplayName;
        public string Description;

        string IUpgradeMeta.Type => Type;
        string IUpgradeMeta.DisplayName => DisplayName;
        string IUpgradeMeta.Description => Description;
    }
}