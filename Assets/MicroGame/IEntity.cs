using UnityEngine;

namespace Assets.MicroGame
{
    public interface IEntity : IUpdateable
    {
        IEventReceiver[] EventReceivers { get; }
        IMapProvider MapProvider { get; set; }
        Vector2 Position { get; }
        Vector2Int CellPosition { get; }
        double NutritionValue { get; }

        void OnEaten(IEntity eatingEntity);
        void OnDeath();
        void AddEventReceiver(IEventReceiver eventReceiver);

        bool IsAlive { get; }

        float Size { get; }
    }
}