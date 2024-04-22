using System;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine.AI;

namespace GameArchitecture.Util
{
    public static class MathUtil
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)] public float f;

            [FieldOffset(0)] public int tmp;
        }

        public static float Sqrt(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            u.f = z;
            u.tmp -= 1 << 23;
            u.tmp >>= 1;
            u.tmp += 1 << 29;
            return u.f;
        }

        public static float InvSqrt(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            float xhalf = 0.5f * z;
            u.f = z;
            u.tmp = 0x5f3759df - (u.tmp >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f;
        }

        public static Vector3 Normalize(Vector3 v3)
        {
            float x = v3.x, y = v3.y, z = v3.z;
            float temp = x * x + y * y + z * z;

            if (temp == 0) return new Vector3();

            FloatIntUnion u;
            u.tmp = 0;
            u.f = z;
            u.tmp -= 1 << 23;
            u.tmp >>= 1;
            u.tmp += 1 << 29;

            temp = 1 / u.f;
            return new Vector3(x * temp, y * temp, z * temp);
        }

        public static float Bounce(float value)
        {
            value -= 0.5f;
            return -4 * (value * value) + 1;
        }

        public static float Inverse(float value) => 1 - value;

        public static Vector3 Inverse(Vector3 value) => Vector3.one - value;

        public static Vector2 Inverse(Vector2 value) => Vector2.one - value;

        public static float ABS(float value) => value < 0 ? -value : value;

        public static Vector3 ABS(Vector3 value) => new Vector3(ABS(value.x), ABS(value.y), ABS(value.z));

        public static Vector2 ABS(Vector2 value) => new Vector2(ABS(value.x), ABS(value.y));

        public static float Lerp(float a, float b, float f) => f * a + (1 - f) * b;

        public static Vector3 Lerp(Vector3 a, Vector3 b, float f) => f * a + (1 - f) * b;

        public static Vector2 Lerp(Vector2 a, Vector2 b, float f) => f * a + (1 - f) * b;

        public static float SqrtLerp(float a, float b, float f)
        {
            f = Sqrt(f);
            return f * a + (1 - f) * b;
        }

        public static Vector3 SqrtLerp(Vector3 a, Vector3 b, float f)
        {
            f = Sqrt(f);
            return f * a + (1 - f) * b;
        }

        public static Vector2 SqrtLerp(Vector2 a, Vector2 b, float f)
        {
            f = Sqrt(f);
            return f * a + (1 - f) * b;
        }

        public static float BounceLerp(float a, float b, float f)
        {
            f = Bounce(f);
            return f * a + (1 - f) * b;
        }

        public static Vector3 BounceLerp(Vector3 a, Vector3 b, float f)
        {
            f = Bounce(f);
            return f * a + (1 - f) * b;
        }

        public static Vector2 BounceLerp(Vector2 a, Vector2 b, float f)
        {
            f = Bounce(f);
            return f * a + (1 - f) * b;
        }

        public static float CurveLerp(AnimationCurve curve, float a, float b, float f)
        {
            f = curve.Evaluate(f);
            return f * a + (1 - f) * b;
        }

        public static Vector3 CurveLerp(AnimationCurve curve, Vector3 a, Vector3 b, float f)
        {
            f = curve.Evaluate(f);
            return f * a + (1 - f) * b;
        }

        public static Vector2 CurveLerp(AnimationCurve curve, Vector2 a, Vector2 b, float f)
        {
            f = curve.Evaluate(f);
            return f * a + (1 - f) * b;
        }

        public static Vector3 Friction(float friction,
            Vector3 prevVelocity, ref Vector3 velocity,
            float stopSpeed = Single.MaxValue)
        {
            float speed = prevVelocity.magnitude;
            if (speed != 0)
            {
                float control = speed < stopSpeed ? stopSpeed : speed;
                float drop = control * friction * Time.fixedDeltaTime;
                float tempY = prevVelocity.y;

                prevVelocity *= Mathf.Max(speed - drop, 0) / speed;
                prevVelocity.y = tempY;
                velocity = prevVelocity;
            }

            return prevVelocity;
        }

        public static Vector3 Accelerate(Vector3 prevVelocity,
            float maxSpeed, float acceleration,
            Vector3 wishDirection)
        {
            float currentSpeed, addSpeed, accelSpeed;

            currentSpeed = Vector3.Dot(prevVelocity, wishDirection);
            addSpeed = maxSpeed - currentSpeed;

            if (addSpeed <= 0)
                return new Vector3();

            accelSpeed = maxSpeed * Time.fixedDeltaTime * acceleration;
            if (accelSpeed > addSpeed)
                accelSpeed = addSpeed;

            return accelSpeed * wishDirection;
        }
        
        public static float getDistanceFromPath(NavMeshPath path)
        {
            Vector3 [] corners = path.corners;
            float totalDistance = 0f;
            if (corners.Length == 0) return 0f;

            for (int i = 1; i < corners.Length; i++)
            {
                totalDistance += Vector3.Distance(corners[i - 1], corners[i]);
            }
            
            return totalDistance;
        }

    }
}