using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Anymotion.Editor.Graph
{
    public class BaseGraphView : GraphView
    {
        
        public BaseGraphView()
        {
            AddGridBackground();
            AddManipulators();
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale, ContentZoomer.DefaultScaleStep, ContentZoomer.DefaultReferenceScale);
        }

        private void AddGridBackground()
        {
            var grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }
    }
}