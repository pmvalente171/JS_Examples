using System;
using GameArchitecture.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameArchitecture
{
    public class App : MonoBehaviour
    {
        private static App _instance;
        
        private void Start()
        {
#if UNITY_EDITOR
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    // main menu
                    break;
                default:
                    // it's a gameplay scene
                    break;
            } 
#else
            // Load the default method
#endif
        }

        /// <summary>
        /// This method is called
        /// before the scene is loaded
        /// And it will load all main
        /// Components that are needed
        /// to run the game 
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            // Load the singleton instance
            _instance = SingletonUtil.LoadSingletonInstance<App>();

#if UNITY_EDITOR
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    // main menu
                break;
                default:
                    // it's a gameplay scene
                    break;
            } 
#else
            // Load the default manner
#endif
        }
    }
}