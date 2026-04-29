using UnityEngine;

namespace Game.Core
{
    public class PlayerViewModel
    {
        private readonly PlayerModel _model;
        private readonly MapService _mapService;

        public PlayerViewModel(PlayerModel model, WorldConfig worldConfig, MapService mapService)
        {
            _model = model;
            _mapService = mapService;
        }

        // Пробрасываем данные для отображения
        public Vector2 Position => _model.Body.Position;
        public float Rotation => _model.Body.Rotation;
        public float RoundRotation => (_model.Body.Rotation % PhysicsBody.RoundDegree + PhysicsBody.RoundDegree) % PhysicsBody.RoundDegree;
        public float Speed => _model.Body.Velocity.magnitude;

        // Количество целых готовых зарядов
        public int ReadyCharges => Mathf.FloorToInt(_model.LaserCharge);

        // Процент восстановления следующего заряда (для визуальной шкалы или таймера)
        public float RechargeProgress => _model.LaserCharge % 1f;

        public void Update(float deltaTime)
        {
            // Безопасная проверка: если чего-то нет, просто не считаем физику в этом кадре
            if (_model?.Body == null) 
                return;

            _model.Body.UpdatePhysics(deltaTime);
            _model.Body.TeleportIfOutOfBounds(_mapService.Width, _mapService.Height);
        }

        // Возвращаем остаток времени до восстановления СЛЕДУЮЩЕГО заряда
        public float NextChargeCooldown
        {
            get
            {
                if (_model.LaserCharge >= _model.Config.MaxLaserCharges) return 0f;

                // Получаем дробную часть заряда и переводим её в секунды
                float partialCharge = _model.LaserCharge - Mathf.Floor(_model.LaserCharge);
                return (1f - partialCharge) * _model.Config.LaserCooldown;
            }
        }

        // Можно даже подготовить готовую строку здесь, если View совсем простая
        // Готовая строка для UI (View просто обращается к этому свойству)
        public string LaserStatusText
        {
            get
            {
                if (ReadyCharges >= _model.Config.MaxLaserCharges)
                    return $"LASER: {ReadyCharges} (READY)";

                // Считаем сколько секунд осталось до +1 заряда
                float secondsLeft = (1f - RechargeProgress) * _model.Config.LaserCooldown;
                return $"LASER: {ReadyCharges} | NEXT: {secondsLeft:F1}s";
            }
        }

        public float GetChargeForSlider(int index)
        {
            return Mathf.Clamp01(_model.LaserCharge - index);
        }
    }
}