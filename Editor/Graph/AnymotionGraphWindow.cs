using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Anymotion.Editor.Graph
{
    public class AnymotionGraphWindow : EditorWindow
    {
        public StyleSheet graphStyleSheet;
        
        [MenuItem("Window/Animation/Anymotion")]
        public static void ShowWindow()
        {
            GetWindow<AnymotionGraphWindow>("Anymotion Graph");
        }

        private void CreateGUI()
        {
            var graphView = new BaseGraphView();
            graphView.StretchToParentSize();
            graphView.styleSheets.Add(graphStyleSheet);
            
            rootVisualElement.Add(graphView);
        }
    }
}