using UnityEngine;

namespace Anymotion.Graph.Managed
{
    [CreateAssetMenu(fileName = "Anymotion Graph", menuName = "Anymotion/Graph", order = 0)]
    public class AnymotionGraphSO : ScriptableObject
    {
        public AnymotionGraph graph;
    }
}