using UnityEngine;

namespace Assets.MicroGame
{
    public interface IMicro : IEntity
    {
        IMicroController Controller { get; set; }
        float MoveSpeed { get; }
        double FoodLeft { get; }
        double LifetimeLeft { get; }
        bool CanBreed { get; }

        bool CanEat(IEntity entity);
        bool WantToEat(IEntity entity);
        void Breed();
        void Move(Vector2 towards);
        void Eat(IEntity entity);
    }
}