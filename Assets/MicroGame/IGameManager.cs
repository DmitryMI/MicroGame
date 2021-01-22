namespace Assets.MicroGame
{
    public interface IGameManager : IUpdateable, IEventReceiver
    {
        IMapProvider MapProvider { get; }
        IGameFactory GameFactory { get; }

        void OnSheepsExtinction();
        void OnWolvesExtinction();
        void CreateGame();
        void StartGame();
    }
}