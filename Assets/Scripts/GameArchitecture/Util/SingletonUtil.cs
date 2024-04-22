using UnityEngine;

namespace GameArchitecture.Util
{
    public static class SingletonUtil
    {
        // Make sure the object is not destroyed when loading a new scene
        public static T LoadSingletonInstance<T>() where T : MonoBehaviour
        {
            T instance = null;
            // Find the instance of the object in the scene
            T[] instancedObjects = Object.FindObjectsOfType<T>();
            if (instancedObjects is not null && instancedObjects.Length > 0)
            {
                if (instancedObjects.Length > 1)
                    for (int i = instancedObjects.Length - 1; i > 0; i--)
                    {
                        Object.Destroy(instancedObjects[i].gameObject);
                    }
                
                instance = instancedObjects[0];
                Object.DontDestroyOnLoad(instance);
                return instance;
            }
            
            // Load the prefab from the resources folder
            T prefab = Resources.Load<T>(typeof(T).Name);
            if (prefab is null)
            {
                prefab = new GameObject(typeof(T).Name).AddComponent<T>();
                instance = prefab;
            }
            else instance = Object.Instantiate(prefab);
            
            // Make sure the object is not destroyed when loading a new scene
            Object.DontDestroyOnLoad(instance);
            return instance;
        }
        
        // Make sure the object is not destroyed when loading a new scene
        public static T LoadSingletonInstance<T>(string name) where T : MonoBehaviour
        {
            T instance;
            
            // Find the instance of the object in the scene
            T[] instancedObjects = Object.FindObjectsOfType<T>();
            if (instancedObjects is not null && instancedObjects.Length > 0)
            {
                if (instancedObjects.Length > 1)
                    for (int i = instancedObjects.Length - 1; i > 0; i--)
                    {
                        Object.Destroy(instancedObjects[i].gameObject);
                    }
                
                instance = instancedObjects[0];
                Object.DontDestroyOnLoad(instance);
                return instance;
            }
            
            // Load the prefab from the resources folder
            T prefab = Resources.Load<T>(name);
            if (prefab is null)
            {
                prefab = new GameObject(name).AddComponent<T>();
                instance = prefab;
            }
            else instance = Object.Instantiate(prefab);
            
            // Make sure the object is not destroyed when loading a new scene
            Object.DontDestroyOnLoad(instance);
            return instance;
        }
    }
}