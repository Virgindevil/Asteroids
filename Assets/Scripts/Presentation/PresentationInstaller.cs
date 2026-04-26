using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PresentationInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _asteroidPrefab;
        [SerializeField] private GameObject _ufoPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<ProjectileView, ProjectileView.Factory>()
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransformGroup("Bullets");

            //Container.BindInterfacesTo<EnemyViewManager>().AsSingle();
            //Container.BindInterfacesAndSelfTo<EnemyViewManager>().AsSingle();

            Container.BindFactory<Vector2, AsteroidView, AsteroidView.Factory>()
                .FromComponentInNewPrefab(_asteroidPrefab)
                .UnderTransformGroup("Enemies");

            // Создаем вторую фабрику для НЛО (нужно создать класс UfoView аналогичный AsteroidView)
            Container.BindFactory<Vector2, UfoView, UfoView.Factory>()
                .FromComponentInNewPrefab(_ufoPrefab)
                .UnderTransformGroup("Enemies");

            Container.BindInterfacesAndSelfTo<EnemyViewManager>().AsSingle();
            
            // Для пуль
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle();
            
            
            // Для коллизий
            Container.BindInterfacesAndSelfTo<CollisionManager>().AsSingle().NonLazy();
            
            // MVVM
            Container.Bind<GameOverViewModel>().AsSingle();
            
            Container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle();
        }
    }
}