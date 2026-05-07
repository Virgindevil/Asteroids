using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PlayerInvincibilityView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _invincibilityParticles;
        private SignalBus _signalBus;

        private void Start()
        {
            _signalBus.Subscribe<InvincibleEffectActiveSignal>(OnInvincibilityChanged);
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<InvincibleEffectActiveSignal>(OnInvincibilityChanged);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnInvincibilityChanged(InvincibleEffectActiveSignal signal)
        {
            if (signal.IsActive) _invincibilityParticles.Play();
            else _invincibilityParticles.Stop();
        }
    }
}