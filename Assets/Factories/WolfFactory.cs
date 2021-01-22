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

       
    }
}