using UnityEngine;

namespace GameArchitecture
{
    public class SecondOrderDynamics
    {
        private Vector3 xp; // previous input
        private Vector3 y, yd; // state and derivative
        private float k1, k2, k3; // dynamics constants

        public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
        {
            k1 = z / (Mathf.PI * f);
            var temp = 2 * Mathf.PI * f;
            k2 = 1 / (temp* temp);
            k3 = r * z / (2 * Mathf.PI * f);

            xp = x0;
            y = x0;
            yd = new();
        }
        
        /// <summary>
        /// Update the state of the second order dynamics.
        /// </summary>
        /// <param name="dt">time between frames</param>
        /// <param name="x">target value</param>
        /// <returns>the new value</returns>
        public Vector3 Update(float dt, Vector3 x)
        {
            var xd = (x - xp) / dt;
            xp = x;
            
            y += yd * dt;
            yd += dt * (x + k3 * xd - y - k1 * yd) / k2;
            // Debug.Log(x + " " + y + " " + yd);
            return y;
        }
    }
}