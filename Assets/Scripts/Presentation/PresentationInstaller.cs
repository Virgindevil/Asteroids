using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Presentation
{
    public class PresentationInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _asteroidPrefab;
        [SerializeField] private GameObject _ufoPrefab;
        
        [SerializeField] private UnityCameraProvider _cameraProvider;
        [SerializeField] private GameOverView _gameOverView;
        
        public override void InstallBindings()
        {
            Container.Bind<ICameraProvider>()
                .FromInstance(_cameraProvider)
                .AsSingle();
            
            Container.BindFactory<ProjectileView, ProjectileView.Factory>()
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransformGroup("Bullets");

            Container.BindFactory<Vector2, AsteroidView, AsteroidView.Factory>()
                .FromComponentInNewPrefab(_asteroidPrefab)
                .UnderTransformGroup("Enemies");

            Container.BindFactory<Vector2, UfoView, UfoView.Factory>()
                .FromComponentInNewPrefab(_ufoPrefab)
                .UnderTransformGroup("Enemies");

            Container.Bind<GameOverViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyViewManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverView>()
                .FromInstance(_gameOverView)
                .AsSingle();

        }
    }
}