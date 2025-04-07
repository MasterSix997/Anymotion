using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anymotion.Graph.Managed
{
    [Serializable]
    public class AnymotionGraph
    {
        [SerializeReference] private List<INode> nodes;
        
    }
}