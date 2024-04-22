using UnityEngine;

namespace EXAMPLES.EXAMPLE_1_GUI.Scripts
{
    [RequireComponent(typeof(GUIFade))]
    public class GUIMarker : MonoBehaviour
    {
        [Header("Values")]
        public float maxDistance = 100f;
        
        [HideInInspector] public Transform target;
        [HideInInspector] public Transform player;
        [HideInInspector] public GUIFade fade;
        
        private RectTransform _rectTransform;
        private Camera _camera;
        
        private void Awake()
        {
            fade = GetComponent<GUIFade>();
            _rectTransform = GetComponent<RectTransform>();
        }
        private void Update() =>
            UpdatePosition();

        public void UpdatePosition()
        {
            if (target is null) return;
            if (player is null) return;
            
            _camera = _camera ? _camera : Camera.main;  // Camera.main is expensive, but you can't call it in Awake
                                                          // because the camera might not be initialized yet,
                                                          // meaning this is the next best thing
            Vector3 targetPos = target.position;
            Vector3 playerPos = player.position;
            
            // Get the distance between the player and the target
            float distance = Vector3.Distance(playerPos, targetPos);
            
            Vector3 screenPos = _camera.WorldToScreenPoint(targetPos);
            screenPos.z = 0;
            
            // clamp the position to the screen
            screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
            
            // If the target is too far away, fade out the marker
            if (distance > maxDistance)
            {
                print("Help");
                fade.FadeOut();
                return;
            }
            
            // If the target is within the distance, fade in the marker
            fade.FadeIn();
            
            // Dampen the position of the marker
            _rectTransform.position = Vector3.Lerp(_rectTransform.position, screenPos, Time.deltaTime * 20);
        }
    }
}