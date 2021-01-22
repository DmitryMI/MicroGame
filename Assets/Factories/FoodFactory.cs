using Assets.MicroGame;
using UnityEngine;

namespace Assets.Factories
{
    public class FoodFactory : EntityFactory
    {
        private const string PrefabPath = "Prefabs/Food";
        public FoodFactory(Transform parentTransform = null)
        {
            Prefab = (GameObject)Resources.Load(PrefabPath, typeof(GameObject));
            ParentTransform = parentTransform;
        }

        public override IEntity CreateEntity(IGameManager gameManager, Vector2 position)
        {
            return InstantiatePrefab(Prefab, gameManager, position);
        }
    }
}