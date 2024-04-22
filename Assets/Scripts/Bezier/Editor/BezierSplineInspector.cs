using Bezier.Spline;
using UnityEditor;
using UnityEngine;

namespace Bezier.Editor
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : UnityEditor.Editor {

        const float directionScale = 0.5f;
        const int stepsPerCurve = 10;
        const float handleSize = 0.04f;
        const float pickSize = 0.06f;

        int selectedIndex = -1; 
        BezierSpline spline;
        Transform handleTransform;
        Quaternion handleRotation;

        static Color[] modeColors = {
            Color.red,
            Color.yellow,
            new Vector4(0.7f, 0.7f, 0.7f, 1)
        };

        void OnEnable() {
            spline = target as BezierSpline;
            handleTransform = spline.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;
        }

        void OnSceneGUI() {
            Vector3 p0 = showPoint(0);
            for (int i = 1; i < spline.nbOfPoints; i += 3) {
                Vector3[] p = showPoints(i);

                Handles.color = Color.black;
                Handles.DrawLine(p0, p[0]);
                Handles.DrawLine(p[1], p[2]);

                Handles.DrawBezier(p0, p[2], p[0], p[1], Color.gray, null, 3f);
                p0 = p[2];
            }
            if (spline.showDirs)
                showDirections();
        }

        void DrawSelectedPointInspector() {
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Selected Point");
            Vector3 point = EditorGUILayout.Vector3Field("Position", spline[selectedIndex]);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline[selectedIndex] = point;
            }

            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)
                EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Change Point Mode");
                spline.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(spline);
            }
        }

        public override void OnInspectorGUI() {
            spline = target as BezierSpline;

            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", spline.loop);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Toggle Loop");
                EditorUtility.SetDirty(spline);
                spline.loop = loop;
            }

            //EditorGUI.BeginChangeCheck();
            //bool showDir = EditorGUILayout.BeginToggleGroup("Show directions", spline.showDirs);
            //if (EditorGUI.EndChangeCheck()) {
            //    Undo.RecordObject(spline, "Toggle direction");
            //    EditorUtility.SetDirty(spline);
            //    spline.showDirs = showDir;
            //}

            if (selectedIndex >= 0 && selectedIndex < spline.nbOfPoints) {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button("Add Curve")) {
                Undo.RecordObject(spline, "Add Curve");
                //spline.AddCurve();
                spline.AddCurve(new Vector3 (spline[spline.nbOfPoints - 1].x,
                    spline[spline.nbOfPoints - 1].y, spline[spline.nbOfPoints - 1].z + 1));
                EditorUtility.SetDirty(spline);
            }
        }

        void showDirections() {
            Handles.color = Color.green;
            Vector3 point = spline.GetPoint(0f);
            Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * spline.nbOfCurves;
            for (int i = 1; i <= steps; i++) {
                point = spline.GetPoint(i / (float)steps);
                Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
            }
        }


        Vector3 showPoint(int index) {
            Vector3 point = handleTransform.TransformPoint(spline[index]);
            float size = 1.5f;//HandleUtility.GetHandleSize(point);
            if (index == 0) {
                size *= 2f;
            }
            Handles.color = modeColors[(int)spline.GetControlPointMode(index)]; ;
            if (Handles.Button(point, handleRotation, handleSize * size, pickSize * size, Handles.SphereHandleCap)) {
                selectedIndex = index;
                Repaint();
            }
            if (selectedIndex == index) {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Move Point");
                    EditorUtility.SetDirty(spline);
                    spline[index] = handleTransform.InverseTransformPoint(point);
                }
            }
            return point;
        }

        Vector3[] showPoints(int curveIndex) {
            return new Vector3[] {
                showPoint(curveIndex),
                showPoint(curveIndex + 1),
                showPoint(curveIndex + 2)
            };
        }
    }
}
