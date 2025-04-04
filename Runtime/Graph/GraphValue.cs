using System;

namespace Anymotion.Graph
{
    [Serializable]
    public struct GraphValue<T> where T : unmanaged
    {
        public enum ValueMode
        {
            /// <summary>
            /// A constant value defined in the graph.
            /// </summary>
            Constant,
            /// <summary>
            /// A value obtained after the execution of some node
            /// </summary>
            Node,
            /// <summary>
            /// 
            /// </summary>
            Persistent
        }
        
        private T _defaultValue;
        private ValueMode _valueMode;
        private int _nodeId;
        private int _nodeValueId;
        private int _persistentId;
        private T _value;
        private bool _hasValue;

        public T Value
        {
            get
            {
                if (!_hasValue)
                    ProcessValue();
                
                return _value;
            }
            set
            {
                if (_valueMode != ValueMode.Persistent)
                    return;
                
                _value = value;
                _hasValue = true;
            }
        }

        private void ProcessValue()
        {
            throw new NotImplementedException();
        }
    }
}