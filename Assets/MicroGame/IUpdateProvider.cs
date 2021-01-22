namespace Assets.MicroGame
{
    public interface IUpdateProvider
    {
        void Add(IUpdateable updateable);
        void Remove(IUpdateable updateable);
        void SendUpdates();
    }
}
