namespace LD51
{
    using System;
    using UnityEditor;
    using UnityEngine;


    public class MultiButtonPopup : EditorWindow
    {
        private string caption;
        private (string, Action)[] buttons;
        
        public static void Show(string caption, params (string, Action)[] buttons)
        {
            MultiButtonPopup popup = ScriptableObject.CreateInstance<MultiButtonPopup>();
            var targetPosition = mouseOverWindow.position;
            popup.position = new Rect(targetPosition.x, targetPosition.y + 150, 250, 150 + (buttons.Length * 50));
            popup.caption = caption;
            popup.buttons = buttons;
            popup.ShowPopup();
        }


        void OnGUI()
        {
            EditorGUILayout.LabelField(caption);
            GUILayout.Space(70);

            foreach (var button in this.buttons)
            {
                if (GUILayout.Button(button.Item1))
                {
                    button.Item2.Invoke();
                    Close();
                }
                    
            }
            
            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
        }
    }
}