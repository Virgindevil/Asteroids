using UnityEngine;
using TMPro;
using Zenject;
using Game.Core;

namespace Game.UI
{
    public class MainUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private PlayerViewModel _viewModel;

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Update()
        {
            _scoreText.text = $"321";
        }

    }
}