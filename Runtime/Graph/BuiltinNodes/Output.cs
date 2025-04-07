using Anymotion.Core;
using Unity.Collections;

namespace Anymotion.Graph.BuiltinNodes
{
    public struct Output : INode
    {
        public GraphInput<NativePose> OutputValues;
        
        public void Execute(in NativeGraph graph) { }
    }
}