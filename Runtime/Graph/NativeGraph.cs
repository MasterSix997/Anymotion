using System;
using Anymotion.Core;
using Anymotion.Graph.Managed;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Anymotion.Graph
{
    public struct NativeGraph : IDisposable
    {
        // private UnsafeAppendBuffer _persistentValueBuffer;
        private UnsafeAppendBuffer _nodes;
        public NativePose CurrentPose;
        public BoneMapping BoneMapping;

        public NativeGraph(AnymotionGraph graph)
        {
            // _persistentValueBuffer = new UnsafeAppendBuffer(persistentValueCount, 4, Allocator.Persistent);
            _nodes = new UnsafeAppendBuffer(1, 8, Allocator.Persistent);
            CurrentPose = new NativePose();
        }

        public void Dispose()
        {
            // _persistentValueBuffer.Dispose();
            _nodes.Dispose();
        }
    }
}