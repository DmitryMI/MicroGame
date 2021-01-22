using Assets.MicroGame;

namespace Assets.Entities
{
    public class Wolf : Micro
    {
        public override bool CanEat(IEntity entity)
        {
            return (entity.Position - Position).sqrMagnitude < (Size / 2 + entity.Size / 2) * (Size / 2 + entity.Size / 2);
        }

        public override bool WantToEat(IEntity entity)
        {
            return (entity is Sheep);
        }

        public override void Breed()
        {
            
        }
    }
}