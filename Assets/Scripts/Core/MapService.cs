using Zenject;

namespace Game.Core
{
    public class MapService : IInitializable
    {
        private readonly ICameraProvider _cameraProvider;

        public MapService(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public void Initialize()
        {
            Height = _cameraProvider.OrthoHeight;
            Width = _cameraProvider.OrthoWidth;
        }
    }
}