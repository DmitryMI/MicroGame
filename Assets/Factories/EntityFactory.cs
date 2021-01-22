using Assets.Controllers;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Factories
{
    public abstract class EntityFactory : IEntityFactory
    {
        protected GameObject Prefab { get; set; }
        protected IUpdateProvider MicroUpdateProvider { get; set; }
        protected IUpdateProvider ControllerUpdateProvider { get; set; }
        protected Transform ParentTransform { get; set; }

        protected IEntity InstantiatePrefab(GameObject prefab, IGameManager gameManager, Vector2 position)
        {
            GameObject instance = Object.Instantiate(Prefab, position, Quaternion.identity, ParentTransform);
            IEntity entity = instance.GetComponent<IEntity>();
            entity.AddEventReceiver(gameManager);
            entity.MapProvider = gameManager.MapProvider;
            entity.GameManager = gameManager;

            gameManager.MapProvider.Entities.Add(entity);
            return entity;
        }

        public abstract IEntity CreateEntity(IGameManager gameManager, Vector2 position);
    }
}