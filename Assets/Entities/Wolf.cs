using Assets.MicroGame;
using UnityEngine;

namespace Assets.Entities
{
    public class Wolf : Micro
    {
        private IEntityFactory _breedingFactory;

        public override IEntityFactory BreedingFactory
        {
            get => _breedingFactory;
            set => _breedingFactory = value;
        }

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
            if (_breedingFactory == null)
            {
                return;
            }

            if (!CanBreed)
            {
                return;
            }

            Wolf wolf = (Wolf)_breedingFactory.CreateEntity(GameManager, Position + (Vector2)Random.onUnitSphere);
            wolf.FoodLeft = FoodLeft / 2;
            FoodLeft = FoodLeft / 2;
        }
    }
}