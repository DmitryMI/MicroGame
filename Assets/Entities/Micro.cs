using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Entities
{
    public abstract class Micro : Entity, IMicro
    {
        [SerializeField]
        private double _baseNutrition;

        [SerializeField] private double _food;

        [SerializeField] private float _moveSpeed;

        [SerializeField] private float _maxLifetime;

        [SerializeField] private double _minFoodForBreeding;

        protected Vector2 MovementTarget { get; set; }

        protected virtual void Start()
        {
            LifetimeLeft = _maxLifetime;
        }

        public override double NutritionValue => _baseNutrition + FoodLeft;
        public override void OnEaten(IEntity eatingEntity)
        {
            OnDeath();
        }

        public IMicroController Controller { get; set; }
        public float MoveSpeed => _moveSpeed;

        public double FoodLeft
        {
            get => _food;
            set => _food = value;
        }
        public double LifetimeLeft { get; set; }
        public bool CanBreed => FoodLeft >= _minFoodForBreeding;
        public abstract bool CanEat(IEntity entity);

        public abstract bool WantToEat(IEntity entity);

        public abstract void Breed();

        public void Eat(IEntity entity)
        {
            if (CanEat(entity))
            {
                FoodLeft += entity.NutritionValue;
                entity.OnEaten(this);
            }
        }

        public void Move(Vector2 towards)
        {
            MovementTarget = towards;
        }

        private void ProcessMovement(float deltaTime)
        {
            Vector2 position = Position;
            Vector2 direction = MovementTarget - position;
            Vector2 motion = direction.normalized;
            Vector2 velocity = motion * MoveSpeed * deltaTime;
            transform.position += new Vector3(velocity.x, velocity.y);
        }

        private void ProcessFood(float deltaTime)
        {
            FoodLeft -= deltaTime;
            if (FoodLeft <= 0)
            {
                OnDeath();
            }
        }

        public override void OnUpdate(IUpdateProvider sender, float deltaTime)
        {
           ProcessMovement(deltaTime);
           ProcessFood(deltaTime);
        }

        protected override void OnEntityDeath()
        {
            Controller?.UpdateProvider?.Remove(Controller);
        }

        private void OnDestroy()
        {
            
        }
    }
}
