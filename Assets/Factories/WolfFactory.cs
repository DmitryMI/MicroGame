using Assets.Controllers;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Factories
{
    public class WolfFactory : EntityFactory
    {
        private const string PrefabPath = "Prefabs/Wolf";
        public WolfFactory(IUpdateProvider microUpdateProvider, IUpdateProvider controllerUpdateProvider, Transform parentTransform = null)
        {
            MicroUpdateProvider = microUpdateProvider;
            ControllerUpdateProvider = controllerUpdateProvider;
            Prefab = (GameObject)Resources.Load(PrefabPath, typeof(GameObject));
            ParentTransform = parentTransform;
        }


        public override IEntity CreateEntity(IGameManager gameManager, Vector2 position)
        {
            IEntity entity = InstantiatePrefab(Prefab, gameManager, position);
            if (entity is IMicro micro)
            {
                micro.UpdateProvider = MicroUpdateProvider;
                ClosestProximityController closestProximityController =
                    new ClosestProximityController(ControllerUpdateProvider);
                closestProximityController.Controllable = micro;
                micro.Controller = closestProximityController;
                micro.BreedingFactory = this;
            }

            return entity;
        }
    }
}