using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MicroGame;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Assets.Controllers
{
    public class SheepRunnerController : IMicroController
    {
        private const float DangerDistance = 5f;

        private float _deltaTime;
        protected IUpdateProvider UpdateProviderInternal { get; set; }
        public IUpdateProvider UpdateProvider
        {
            get => UpdateProviderInternal;
            set
            {
                UpdateProviderInternal?.Remove(this);

                UpdateProviderInternal = value;
                UpdateProviderInternal.Add(this);
            }
        }
        public void OnUpdate(IUpdateProvider sender, float deltaTime)
        {
            _deltaTime = deltaTime;
            ControllerLoop();
        }

        public bool IsUpdateableActive => true;

        private void GetClosestEntities(out IEntity closestFood, out IEntity closestEnemy)
        {
            IMapProvider mapProvider = Controllable.MapProvider;
            closestFood = null;
            closestEnemy = null;
            float minFoodDistance = float.PositiveInfinity;
            float minEnemyDistance = float.PositiveInfinity;
            foreach (var entity in mapProvider.Entities)
            {
                bool isFood = false;
                bool isEnemy = false;

                if (!entity.IsAlive)
                {
                    continue;
                }

                isFood = Controllable.WantToEat(entity);

                if (entity is IMicro micro)
                {
                    isEnemy = micro.WantToEat(Controllable);
                }

                if (isFood)
                {
                    float distance = (entity.Position - Controllable.Position).sqrMagnitude;
                    if (minFoodDistance >= distance)
                    {
                        minFoodDistance = distance;
                        closestFood = entity;
                    }
                }
                else if (isEnemy)
                {
                    float distance = (entity.Position - Controllable.Position).sqrMagnitude;
                    if (minEnemyDistance >= distance)
                    {
                        minEnemyDistance = distance;
                        closestEnemy = entity;
                    }
                }
            }
        }

        private void MotionDeciderLoop()
        {
            GetClosestEntities(out var closestFood, out var closestEnemy);

            float distanceToEnemy = float.PositiveInfinity;
            if (closestFood != null)
            {
                if (Controllable.CanEat(closestFood))
                {
                    Controllable.Eat(closestFood);
                }
            }

            if (closestEnemy != null)
            {
                distanceToEnemy = (closestEnemy.Position - Controllable.Position).sqrMagnitude;
            }

            if (distanceToEnemy > DangerDistance * DangerDistance && closestFood != null)
            {
                Controllable.Move(closestFood.Position);
            }
            else if(distanceToEnemy <= DangerDistance * DangerDistance && closestEnemy != null)
            {
                IMapProvider mapProvider = Controllable.MapProvider;
                Vector2 opposite = -(closestEnemy.Position - Controllable.Position);
                Vector2 direction = opposite.normalized;
                Vector2 targetPoint = Controllable.Position + direction * DangerDistance;

                float mapMinX = mapProvider.MapCenter.x - mapProvider.MapRealWidth / 2;
                float mapMinY = mapProvider.MapCenter.y - mapProvider.MapRealHeight / 2;
                float mapMaxX = mapProvider.MapCenter.x + mapProvider.MapRealWidth / 2;
                float mapMaxY = mapProvider.MapCenter.y + mapProvider.MapRealHeight / 2;

                if (targetPoint.x < mapMinX)
                {
                    if (Controllable.Position.y < closestEnemy.Position.y)
                    {
                        targetPoint.y -= DangerDistance;
                    }
                    else
                    {
                        targetPoint.y += DangerDistance;
                    }

                    targetPoint.x = mapMinX + Controllable.Size;
                }
                if (targetPoint.x >= mapMaxX)
                {
                    if (Controllable.Position.y < closestEnemy.Position.y)
                    {
                        targetPoint.y -= DangerDistance;
                    }
                    else
                    {
                        targetPoint.y += DangerDistance;
                    }
                    targetPoint.x = mapMaxX - Controllable.Size;
                }
                if (targetPoint.y < mapMinY)
                {
                    if (Controllable.Position.x < closestEnemy.Position.x)
                    {
                        targetPoint.x -= DangerDistance;
                    }
                    else
                    {
                        targetPoint.x += DangerDistance;
                    }
                    targetPoint.y = mapMinY + Controllable.Size;
                }
                if (targetPoint.y >= mapMaxY)
                {
                    if (Controllable.Position.x < closestEnemy.Position.x)
                    {
                        targetPoint.x -= DangerDistance;
                    }
                    else
                    {
                        targetPoint.x += DangerDistance;
                    }
                    targetPoint.y = mapMaxY - Controllable.Size;
                }

                Controllable.Move(targetPoint);
                
            }
        }

        private void BreedingDeciderLoop()
        {
            if (Controllable.CanBreed)
            {
                Controllable.Breed();
            }
        }

        private void ControllerLoop()
        {
            if (Controllable == null)
            {
                return;
            }

            MotionDeciderLoop();
            BreedingDeciderLoop();
        }

        public IMicro Controllable { get; set; }

        public SheepRunnerController(IUpdateProvider updateProvider)
        {
            UpdateProvider = updateProvider;
        }
    }
}
