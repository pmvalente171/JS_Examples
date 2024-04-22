using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        public event Action OnInteract;
        public event Action<Vector2> OnMoveAction;
        
        public void MovementInput(InputAction.CallbackContext context) => 
            Move(context.ReadValue<Vector2>());
        public void Move(Vector2 direction) => OnMoveAction?.Invoke(direction);

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnInteract?.Invoke();
        }
    }
}