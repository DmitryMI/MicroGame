using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Assets.MicroGame;
using UnityEngine;

namespace Assets
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        private static UpdateProvider _instance;
        public static UpdateProvider Instance => _instance;

        private readonly List<SubUpdateProvider> _subUpdateProviders = new List<SubUpdateProvider>();


        private readonly List<IUpdateable> _updateablesList = new List<IUpdateable>();
        private readonly List<IUpdateable> _removedUpdateables = new List<IUpdateable>();
        public void Add(IUpdateable updateable)
        {
            _updateablesList.Add(updateable);
        }

        public void Remove(IUpdateable updateable)
        {
            _removedUpdateables.Add(updateable);
        }

        public void SendUpdates()
        {
            foreach (var updateable in _updateablesList)
            {
                if (updateable.IsUpdateableActive)
                {
                    updateable.OnUpdate(this, Time.deltaTime);
                }
            }

            foreach (var removed in _removedUpdateables)
            {
                _updateablesList.Remove(removed);
            }

            if (_removedUpdateables.Count != 0)
            {
                Debug.Log(gameObject.name + $": {_updateablesList.Count} updateables left, {_subUpdateProviders.Count} sub providers left");
            }
            _removedUpdateables.Clear();
        }

        void Start()
        {
            _instance = this;
        }

        void Update()
        {
            SendUpdates();
        }

        private IEnumerator SubUpdateLoop(SubUpdateProvider provider, float timePeriod)
        {
            while (provider.IsRunning)
            {
                provider.SendUpdates();
                yield return new WaitForSeconds(timePeriod);
            }
        }

        public IUpdateProvider CreateSubProvider(float timePeriod)
        {
            var provider = new SubUpdateProvider(this);
            Coroutine coroutine = StartCoroutine(SubUpdateLoop(provider, timePeriod));
            provider.Coroutine = coroutine;
            _subUpdateProviders.Add(provider);
            return provider;
        }

        private void DeleteSubUpdateProvider(SubUpdateProvider provider)
        {
            StopCoroutine(provider.Coroutine);
            _subUpdateProviders.Remove(provider);
            Debug.Log(gameObject.name + $": {_subUpdateProviders.Count} sub providers left");
        }

        private class SubUpdateProvider : IUpdateProvider
        {
            private readonly List<IUpdateable> _subUpdateables = new List<IUpdateable>();
            private readonly List<IUpdateable> _subRemovedUpdateables = new List<IUpdateable>();
            private float _prevUpdate;
            private readonly UpdateProvider _parentUpdateProvider;
            private bool _isRunning = true;

            public bool IsRunning => _isRunning;

            public Coroutine Coroutine { get; set; }
        
            public SubUpdateProvider(UpdateProvider parentUpdateProvider)
            {
                _prevUpdate = Time.time;
                _parentUpdateProvider = parentUpdateProvider;
            }

            public void Add(IUpdateable updateable)
            {
                _subUpdateables.Add(updateable);
            }

            public void Remove(IUpdateable updateable)
            {
                _subRemovedUpdateables.Add(updateable);
            }

            public void SendUpdates()
            {
                foreach (var updateable in _subUpdateables)
                {
                    if (updateable.IsUpdateableActive)
                    {
                        updateable.OnUpdate(this, Time.time - _prevUpdate);
                    }
                }

                foreach (var removed in _subRemovedUpdateables)
                {
                    _subUpdateables.Remove(removed);
                    if (_subUpdateables.Count == 0)
                    {
                        _parentUpdateProvider.DeleteSubUpdateProvider(this);
                        _isRunning = false;
                    }
                }

                if (_subRemovedUpdateables.Count != 0)
                {

                    Debug.Log("subProvider" + $": {_subUpdateables.Count} sub-updateables left");

                }

                _subRemovedUpdateables.Clear();

                _prevUpdate = Time.time;
            }
        }
    }
}