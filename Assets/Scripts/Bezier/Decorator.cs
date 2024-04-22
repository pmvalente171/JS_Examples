using System.Collections;
using Bezier.Spline;
using UnityEngine;

namespace Bezier
{
    public class Decorator : MonoBehaviour
    {
        public BezierSpline spline;

        public int frequency;

        public bool lookForward;

        public Transform[] items;

        private IEnumerator Start () {
            
            if (frequency <= 0 || items == null || items.Length == 0) 
            {
                yield break;
            }

            yield return new WaitForSeconds(10f);
            
            float stepSize = 1f / (frequency * items.Length);
            for (int p = 0, f = 0; f < frequency; f++) 
            {
                for (int i = 0; i < items.Length; i++, p++) 
                {
                    Transform item = Instantiate(items[i], transform, true) as Transform;
                    Vector3 position = spline.GetPoint(p * stepSize);
                    item.transform.localPosition = position;
                    
                    if (lookForward)
                    {
                        item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                    }
                }
            }
        }
    }
}