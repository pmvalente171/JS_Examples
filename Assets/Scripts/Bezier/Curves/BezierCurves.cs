﻿using System.Collections.Generic;
using UnityEngine;

namespace Bezier.Curves
{
    public class BezierCurves : MonoBehaviour {

        public List<Vector3> points;

        public void Reset () {
            points = new List<Vector3> {
                new Vector3(1f, 0f, 0f),
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f),
                new Vector3(4f, 0f, 0f)
            };
        }

        public Vector3 this[int i] {
            get {
                return points[i];
            }
            set {
                points[i] = value;
            }
        }

        public Vector3 GetPoint(float t) {
            return transform.TransformPoint(BezierUtil.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetVelocity(float t) {
            return transform.TransformPoint(BezierUtil.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
                   transform.position;
        }

        public Vector3 GetDirection(float t) {
            return GetVelocity(t).normalized;
        }
    }
}
