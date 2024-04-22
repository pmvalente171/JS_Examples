using System.Collections.Generic;

namespace GameArchitecture.Store
{
    public class RuntimeStore<T> : IRuntimeStore
    {
        private Dictionary<int, T> _table = new Dictionary<int, T>();

        public void InitiateStore() => _table = new Dictionary<int, T>();

        public void Destroy() => _table.Clear(); 

        public void GetTags()
        {
        }

        public bool GETElement(int objectID, out T data) => _table.TryGetValue(objectID, out data);

        public void SetElement(int objectID, T data) => _table.Add(objectID, data);

        public void RemoveElement(int objectID) => _table.Remove(objectID);
    }
}