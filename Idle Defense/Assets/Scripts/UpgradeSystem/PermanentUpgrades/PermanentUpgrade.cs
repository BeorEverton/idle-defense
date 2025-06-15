using System;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.PermanentUpgrades
{
    public class PermanentUpgrade
    {
        public Action<PermanentStatsInstance, int> Upgrade;
    }
}