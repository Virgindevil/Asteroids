using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PresentationInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectileManager>().AsSingle();
            
            Container.BindFactory<ProjectileView, ProjectileView.Factory>()
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransformGroup("Bullets");
        }
    }
}