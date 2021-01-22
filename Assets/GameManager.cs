using Assets.Controllers;
using Assets.Factories;
using Assets.MicroGame;
using UnityEngine;

namespace Assets
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private UpdateProvider _updateProvider;

        [SerializeField] private MapProvider _mapProvider;

        [SerializeField] private int _foodCount;
        [SerializeField] private int _sheepsCount;
        [SerializeField] private int _wolvesCount;

        [SerializeField] private float _controllerLoopPeriod;

        private IGameFactory _gameFactory;

        public IUpdateProvider UpdateProvider
        {
            get => _updateProvider;
            set => _updateProvider = (UpdateProvider)value;
        }

        public void OnUpdate(IUpdateProvider sender, float deltaTime)
        {
            
        }

        public bool IsUpdateableActive => gameObject != null;

        void Start()
        {
            _updateProvider.Add(this);
            var controllerUpdateProvider = _updateProvider.CreateSubProvider(_controllerLoopPeriod);
            var factory = new RandomGameFactory(this, MapProvider,
                new FoodFactory(_mapProvider.transform),
                new SheepFactory(_updateProvider, controllerUpdateProvider, _mapProvider.transform),
                new WolfFactory(_updateProvider, controllerUpdateProvider, _mapProvider.transform)
            ) {SheepsCount = _sheepsCount, WolvesCount = _wolvesCount, FoodCount = _foodCount};
            _gameFactory = factory;
            CreateGame();
        }

        public IMapProvider MapProvider => _mapProvider;
        public IGameFactory GameFactory => _gameFactory;
        public void OnSheepsExtinction()
        {
            
        }

        public void OnWolvesExtinction()
        {
            
        }

        public void CreateGame()
        {
            _gameFactory.CreateStaticFood();
            _gameFactory.CreateSheeps();
            _gameFactory.CreateWolves();
        }

        public void StartGame()
        {
            
        }

        public void ReportDeath(IEntity dyingEntity)
        {
            
        }
    }
}