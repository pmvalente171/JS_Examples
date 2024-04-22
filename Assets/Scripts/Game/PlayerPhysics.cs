using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Rigidbody), 
        typeof(Collider),
        typeof(PlayerInput))]
    public class PlayerPhysics : MonoBehaviour
    {
        [Header("References")]
        public new Camera camera;
        public Transform boat;
    
        [Header("Values")]
        public float acceleration = 5;
        public float maxSpeed = 5;
        public float friction = 0.5f;
        public float stopSpeed = 0.1f;
    
        [Space] public float turnSpeed = 5;
    
    
        [Space(10)] public Vector3 movementDir;

        private Rigidbody _rigidbody;
        private Collider _collider;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            var playerInput = GetComponent<PlayerInput>();
            playerInput.OnMoveAction += ReceiveInput;
        }
    
        public void ReceiveInput(Vector2 dir)
        {
            // read the input
            movementDir = dir;
            movementDir.z = movementDir.y;
        
            // convert the input to world space
            movementDir = camera.transform.TransformDirection(movementDir);
            movementDir.y = 0;
            movementDir.Normalize();
        }

        private void Update()
        {
            // if the velocity is too low, don't rotate the boat
            if (_rigidbody.velocity.sqrMagnitude < 0.1f)
                return;
        
            var dir = _rigidbody.velocity.normalized;
        
            // rotate dir 90 degrees
            dir = Quaternion.Euler(0, 90, 0) * dir;
        
            boat.rotation = Quaternion.Slerp(boat.rotation, 
                Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }

        private void Move()
        {
            // use the input to calculate the target velocity
            Vector3 velocity = _rigidbody.velocity; 
        
            // clamp the velocity according to the friction
            _rigidbody.velocity = GameArchitecture.Util.MathUtil.Friction(
                friction, velocity, ref velocity, stopSpeed);
        
            // calculate the acceleration
            Vector3 acc = GameArchitecture.Util.MathUtil.Accelerate(
                velocity, maxSpeed, acceleration, movementDir);
        
            // move the rigidbody
            _rigidbody.AddForce(acc, ForceMode.Acceleration);
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
