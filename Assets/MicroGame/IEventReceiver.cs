namespace Assets.MicroGame
{
    public interface IEventReceiver
    {
        void ReportDeath(IEntity dyingEntity);
    }
}