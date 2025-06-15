using System;

namespace Assets.Scripts.UpgradeSystem.PermanentUpgrades
{
    [Serializable]
    public class PermanentUpgradeMeta : IUpgradeMeta
    {
        public string Type;
        public string DisplayName;
        public string Description;

        string IUpgradeMeta.Type => Type;
        string IUpgradeMeta.DisplayName => DisplayName;
        string IUpgradeMeta.Description => Description;
    }
}