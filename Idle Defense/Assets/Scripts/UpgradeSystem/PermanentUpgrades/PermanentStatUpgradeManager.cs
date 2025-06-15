using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.PermanentUpgrades
{
    public class PermanentStatUpgradeManager : MonoBehaviour
    {
        private Dictionary<PermanentStatUpgradeType, PermanentUpgrade> _permanentUpgrades;

        private void Start()
        {
            InitializeUpgrades();
        }

        private void InitializeUpgrades()
        {
            _permanentUpgrades = new Dictionary<PermanentStatUpgradeType, PermanentUpgrade>
            {
                [PermanentStatUpgradeType.DamageMultiplier] = new()
                {
                    Upgrade = (p, a) =>
                    {
                        //TODO
                    }
                }
            };
        }

        public enum PermanentStatUpgradeType
        {
            DamageMultiplier,
        }
    }
}