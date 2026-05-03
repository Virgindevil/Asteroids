using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerViewModel _viewModel;

        [Inject]
        public void Construct(PlayerViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        private void Update()
        {
            if (_viewModel == null) 
                return;

            transform.position = _viewModel.Position;

            transform.rotation = Quaternion.Euler(0, 0, _viewModel.Rotation);
        }
    }
}