using System;
using Anymotion.Graph.Managed;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Anymotion.Graph
{
    public struct NativeGraph : IDisposable
    {
        private UnsafeAppendBuffer _persistentValueBuffer;
        private UnsafeAppendBuffer _nodes;

        public NativeGraph(int persistentValueCount, AnymotionGraph managedGraph)
        {
            _persistentValueBuffer = new UnsafeAppendBuffer(persistentValueCount, 4, Allocator.Persistent);
            _nodes = new UnsafeAppendBuffer(1, 8, Allocator.Persistent);
        }

        public void Dispose()
        {
            _persistentValueBuffer.Dispose();
            _nodes.Dispose();
        }
    }
}