using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MicroGame
{
    public interface IMapProvider
    {
        IList<IEntity> Entities { get; }

        int MapCellularWidth { get; }
        int MapCellularHeight { get; }
        float CellWidth { get; }
        float CellHeight { get; }

        IEntity[] GetCellEntities(int x, int y);

        bool IsPassable(int x, int y);

        bool IsCellInBounds(int x, int y);

        Vector2Int TranslateToCell(Vector2 position);
        Vector2 TranslateToReal(Vector2Int cellPosition);
    }
}