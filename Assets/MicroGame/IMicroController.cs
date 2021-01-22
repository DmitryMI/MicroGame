namespace Assets.MicroGame
{
    public interface IMicroController : IUpdateable
    {
        IMicro Controllable { get; set; }
    }
}