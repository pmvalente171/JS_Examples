using System.Collections.Generic;
using System.Linq;
using GameArchitecture.ScriptablePatterns;
using UnityEngine;
using GameArchitecture.Util;

namespace Game
{
    public class FishingManager : MonoBehaviour
    {
        [Header("References")]
        public ScriptableString scoreText;
        
        [Header("Values")]
        [Tooltip("Value in seconds")] public float fishingTime = 3f; // Time it takes to fish, in seconds
        [Tooltip("Value in seconds")] public float fishTimeWindow = 0.85f; // Time window to catch the fish, in seconds
        
        [Header("Events")] public GameEvent closeFishUI; // Event to trigger when the user interacts with the fish
        public GameEvent fishCaught; // Event to trigger when the user catches a fish
        
        private static FishingManager _instance;
        
        // Bootstrapping the PlayerFishing class and loading the singleton instance
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap() => _instance = SingletonUtil.
            LoadSingletonInstance<FishingManager>();

        private bool _isInFishingSlot;
        private bool _canFish;
        private float _fishingTimer; // Value between 0 and 1
        private float _fishTimerWindow; // Value between 0 and 1, in percentage
        
        private Coroutine _timerCoroutine;
        
        private List<Fish> _fishCaught = new ();

        public static bool CanFish
        {
            get => _instance._canFish;
            set => _instance._canFish = value;
        }
        
        // This the 
        public static float FishingTimer => _instance._fishingTimer;
        public static float FishingTimerWindow => _instance._fishTimerWindow;
        
        public static List<Fish> FishCaught => _instance._fishCaught;
        
        private void Awake()
        {
            _fishingTimer = 0f;
            _fishTimerWindow = fishTimeWindow / fishingTime;
        }
        
        private bool _fishEvent()
        {
            if (!_canFish)
                return false;
            
            if (_timerCoroutine is not null)
            {
                _fishingTimer = 0f;
                StopCoroutine(_timerCoroutine);
            }
            
            _timerCoroutine = StartCoroutine(AnimationUtil.LerpInTimeWindow(fishingTime, f =>
            {
                // Update the fishing timer
                _fishingTimer = f;
                _isInFishingSlot = Mathf.Abs(_fishingTimer - 0.5f) < _fishTimerWindow;
                
                if (f >= 1f)
                {
                    _fishingTimer = 0f;
                    _isInFishingSlot = false;
                    closeFishUI.Invoke();
                }
            }));
            return true;
        }
        
        private bool _catchFish(Fish fish)
        {
            if (_timerCoroutine is not null)
            {
                _fishingTimer = 0f;
                StopCoroutine(_timerCoroutine);
            }
            
            // Check if the player caught the fish
            if (!_canFish || !_isInFishingSlot)
                return false;
            
            // Add the fish to the list of fish caught
            _fishCaught.Add(fish);
            scoreText.Value = _fishCaught.Sum(f => f.value).ToString("C0");
            fishCaught.Invoke();
            return true;
        }
        
        public static bool FishEvent() => _instance._fishEvent();
        public static bool CatchFish(Fish fish) => _instance._catchFish(fish); 
    }
}