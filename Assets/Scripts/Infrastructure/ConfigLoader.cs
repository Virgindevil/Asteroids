using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Newtonsoft.Json;
using Zenject;

namespace Game.Infrastructure
{
    public class ConfigLoader : IInitializable
    {
        public PlayerConfig Player { get; private set; }
        public WorldConfig World { get; private set; }
        public List<EnemyConfig> Enemies { get; private set; }

        public ConfigLoader()
        {
            Player = LoadFile<PlayerConfig>("PlayerSettings.json");
            World = LoadFile<WorldConfig>("WorldSettings.json");
            Enemies = LoadFile<List<EnemyConfig>>("EnemiesSettings.json");

            // Важная проверка! Если файлы не найдутся, вы увидите это сразу
            if (Player == null || World == null || Enemies == null)
                Debug.LogError("[ConfigLoader] Critical: One or more configs failed to load!");
        }

        public void Initialize()
        {
            LoadAll();
        }

        private void LoadAll()
        {
            Player = LoadFile<PlayerConfig>("PlayerSettings.json");
            World = LoadFile<WorldConfig>("WorldSettings.json");
            Enemies = LoadFile<List<EnemyConfig>>("EnemiesSettings.json");

            Debug.Log("<color=green>[ConfigLoader] All Settings loaded successfully!</color>");
        }

        private T LoadFile<T>(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"[ConfigLoader] File not found: {path}");
                return default;
            }

            string jsonContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
}
