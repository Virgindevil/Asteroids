using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    public class PlayerInvincibilityView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _invincibilityParticles;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<InvincibleEffectActiveSignal>(OnInvincibilityChanged);
        }

        private void OnInvincibilityChanged(InvincibleEffectActiveSignal signal)
        {
            //Debug.Log($"<color=yellow>[PlayerView]</color> Received Signal. Active: {signal.IsActive}");
            if (signal.IsActive) _invincibilityParticles.Play();
            else _invincibilityParticles.Stop();
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<InvincibleEffectActiveSignal>(OnInvincibilityChanged);
        }
    }
}