using Assets.Scripts.Helpers;
using Assets.Scripts.UpgradeSystem.TurretUpgrades;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem
{
    public static class GenericMetaManager
    {
        private static Dictionary<System.Type, object> _metaDict = new();

        public static void Load<T>(string path) where T : IUpgradeMeta
        {
            if (_metaDict.ContainsKey(typeof(T))) //Already loaded
                return;

            TextAsset json = Resources.Load<TextAsset>(path);

            if (json == null)
            {
                Debug.LogError("Failed to load TurretUpgradeMeta.json");
                _metaDict[typeof(T)] = new Dictionary<string, T>();
                return;
            }

            T[] metas = JsonHelper.FromJson<T>(json.text);
            Dictionary<string, T> metaDict = new();

            foreach (T meta in metas)
                metaDict[meta.Type] = meta;

            _metaDict[typeof(T)] = metaDict;
        }

        public static TMeta GetMeta<TMeta, TEnum>(TEnum enumValue, string path)
            where TMeta : IUpgradeMeta
            where TEnum : Enum
        {
            Load<TMeta>(path);
            Dictionary<string, TMeta> dict = _metaDict[typeof(TMeta)] as Dictionary<string, TMeta>;

            dict.TryGetValue(enumValue.ToString(), out TMeta meta);

            return meta;
        }
    }
}
