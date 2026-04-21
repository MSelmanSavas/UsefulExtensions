using UnityEditor;
using UnityEngine;

namespace HorseHead.Utils.UsefulExtensions.Editor
{
    public static class HandlesExtensions
    {
        public static void CenteredLabel(Vector3 position, string text, GUIStyle style = null)
        {
            if (style == null)
                style = GUI.skin.label; // default label style

            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = GetDynamicFontSize(position, 35f, 500f, 5, 25);

            //style.fontSize = 500;
            // Calculate text size
            Vector2 size = style.CalcSize(new GUIContent(text));

            // Convert world position to GUI point
            Vector3 screenPoint = HandleUtility.WorldToGUIPoint(position);

            // Offset so it's centered
            Rect rect = new Rect(
                screenPoint.x - size.x / 2f,
                screenPoint.y - size.y / 2f,
                size.x,
                size.y
            );

            // Draw in GUI space
            Handles.BeginGUI();
            GUI.Label(rect, text, style);
            Handles.EndGUI();
        }

        public static int GetDynamicFontSize(Vector3 worldPosition, float minDist, float maxDist, int minSize, int maxSize)
        {
            // Get the current Scene View camera
            Camera cam = SceneView.lastActiveSceneView?.camera;
            if (cam == null) return minSize;

            float distance = Vector3.Distance(cam.transform.position, worldPosition);

            // t is 0 at maxDist (or further) and 1 at minDist (or closer)
            float t = Mathf.InverseLerp(maxDist, minDist, distance);

            // Smoothstep makes the transition feel less "linear" and more natural
            t = Mathf.SmoothStep(0, 1, t);

            return (int)Mathf.Lerp(minSize, maxSize, t);
        }
    }
}
