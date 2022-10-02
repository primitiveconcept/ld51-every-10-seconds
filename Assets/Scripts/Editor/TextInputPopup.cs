namespace LD51
{
    using System;
    using UnityEditor;
    using UnityEngine;


    public class TextInputPopup : EditorWindow
    {
        private string caption;
        private Action<string> action;
        private string input = string.Empty;
        
        public static void Show(string caption, Action<string> action)
        {
            TextInputPopup popup = ScriptableObject.CreateInstance<TextInputPopup>();
            var targetPosition = mouseOverWindow.position;
            popup.position = new Rect(targetPosition.x, targetPosition.y + 150, 250, 150);
            popup.caption = caption;
            popup.action = action;
            popup.ShowPopup();
        }


        void OnGUI()
        {
            EditorGUILayout.LabelField(caption);
            this.input = EditorGUILayout.TextField(this.input);
            GUILayout.Space(70);
            
            if (GUILayout.Button("OK"))
            {
                this.action.Invoke(this.input);
                Close();
            }

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
        }
    }
}