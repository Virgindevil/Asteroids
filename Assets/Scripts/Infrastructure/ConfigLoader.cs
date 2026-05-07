using System.Collections.Generic;
using System.IO;
using Game.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Infrastructure
{
    public class ConfigLoader
    {
        public ConfigLoader()
        {
            Player = LoadFile<PlayerConfig>("PlayerSettings.json");
            World = LoadFile<WorldConfig>("WorldSettings.json");
            Bullet = LoadFile<BulletSettings>("BulletSettings.json");
            Enemies = LoadFile<List<EnemyConfig>>("EnemiesSettings.json");
        }

        public PlayerConfig Player { get; private set; }
        public WorldConfig World { get; private set; }
        public BulletSettings Bullet { get; private set; }
        public List<EnemyConfig> Enemies { get; private set; }

        private T LoadFile<T>(string fileName)
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);

            var jsonContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
}