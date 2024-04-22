using System;
using EXAMPLES.EXAMPLE_1_GUI.Scripts;
using GameArchitecture.ScriptablePatterns;
using UnityEngine;

namespace Game
{
    public class FishingSpot : MonoBehaviour
    {
        [Header("References")]
        public FishSO fish;

        [SerializeField] private GUIMarker markerPrefab;
        
        [Header("Events")]
        public GameEvent onEnter;
        public GameEvent onExit;
        
        private GUIMarker _marker;
        private PlayerFishing _playerFishing;
        private int _playerID;
        
        private bool _isFishing;

        private void Awake()
        {
            // Cache the PlayerMovement component at the start
            _playerFishing = FindObjectOfType<PlayerFishing>();
            if (_playerFishing == null)
                throw new Exception("PlayerMovement not found in the scene");
            
            _playerID = _playerFishing.gameObject.GetInstanceID();
            _isFishing = false;
            
            // Instantiate the marker
            _marker = Instantiate(markerPrefab, FindObjectOfType<Canvas>().transform);
            _marker.target = transform;
            _marker.player = _playerFishing.transform;
            _marker.UpdatePosition();
        }
        
        // This method triggers the event
        private void TriggerEvent(Collider other, GameEvent gameEvent)
        {
            // Check if the object that entered the trigger is the player
            if (other.gameObject.GetInstanceID() != _playerID)
                return;

            // assign the fish to the player
            _playerFishing.Fish = fish;
            
            // Raise the event
            gameEvent.Invoke();
            FishingManager.CanFish = _isFishing;
            
            // Fade out the marker
            _marker.enabled = !_isFishing;
            if (_isFishing) _marker.fade.FadeOut();
            else _marker.fade.FadeIn();
        }
    
        // This method is called when another collider enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            _isFishing = true;
            TriggerEvent(other, onEnter);
        }

        // This method is called when another collider exits the trigger
        private void OnTriggerExit(Collider other)
        {
            _isFishing = false;
            TriggerEvent(other, onExit);
        }
    }
}