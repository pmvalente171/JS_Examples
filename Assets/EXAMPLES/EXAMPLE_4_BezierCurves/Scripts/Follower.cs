using System;
using Bezier.Spline;
using UnityEngine;
using UnityEngine.Serialization;

namespace EXAMPLES.EXAMPLE_4_BezierCurves.Scripts
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Follower : MonoBehaviour
    {
        [Header("References")] public BezierSpline spline;
        public float TimeToLoop = 2500f;
        
        private float _t;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _t = 0f;
            
            Vector3 position = spline.GetPoint(_t);
            Vector3 direction = spline.GetDirection(_t);
            
            _rigidbody.MovePosition(position);
            _rigidbody.MoveRotation(Quaternion.LookRotation(direction));
        }

        private void FixedUpdate()
        {
            _t += 1 / TimeToLoop;
            if (_t > 1f)
                _t = 0f;
            
            Vector3 position = spline.GetPoint(_t);
            Vector3 direction = spline.GetDirection(_t);
            
            _rigidbody.MovePosition(position);
            _rigidbody.MoveRotation(Quaternion.LookRotation(direction));
        }
    }
}
