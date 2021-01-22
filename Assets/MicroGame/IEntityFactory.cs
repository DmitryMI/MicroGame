using UnityEngine;

namespace Assets.MicroGame
{
    public interface IEntityFactory
    {
        IEntity CreateEntity(IGameManager gameManager, Vector2 position);
    }
}