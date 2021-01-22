using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Controllers;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Factories
{
    public class SheepFactory : EntityFactory
    {
        private const string PrefabPath = "Prefabs/Sheep";
        public SheepFactory(IUpdateProvider microUpdateProvider, IUpdateProvider controllerUpdateProvider, Transform parentTransform = null)
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
                SheepRunnerController closestProximityController =
                    new SheepRunnerController(ControllerUpdateProvider);
                closestProximityController.Controllable = micro;
                micro.Controller = closestProximityController;
                micro.BreedingFactory = this;
            }

            return entity;
        }

    }
}
