using Assets.MicroGame;
using UnityEngine;

namespace Assets
{
    public class RandomGameFactory : IGameFactory
    {
        public IGameManager GameManager { get; }
        public IMapProvider MapProvider { get; }
        public IEntityFactory FoodFactory { get; }
        public IEntityFactory SheepFactory { get; }
        public IEntityFactory WolfFactory { get; }

        public int SheepsCount { get; set; }
        public int WolvesCount { get; set; }
        public int FoodCount { get; set; }

        private void CreateUniformSpread(IEntityFactory factory, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int x = Random.Range(0, MapProvider.MapCellularWidth);
                int y = Random.Range(0, MapProvider.MapCellularHeight);
                Vector2 position = MapProvider.TranslateToReal(new Vector2Int(x, y));
                IEntity food = factory.CreateEntity(GameManager, position);
                MapProvider.Entities.Add(food);
            }
        }

        public void CreateStaticFood()
        {
            CreateUniformSpread(FoodFactory, FoodCount);
        }

        public void CreateSheeps()
        {
            CreateUniformSpread(SheepFactory, SheepsCount);
        }

        public void CreateWolves()
        {
            CreateUniformSpread(WolfFactory, WolvesCount);
        }

        public RandomGameFactory(IGameManager gameManager, IMapProvider mapProvider, IEntityFactory foodFactory, IEntityFactory sheepFactory, IEntityFactory wolfFactory)
        {
            MapProvider = mapProvider;
            FoodFactory = foodFactory;
            SheepFactory = sheepFactory;
            WolfFactory = wolfFactory;
            GameManager = gameManager;
        }
    }
}