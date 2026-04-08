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
            if (_viewModel == null) return;

            // 1. Обновляем логику перемещения и телепортации внутри ViewModel
            _viewModel.Update(Time.deltaTime);

            // 2. Синхронизируем позицию спрайта с данными из Core
            transform.position = _viewModel.Position;

            // 3. Синхронизируем поворот
            // (Unity использует Эйлеровы углы для Z, наша модель тоже)
            transform.rotation = Quaternion.Euler(0, 0, _viewModel.Rotation);
        }
    }
}