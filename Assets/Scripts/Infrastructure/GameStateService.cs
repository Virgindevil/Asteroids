using UnityEngine;

namespace Game.Infrastructure
{
    public class GameStateService
    {
        public bool IsPaused { get; private set; }

        public void PauseGame()
        {
            IsPaused = true;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}