using GameArchitecture.ScriptablePatterns;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerFishing : MonoBehaviour
    {
        [Header("Events")]
        public GameEvent openFishingUI;
        public GameEvent closeFishingUI;
        
        private FishSO _fish;
        
        public FishSO Fish
        {
            get => _fish;
            set => _fish = value;
        }
        
        private void Awake()
        {
            var playerInput = GetComponent<PlayerInput>();
            playerInput.OnInteract += TryToFish;
        }
        
        public void TryToFish()
        {
            print("Fishing!");
            if (!FishingManager.CanFish)
                return;
            
            if (FishingManager.FishingTimer > 0f)
            {
                bool caughtFish = FishingManager.CatchFish(_fish.fish);
                print(caughtFish ? "Caught fish!" : "Failed to catch fish!");
                
                // Close the fishing UI
                closeFishingUI.Invoke();
                return;
            }
            
            // Open the fishing UI
            openFishingUI.Invoke();
            FishingManager.FishEvent();
        }
    }
}