using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Newtonsoft.Json;
using Zenject;

namespace Game.Infrastructure
{
    public class ConfigLoader
    {
        public PlayerConfig Player { get; private set; }
        public WorldConfig World { get; private set; }
        public BulletSettings Bullet { get; private set; }
        public List<EnemyConfig> Enemies { get; private set; }

        public ConfigLoader()
        {
            Player = LoadFile<PlayerConfig>("PlayerSettings.json");
            World = LoadFile<WorldConfig>("WorldSettings.json");
            Bullet = LoadFile<BulletSettings>("BulletSettings.json");
            Enemies = LoadFile<List<EnemyConfig>>("EnemiesSettings.json");
        }

        private T LoadFile<T>(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);

            string jsonContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
}
