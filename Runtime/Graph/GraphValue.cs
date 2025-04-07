using System;

namespace Anymotion.Graph
{
    public struct GraphInput<T> where T : unmanaged
    {
        private bool _hasValue;
        private int _graphId;
        private int _nodeId;
        private T _value;

        public T Value
        {
            get
            {
                if (!_hasValue && _graphId != -1)
                    EvaluateValueFromNode();

                return _value;
            }
        }

        internal void SetValue(T value)
        {
            _value = value;
        }

        internal void EvaluateValueFromNode()
        {
            
        }
    }
    
    public struct GraphOutput<T> where T : unmanaged
    {
        private bool _hasValue;
        private T _value;

        public T Value
        {
            get
            {
                if (!_hasValue)
                    throw new Exception("Output not set");

                return _value;
            }
            set
            {
                _hasValue = true;
                _value = value;
            }
        }
    }
}