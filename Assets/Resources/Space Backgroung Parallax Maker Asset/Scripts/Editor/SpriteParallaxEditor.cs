using UnityEditor;

using UnityEngine;
namespace Mkey
{
    [CustomEditor(typeof(SpriteParallax))]
    public class SpriteParallaxEditor : Editor
    {
        private bool showDefault;
        public override void OnInspectorGUI()
        {
            var script = target as SpriteParallax;
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Fill the array with Parallax Planes.");
            EditorGUILayout.LabelField("Empty fields will be ignored in Game mode.");

            EditorGUI.indentLevel += 1;
            var sProp = serializedObject.FindProperty("planes");
            var guiContent = new GUIContent();
            guiContent.text = sProp.displayName;
            EditorGUILayout.PropertyField(sProp, guiContent, true);
            EditorGUI.indentLevel -= 1;

            EditorGUILayout.Space();
            EditorGUILayout.Space();


            EditorGUILayout.LabelField("First(foreground) plane relative offset (0-0.1).");
            sProp = serializedObject.FindProperty("firstPlaneRelativeOffset");
            guiContent = new GUIContent();
            guiContent.text = sProp.displayName;
            EditorGUILayout.PropertyField(sProp, guiContent, true);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Last(background) plane relative offset (0.9-1).");
            sProp = serializedObject.FindProperty("lastPlaneRelativeOffset");
            guiContent = new GUIContent();
            guiContent.text = sProp.displayName;
            EditorGUILayout.PropertyField(sProp, guiContent, true);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Create infinite map.");
            sProp = serializedObject.FindProperty("infiniteMap");
            guiContent = new GUIContent();
            guiContent.text = sProp.displayName;
            EditorGUILayout.PropertyField(sProp, guiContent, true);

            if (sProp.boolValue)
            {
                EditorGUILayout.LabelField("Set map size X for infinite scrolling.");
                sProp = serializedObject.FindProperty("mapSizeX");
                guiContent = new GUIContent();
                guiContent.text = sProp.displayName;
                EditorGUILayout.PropertyField(sProp, guiContent, true);
                EditorGUILayout.LabelField("Set map size Y for infinite scrolling.");
                sProp = serializedObject.FindProperty("mapSizeY");
                guiContent = new GUIContent();
                guiContent.text = sProp.displayName;
                EditorGUILayout.PropertyField(sProp, guiContent, true);
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUI.indentLevel += 1;
            if (showDefault = EditorGUILayout.Foldout(showDefault, "Default Inspector"))
            {
                DrawDefaultInspector();
            }
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }
    }
}