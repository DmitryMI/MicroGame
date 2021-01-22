namespace Assets.MicroGame
{
    public interface IGameFactory
    {
        IGameManager GameManager { get; }
        IMapProvider MapProvider { get; }
        IEntityFactory FoodFactory { get; }
        IEntityFactory SheepFactory { get; }
        IEntityFactory WolfFactory { get; }

        void CreateStaticFood();
        void CreateSheeps();
        void CreateWolves();
    }
}