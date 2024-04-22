using Bezier.Curves;
using UnityEditor;
using UnityEngine;

namespace Bezier.Editor
{
    [CustomEditor(typeof (BezierCurves))]
    public class BezierCurveInspector : UnityEditor.Editor{

        BezierCurves curve;
        Transform handleTransform;
        Quaternion handleRotation;

        const float directionScale = 0.5f;
        const int lineSteps = 10;

        void OnSceneGUI() {
            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);
            Vector3 p3 = ShowPoint(3);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        }

        void ShowDirections() {
            Handles.color = Color.green;
            Vector3 point = curve.GetPoint(0f);
            Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
            for (int i = 1; i <= lineSteps; i++) {
                point = curve.GetPoint(i / (float)lineSteps);
                Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
            }
        }

        Vector3 ShowPoint(int index) {
            Vector3 point = handleTransform.TransformPoint(curve[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve[index] = handleTransform.InverseTransformPoint(point);
            }
            return point;
        }

        void OnEnable() {
            curve = target as BezierCurves;
            handleTransform = curve.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;
        }
    }
}
