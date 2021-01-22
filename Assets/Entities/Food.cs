using Assets.MicroGame;
using UnityEngine;

namespace Assets.Entities
{
    public class Food : Entity
    {
        [SerializeField]
        private double _nutritionValue = 100;
        public override double NutritionValue => _nutritionValue;

        public override void OnUpdate(IUpdateProvider sender, float deltaTime)
        {
            
        }

        public override void OnEaten(IEntity eatingEntity)
        {
            OnDeath();
        }

        protected override void OnEntityDeath()
        {
            
        }
    }
}