using System.IO;
using UnityEngine;
using Game.Core;
using Newtonsoft.Json;
using Zenject; 

namespace Game.Infrastructure
{
    public class ConfigLoader : IInitializable 
    {
        public GameSettingsRoot Root { get; private set; }

        public void Initialize()
        {
            Load();
        }

        private void Load()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "GameSettings.json");
            if (!File.Exists(path)) 
                return;

            string jsonContent = File.ReadAllText(path);
            Root = JsonConvert.DeserializeObject<GameSettingsRoot>(jsonContent);
            Debug.Log("<color=green>[ConfigLoader] Game Settings loaded successfully!</color>");
        }
    }
}