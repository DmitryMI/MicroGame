namespace Assets.MicroGame
{
    public interface IUpdateable
    {
        IUpdateProvider UpdateProvider { get; set; }
        void OnUpdate(IUpdateProvider sender, float deltaTime);

        bool IsUpdateableActive { get; }
    }
}