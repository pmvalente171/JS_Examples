using System;

namespace GameArchitecture.Util
{
    public static class ArrayUtil
    {
        public static void AddItemToArray<T>(ref T[] array, T item)
        {
            var newArray = new T[array.Length + 1];
            for (var i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            newArray[array.Length] = item;
            array = newArray;
        }
        
        /// <summary>
        /// This function removes an item from an array
        /// </summary>
        /// <param name="array">the array</param>
        /// <param name="item">the element being removed</param>
        /// <typeparam name="T">the type of the array</typeparam>
        public static void RemoveItemFromArray<T>(ref T[] array, T item)
        {
            // Step 1: Find the index of the item in the array
            int index = Array.IndexOf(array, item);

            // Step 2: If the item is found, create a new array without the element at that index
            if (index != -1)
            {
                T[] newArray = new T[array.Length - 1];
                for (int i = 0, j = 0; i < array.Length; i++)
                {
                    if (i != index)
                    {
                        newArray[j++] = array[i];
                    }
                }

                // Step 3: Update the reference to the original array with the new array
                array = newArray;
            }
        }

    }
}