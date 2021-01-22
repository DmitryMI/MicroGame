using System.Collections.Generic;
using Assets.MicroGame;
using UnityEngine;

namespace Assets.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _size;

        protected List<IEventReceiver> EventReceiversList = new List<IEventReceiver>();
        protected IUpdateProvider UpdateProviderInternal;
        public IUpdateProvider UpdateProvider
        {
            get => UpdateProviderInternal;
            set
            {
                UpdateProviderInternal?.Remove(this);

                UpdateProviderInternal = value;
                UpdateProviderInternal.Add(this);
            }
        }
        public abstract void OnUpdate(IUpdateProvider sender, float deltaTime);
        public bool IsUpdateableActive => this != null && gameObject != null;
        public IEventReceiver[] EventReceivers => EventReceiversList.ToArray();
        public IMapProvider MapProvider { get; set; }

        public Vector2 Position
        {
            get
            {
                if (this != null && gameObject != null)
                {
                    return transform.position;
                }
                return Vector2.positiveInfinity;
            }
        }

        public Vector2Int CellPosition => MapProvider.TranslateToCell(Position);
        public abstract double NutritionValue { get; }


        protected void ReportDeath()
        {
            foreach (var receiver in EventReceiversList)
            {
                receiver.ReportDeath(this);
            }
        }

        public abstract void OnEaten(IEntity eatingEntity);

        public void OnDeath()
        {
            IsAlive = false;
            ReportDeath();
            UpdateProvider?.Remove(this);
            OnEntityDeath();
            Destroy(gameObject);
        }

        protected abstract void OnEntityDeath();

        public void AddEventReceiver(IEventReceiver eventReceiver)
        {
            EventReceiversList.Add(eventReceiver);
        }

        public bool IsAlive { get; protected set; } = true;
        public float Size => _size;
    }
}