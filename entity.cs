using System;
using System.Collections.Generic;

namespace MyGame {
        
    public class Entity
    {
        public int Id { get; private set; }

        public Entity(int id)
        {
            Id = id;
        }
    }

    public class EntityManager
    {
        private int _nextId = 0;
        private Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();
        private Dictionary<Type, Dictionary<int, Component>> _componentsByType = new Dictionary<Type, Dictionary<int, Component>>();

        public Entity CreateEntity()
        {
            var entity = new Entity(_nextId++);
            _entities.Add(entity.Id, entity);
            return entity;
        }

        public void AddComponent<T>(Entity entity, T component) where T : Component
        {
            var type = typeof(T);
            if (!_componentsByType.ContainsKey(type))
            {
                _componentsByType[type] = new Dictionary<int, Component>();
            }
            _componentsByType[type][entity.Id] = component;
        }

        public T GetComponent<T>(Entity entity) where T : Component
        {
            var type = typeof(T);
            if (_componentsByType.ContainsKey(type) && _componentsByType[type].ContainsKey(entity.Id))
            {
                return (T)_componentsByType[type][entity.Id];
            }
            return null;
        }

        public IEnumerable<Entity> GetEntitiesWithComponent<T>() where T : Component
        {
            var type = typeof(T);
            if (_componentsByType.ContainsKey(type))
            {
                foreach (var entityId in _componentsByType[type].Keys)
                {
                    yield return _entities[entityId];
                }
            }
        }

        public IEnumerable<Entity> GetEntitiesWithComponents(params Type[] componentTypes)
        {
            foreach (var entity in _entities.Values)
            {
                bool hasAllComponents = true;
                foreach (var type in componentTypes)
                {
                    if (!_componentsByType.ContainsKey(type) || !_componentsByType[type].ContainsKey(entity.Id))
                    {
                        hasAllComponents = false;
                        break;
                    }
                }
                if (hasAllComponents)
                {
                    yield return entity;
                }
            }
        }
    }
}