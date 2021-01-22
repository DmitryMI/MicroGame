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
    }
}
