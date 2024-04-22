using System;
using System.Collections.Generic;
using GameArchitecture.Store;

namespace GameArchitecture
{
    /// <summary>
    /// This class is used
    /// to manage runtime
    /// stores
    /// </summary>
    public class StoreManager : GenericSingletonClass<StoreManager>
    {
        /// <summary>
        /// Table of active stores
        /// private Dictionary
        /// </summary>
        private readonly Dictionary<int, IRuntimeStore> _tables = new Dictionary<int, IRuntimeStore>();

        /// <summary>
        /// Tables that
        /// will be created
        /// at run time
        /// </summary>
        private List<Action> _createOnSceneChange = new List<Action>();

        /// <summary>
        /// Table that
        /// need to be
        /// destroyed
        /// on scene
        /// change
        /// </summary>
        private List<int> _destroyOnSceneChange = new List<int>();

        /// <summary>
        /// create a new store
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <typeparam name="T">The object type</typeparam>
        public void CreateStore<T>(int tableID)
        {
            if (_tables.ContainsKey(tableID))
                return;

            _tables.Add(tableID, new RuntimeStore<T>());
        }

        /// <summary>
        /// Add a new element
        /// to a table
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <param name="objectID">The object id</param>
        /// <param name="data">The data</param>
        /// <typeparam name="T">The object type</typeparam>
        public void AddElementToTable<T>( int tableID, int objectID, T data)
        {
            if (!_tables.TryGetValue(tableID, out var table))
            {
                table = new RuntimeStore<T>();
                _tables.Add(tableID, table);
            }

            ((RuntimeStore<T>) table).SetElement(objectID, data);
            _tables[tableID] = table;
        }

        /// <summary>
        /// Adds a set of
        /// new elements
        /// to the table
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <param name="objectID">The object id</param>
        /// <param name="data">The data</param>
        /// <typeparam name="T">The object type</typeparam>
        public void AddElementToTable<T>(int tableID, int objectID, T[] data)
        {
            if (!_tables.TryGetValue(tableID, out var table))
                table = new RuntimeStore<T>();

            for (int i = 0; i < data.Length; i++)
            {
                ((RuntimeStore<T>) table).SetElement(objectID, data[i]);
            }

            _tables[tableID] = table;
        }

        /// <summary>
        /// access the data of
        /// a single element of
        /// a store 
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <param name="objectID">The object id</param>
        /// <param name="data">the retrieved data</param>
        /// <typeparam name="T">The data type</typeparam>
        /// <returns>True if the element was
        /// successfully obtained false otherwise</returns>
        public bool GETData<T>(int tableID, int objectID, out T data)
        {
            if (!_tables.TryGetValue(tableID, out var table))
            {
                data = default;
                return false;
            }

            return ((RuntimeStore<T>) table).GETElement(objectID, out data);
        }

        /// <summary>
        /// Remove an element
        /// from a store
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <param name="objectID">The object id</param>
        /// <typeparam name="T">The data type</typeparam>
        public void RemoveElement<T>(int tableID, int objectID)
        {
            if (!_tables.TryGetValue(tableID, out var table))
                return;

            ((RuntimeStore<T>) table).RemoveElement(objectID);
        }

        /// <summary>
        /// Add a tag to a
        /// store, possible tags:
        /// - Destroy on scene transition
        /// - Create on scene transition
        /// - Don't destroy on scene transition
        /// </summary>
        /// <param name="tableID">The table id</param>
        /// <param name="storeTag">The object id</param>
        public void AddTag<T>(int tableID, StoreTag storeTag)
        {
            if (!_tables.TryGetValue(tableID, out var table))
                return;

            switch (storeTag)
            {
                case StoreTag.CreateOnSceneTransition:
                    _createOnSceneChange.Add(() => CreateStore<T>(tableID));
                    table.Destroy();
                    break;
                case StoreTag.DestroyOnSceneTransition:
                    _destroyOnSceneChange.Add(tableID);
                    break;
                case StoreTag.DontDestroyOnSceneTransition:
                    break;
            }
        }

        /// <summary>
        /// This method it's
        /// called on every
        /// scene transition
        /// </summary>
        public void OnSceneTransition()
        {
            for (int i = 0; i < _destroyOnSceneChange.Count; i++)
            {
                if (!_tables.TryGetValue(_destroyOnSceneChange[i], out var table)) continue;
                table.Destroy();
                _tables.Remove(_destroyOnSceneChange[i]);
            }
            foreach (var action in _createOnSceneChange) action();
        }
    }
}