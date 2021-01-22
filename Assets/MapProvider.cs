using System.Collections.Generic;
using Assets.MicroGame;
using UnityEngine;

namespace Assets
{
    public class MapProvider : MonoBehaviour, IMapProvider
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField] private bool _autoSetSize;

        [SerializeField]
        private int _width, _height;

        [SerializeField] private Vector2 _mapCenter;

        private List<IEntity> _entities = new List<IEntity>();
        public IList<IEntity> Entities => _entities;
        public int MapCellularWidth => _width;
        public int MapCellularHeight => _height;
        public float CellWidth => 1;
        public float CellHeight => 1;

        private void Start()
        {
            if (_autoSetSize)
            {
                float height = _camera.orthographicSize;
                float width = height * _camera.aspect;
                _height = (int) (height / CellHeight);
                _width = (int) (width / CellWidth);

                _mapCenter = _camera.transform.position;
            }
        }

        public IEntity[] GetCellEntities(int x, int y)
        {
            List<IEntity> result = new List<IEntity>();
            for (int i = 0; i < Entities.Count; i++)
            {
                IEntity entity = Entities[i];
                int entityX = entity.CellPosition.x;
                int entityY = entity.CellPosition.y;
                if (entityX == x && entityY == y)
                {
                    result.Add(entity);
                }
            }

            return result.ToArray();
        }

        public bool IsPassable(int x, int y)
        {
            return true;
        }

        public bool IsCellInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < MapCellularWidth && y < MapCellularHeight;
        }

        public Vector2Int TranslateToCell(Vector2 position)
        {
            float sX = position.x + MapCellularWidth * CellWidth / 2 - _mapCenter.x;
            float sY = position.y + MapCellularHeight * CellHeight / 2 - _mapCenter.y;
            int x = (int)(sX / CellWidth);
            int y = (int)(sY / CellHeight);
            return new Vector2Int(x, y);
        }

        public Vector2 TranslateToReal(Vector2Int cellPosition)
        {
            float x = cellPosition.x * CellWidth * 1.5f - MapCellularWidth * CellWidth / 2 + _mapCenter.x;
            float y = cellPosition.y * CellHeight * 1.5f - MapCellularHeight * CellHeight / 2 + _mapCenter.y;
            return new Vector2(x, y);
        }
    }
}