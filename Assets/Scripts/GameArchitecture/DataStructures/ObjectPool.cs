using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameArchitecture.DataStructures
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool;
        
        public ObjectPool(int size, T objectPrefab)
        {
            _pool = new Queue<T>();
            for (int i = 0; i < size; i++)
            {
                T instance = Object.Instantiate(objectPrefab);
                instance.gameObject.SetActive(false);
                _pool.Enqueue(instance);
            }
        }
        
        public ObjectPool(int size, T objectPrefab, Transform parent)
        {
            _pool = new Queue<T>();
            for (int i = 0; i < size; i++)
            {
                T instance = Object.Instantiate(objectPrefab, parent);
                instance.gameObject.SetActive(false);
                _pool.Enqueue(instance);
            }
        }

        public T GetObject(bool isActive=false)
        {
            T instance = _pool.Dequeue();
            instance.gameObject.SetActive(isActive);
            _pool.Enqueue(instance);
            return instance;
        }
        
        public List<T> GetObject(int size, bool isActive=false)
        {
            if (size > _pool.Count) return null;
            List<T> instances = new List<T>();
            for (int i = 0; i < size; i++)
            {
                T instance = _pool.Dequeue();
                instance.gameObject.SetActive(isActive);
                _pool.Enqueue(instance);
                instances.Add(instance);
            }
            return instances;
        }
    }
}