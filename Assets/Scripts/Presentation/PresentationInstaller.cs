using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PresentationInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _asteroidPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectileManager>().AsSingle();
            
            Container.BindFactory<ProjectileView, ProjectileView.Factory>()
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransformGroup("Bullets");

            Container.BindInterfacesTo<EnemyViewManager>().AsSingle();

            Container.BindFactory<AsteroidView, AsteroidView.Factory>()
                .FromComponentInNewPrefab(_asteroidPrefab)
                .UnderTransformGroup("Enemies");
        }
    }
}