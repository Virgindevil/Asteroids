using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerViewModel _viewModel;


        private void Update()
        {
            transform.position = _viewModel.Position;
            transform.rotation = Quaternion.Euler(0, 0, _viewModel.Rotation);
        }

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}