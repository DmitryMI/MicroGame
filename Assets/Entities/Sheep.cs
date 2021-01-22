using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Entities
{
    public class Sheep : Micro
    {
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
            
        }
    }
}
