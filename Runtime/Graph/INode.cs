using System.Collections;

namespace Anymotion.Graph
{
    public interface INode
    {
        public void Execute(in NativeGraph graph);
    }
}