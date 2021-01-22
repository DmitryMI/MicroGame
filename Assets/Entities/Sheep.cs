using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Factories;
using Assets.MicroGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Entities
{
    public class Sheep : Micro
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
            return (entity is Food);
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

            Sheep sheep = (Sheep)_breedingFactory.CreateEntity(GameManager, Position + (Vector2)Random.onUnitSphere);
            sheep.FoodLeft = FoodLeft / 2;
            FoodLeft = FoodLeft / 2;
        }
    }
}
