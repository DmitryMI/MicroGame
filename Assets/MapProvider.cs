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

        [SerializeField] private BoxCollider2D _mapCollider2D;

        private List<IEntity> _entities = new List<IEntity>();
        public IList<IEntity> Entities => _entities;
        public int MapCellularWidth => _width;
        public int MapCellularHeight => _height;
        public float MapRealWidth => CellWidth * MapCellularWidth;
        public float MapRealHeight => CellHeight * MapCellularHeight;
        public float CellWidth => 1;
        public float CellHeight => 1;
        public Vector2 MapCenter => _mapCenter;

        private void Start()
        {
            if (_autoSetSize)
            {
                float height = _camera.orthographicSize;
                float width = height * _camera.aspect;
                _height = 2 * (int) (height / CellHeight);
                _width = 2 * (int) (width / CellWidth);

                _mapCenter = _camera.transform.position;
            }
            _mapCollider2D.size = new Vector2(_width * CellWidth, _height * CellHeight);
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
            float x = cellPosition.x * CellWidth - MapCellularWidth * CellWidth / 2 + _mapCenter.x;
            float y = cellPosition.y * CellHeight - MapCellularHeight * CellHeight / 2 + _mapCenter.y;
            return new Vector2(x, y);
        }

        private void OnTriggerExit2D(Collider2D colliderExiting)
        {
            var obj = colliderExiting.gameObject;
            float x = obj.transform.position.x;
            float y = obj.transform.position.y;

            float mapWidth = CellWidth * MapCellularWidth;
            float mapHeight = CellHeight * MapCellularHeight;
            if (x < _mapCenter.x - mapWidth / 2)
            {
                x = _mapCenter.x - mapWidth / 2;
            }

            if (y < _mapCenter.y - mapHeight / 2)
            {
                y = _mapCenter.y - mapHeight / 2;
            }

            if (x >= _mapCenter.x + mapWidth / 2)
            {
                x = _mapCenter.x + mapWidth / 2 - 0.01f;
            }
            if (y >= _mapCenter.y + mapHeight / 2)
            {
                y = _mapCenter.y + mapHeight / 2 - 0.01f;
            }

            obj.transform.position = new Vector3(x, y);
        }
    }
}