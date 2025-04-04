using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anymotion.Graph.Managed
{
    [Serializable]
    public class AnymotionGraph : INode
    {
        [SerializeReference] private List<INode> nodes;
    }
}