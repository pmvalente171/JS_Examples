using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bezier.Spline
{
    public class BezierSpline : MonoBehaviour 
    {
        #region Vars
        #region Serialze Fields

        [HideInInspector] public float totalDistance = 10f;
        
        /// <summary>
        /// Constitution for 4 points:
        ///
        /// [0] - p1
        /// [1] - p2
        /// [2] - p3
        /// [3] - p4
        /// </summary>
        [SerializeField] List<Vector3> points;
        
        // Contains information 
        //  about the spline itself
        [SerializeField] List<BezierControlPointMode> modes;
        
        [SerializeField] bool isLoop;
        [SerializeField] bool isShowingDir = false;
        [SerializeField] private List<float> distances = new List<float>();
        #endregion
        
        #region Private Vars
        float coneDiameter, coneHeight;
        #endregion
        #endregion

        // Method used for
        //  resetting the 
        //  spline
        void Reset()
        {
            isShowingDir = false;
            Vector3 centre = transform.position;
            // Sets up the initial Anchor points
            points = new List<Vector3>
            {
                centre+Vector3.left,
                centre+(Vector3.left+Vector3.up)*.5f,
                centre + (Vector3.right+Vector3.down)*.5f,
                centre + Vector3.right
            };

            modes = new List<BezierControlPointMode>
            {
                BezierControlPointMode.Aligned,
                BezierControlPointMode.Aligned
            };

            distances = new List<float>()
            {
                1f 
            };
        }

        public void ResetSpline() => Reset();

        void enforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Count - 1))
            {
                return;
            }

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = points.Count - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Count)
                {
                    enforcedIndex = 1;
                }
            } 
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Count)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = points.Count - 2;
                }
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
            }
            points[enforcedIndex] = middle + enforcedTangent;
        }

        public int nbOfCurves => (points.Count - 1) / 3;

        public int nbOfPoints => points.Count;

        public float ConeDiameter 
        {
            set => coneDiameter = value;
            get => coneDiameter;
        }

        public float ConeHeight 
        {
            set => coneHeight = value;
            get => coneHeight;
        }

        public bool showDirs 
        {
            get => isShowingDir;
            set => isShowingDir = value;
        }

        public Vector3 this[int i] 
        {
            get => points[i];
            set 
            {
                if (i % 3 == 0)
                {
                    Vector3 delta = value - points[i];
                    if (loop)
                    {
                        if (i == 0)
                        {
                            points[1] += delta;
                            points[points.Count - 2] += delta;
                            points[points.Count - 1] = value;
                        } else if (i == points.Count - 1)
                        {
                            points[0] = value;
                            points[1] += delta;
                            points[i - 1] += delta;
                        } else
                        {
                            points[i - 1] += delta;
                            points[i + 1] += delta;
                        }
                    } else
                    {
                        if (i > 0)
                        {
                            points[i - 1] += delta;
                        }
                        if (i + 1 < points.Count)
                        {
                            points[i + 1] += delta;
                        }
                    }
                }
                points[i] = value;
                enforceMode(i);
            }
        }

        public void removeTail()
        {
            points.RemoveAt(0);
            points.RemoveAt(0);
            points.RemoveAt(0);
            modes.RemoveAt(0);
        }
    
        public bool loop 
        {
            get => isLoop;
            set 
            {
                isLoop = value;
                if (value == true)
                {
                    modes[modes.Count - 1] = modes[0];
                    this[0] = points[0];
                }
            }
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return modes[(index - 1) / 3];
        }
    
        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;
            if (loop)
            {
                if (modeIndex == 0)
                {
                    modes[modes.Count - 1] = mode;
                } else if (modeIndex == modes.Count - 1)
                {
                    modes[0] = mode;
                }
            }
            enforceMode(index);
        }   

        public Vector3[] GetPointsFromCurve(int curveIndex)
        {
            return new Vector3[] 
            {
                points[curveIndex * 3],
                points[curveIndex * 3 + 1],
                points[curveIndex * 3 + 2],
                points[curveIndex * 3 + 3]
            };
        }

        private void AddCurveDefault(Vector3 anchorPos)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((points[points.Count - 1] + anchorPos) / 2f);
            points.Add(anchorPos);
            modes.Add(modes[modes.Count - 1]);
            enforceMode(points.Count - 4);
            if (loop)
            {
                points[points.Count - 1] = points[0];
                modes[modes.Count - 1] = modes[0];
                enforceMode(0);
            }
        }

        private void AddCurveSmooth(Vector3 anchorPos, float lerpTime=0.4f)
        {
            Vector3 v1 = (points[points.Count - 1] - points[points.Count - 4]);
            Vector3 v2 = (anchorPos - points[points.Count - 1]);
            float distance = v2.magnitude, halfDistance = distance / 3f, v1Distance = v1.magnitude;
            v1.Normalize(); v2.Normalize();

            points[points.Count - 2] = points[points.Count - 1] - v2 * v1Distance / 3f;
            points.Add(points[points.Count - 1] + v2 * halfDistance); // Second anchor point to previous point
            points.Add(anchorPos - v2 * halfDistance); // Anchor point to the new point
            points.Add(anchorPos);
            
            distances.Add(distance);
            modes.Add(modes[modes.Count - 1]);
            enforceMode(points.Count - 4);
            
            if (loop)
            {
                points[points.Count - 1] = points[0];
                modes[modes.Count - 1] = modes[0];
                enforceMode(0);
            }
        }
        
        /**
         * This method adds a new point to 
         * the spline
         */
        public void AddCurve(Vector3 anchorPos, AnchorType type=AnchorType.DEFAULT)
        {
            switch (type)
            {
                case AnchorType.DEFAULT:
                    AddCurveDefault(anchorPos);
                    break;
                case AnchorType.SHORT_ANCHORED:
                    AddCurveSmooth(anchorPos);
                    break;
            }
        }

    /**
     * This Method adds a new random 
     * point to the Spline (course) 
     * in a cone in front of the last point
     */
        public void addRandomPointToSpline()
        {

            Vector3 splineHead = this[nbOfPoints - 1];
            float k = splineHead.x, h = splineHead.y;

            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);

            if (randY <= 0)
                randY = Mathf.Sign(randX);
            else
                randY = -Mathf.Sign(randX);

            randX = coneDiameter * randX + k;
            randY = coneDiameter * randY + h;
        
            AddCurve(new Vector3(randX, randY, splineHead.z + coneHeight));
        }
    
    
        /**
         * This method return a point 
         * from the spline based on its percentage
         */
        public Vector3 GetPoint(float t)
        {
            int i, curveNumber;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Count - 4;
                curveNumber = nbOfCurves - 1;
            } 
            else
            {
                t = Mathf.Clamp01(t) * nbOfCurves;
                curveNumber = (int)t;
                i = curveNumber;
                
                t -= i;
                i *= 3;
            }
            
            // print($"Distances : {distances.Count}; Modes : {modes.Count}; nb of curves : {nbOfCurves};");
            // float distance = distances[curveNumber];
            // float percentage = Mathf.Clamp01(distance / curveNumber);
            // print(percentage);
            // t = Mathf.Pow(t, percentage);
            
            return transform.TransformPoint(BezierUtil.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }
        
        public Vector3 GetPoint(float t, out float distance)
        {
            int i, curveNumber;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Count - 4;
                curveNumber = nbOfCurves - 1;
            } 
            else
            {
                t = Mathf.Clamp01(t) * nbOfCurves;
                curveNumber = (int)t;
                i = curveNumber;
                
                t -= i;
                i *= 3;
            }
            distance = distances[curveNumber];
            
            // print($"Distances : {distances.Count}; Modes : {modes.Count}; nb of curves : {nbOfCurves};");
            // float distance = distances[curveNumber];
            // float percentage = Mathf.Clamp01(distance / curveNumber);
            // print(percentage);
            // t = Mathf.Pow(t, percentage);
            
            return transform.TransformPoint(BezierUtil.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }

        /**
         * This method returns the 
         * position from a curve with 
         * t progress
         */
        public Vector3 GetPointFromCurve(int nbOfCurve, float t)
        {
            Vector3[] p = this.GetPointsFromCurve(nbOfCurve);

            if (t >= 1f)
            {
                t = 1f;
            } else
            {
                t = Mathf.Clamp01(t);
            }

            return transform.TransformPoint(BezierUtil.GetPoint(
                p[0], p[1], p[2], p[3], t));
        }

        public void SetDistance(int index, float value) => distances[index] = value;


    /**
     * This method return the curve velocity 
     * from the spline based on its precentage
     */
        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Count - 4;
            } else
            {
                t = Mathf.Clamp01(t) * nbOfCurves;
                i = (int)t;
                t -= i;
                i *= 3;
            }

            Transform trans;
            return (trans = transform).TransformPoint(BezierUtil.GetFirstDerivative(
                points[i], points[i + 1], points[i + 2], points[i + 3], t)) - trans.position;
        }

        /**
     * This method returns the 
     * velocity from a curve with 
     * t progress
     */
        public Vector3 GetVelocityFromCurve(int nbOfCurve, float t)
        {
            if(nbOfCurve == 0 && t == 0f)
            {
                return new Vector3(0f, 0f, 0f);
            }

            Vector3[] p = this.GetPointsFromCurve(nbOfCurve);

            if (t >= 1f)
            {
                t = 1f;
            } else
            {
                t = Mathf.Clamp01(t);
            }

            Transform trans;
            return (trans = transform).TransformPoint(BezierUtil.GetFirstDerivative(
                p[0], p[1], p[2], p[3], t)) - trans.position;
        }

        /**
     * This method based on the curve percentage 
     * returns the curves direction
     */
        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }
        /**
     * This method returns the 
     * direction from a curve with 
     * t progress
     */
        public Vector3 GetDirectionFromCurve(int nbOfCurve, float t)
        {    
            return GetVelocityFromCurve(nbOfCurve, t).normalized;
        }
    }
}
