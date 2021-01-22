using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MicroGame;

namespace Assets.Controllers
{
    public class ClosestProximityController : IMicroController
    {
        private float _deltaTime;
        protected IUpdateProvider UpdateProviderInternal;
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

        private void ControllerLoop()
        {
            if (Controllable == null)
            {
                return;
            }
            IMapProvider mapProvider = Controllable.MapProvider;
            IEntity closestFood = null;
            float minDistance = float.PositiveInfinity;
            foreach (var entity in mapProvider.Entities)
            {
                if (!entity.IsAlive)
                {
                    continue;
                }

                if (!Controllable.WantToEat(entity))
                {
                    continue;
                }

                if (Controllable.CanEat(entity))
                {
                    Controllable.Eat(entity);
                    return;
                }

                float distance = (entity.Position - Controllable.Position).sqrMagnitude;
                if (minDistance >= distance)
                {
                    minDistance = distance;
                    closestFood = entity;
                }
            }

            if (closestFood == null)
            {
                return;
            }

            Controllable.Move(closestFood.Position);
        }

        public IMicro Controllable { get; set; }

        public ClosestProximityController(IUpdateProvider updateProvider)
        {
            UpdateProvider = updateProvider;
        }
    }
}
