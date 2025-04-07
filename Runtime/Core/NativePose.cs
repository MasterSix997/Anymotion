using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Anymotion.Core
{
    // Define um tipo de valor que pode representar qualquer tipo de dados animado
    [StructLayout(LayoutKind.Explicit)]
    public struct AnimationValue
    {
        // União para armazenar diferentes tipos primitivos
        [FieldOffset(0)] public float FloatValue;
        [FieldOffset(0)] public int IntValue;
        [FieldOffset(0)] public bool BoolValue;
        [FieldOffset(0)] public float3 Float3Value;
        [FieldOffset(0)] public quaternion QuaternionValue;

        // Tipo do valor armazenado
        [FieldOffset(16)] public AnimationValueType ValueType;

        // Construtor para float
        public AnimationValue(float value)
        {
            this = default;
            FloatValue = value;
            ValueType = AnimationValueType.Float;
        }

        // Construtor para int
        public AnimationValue(int value)
        {
            this = default;
            IntValue = value;
            ValueType = AnimationValueType.Int;
        }

        // Construtor para bool
        public AnimationValue(bool value)
        {
            this = default;
            BoolValue = value;
            ValueType = AnimationValueType.Bool;
        }

        // Construtor para float3
        public AnimationValue(float3 value)
        {
            this = default;
            Float3Value = value;
            ValueType = AnimationValueType.Float3;
        }

        // Construtor para quaternion
        public AnimationValue(quaternion value)
        {
            this = default;
            QuaternionValue = value;
            ValueType = AnimationValueType.Quaternion;
        }

        // Método para converter de volta para o tipo correto
        public T GetValue<T>() where T : struct
        {
            switch (ValueType)
            {
                case AnimationValueType.Float when typeof(T) == typeof(float):
                    return (T)(object)FloatValue;
                case AnimationValueType.Int when typeof(T) == typeof(int):
                    return (T)(object)IntValue;
                case AnimationValueType.Bool when typeof(T) == typeof(bool):
                    return (T)(object)BoolValue;
                case AnimationValueType.Float3 when typeof(T) == typeof(float3):
                    return (T)(object)Float3Value;
                case AnimationValueType.Quaternion when typeof(T) == typeof(quaternion):
                    return (T)(object)QuaternionValue;
                default:
                    throw new InvalidCastException($"Cannot convert from {ValueType} to {typeof(T)}");
            }
        }
    }

    // Enum para identificar o tipo de valor armazenado
    public enum AnimationValueType : byte
    {
        Float,
        Int,
        Bool,
        Float3,
        Quaternion
        // Adicione mais tipos conforme necessário
    }

    // Array seguro para Jobs que armazena valores de animação
    public unsafe struct UnsafeAnimationValueArray : IDisposable
    {
        // Ponteiro para os dados
        [NativeDisableUnsafePtrRestriction]
        private AnimationValue* _ptr;
        
        // Comprimento do array
        private int _length;
        
        // Alocador usado
        private Allocator _allocator;
        
        // Flag para verificar se o array foi criado
        private bool _isCreated;
        
        // Construtor
        public UnsafeAnimationValueArray(int length, Allocator allocator)
        {
            if (allocator <= Allocator.None)
                throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", nameof(allocator));
            
            _length = length;
            _allocator = allocator;
            _ptr = (AnimationValue*)UnsafeUtility.Malloc(
                UnsafeUtility.SizeOf<AnimationValue>() * length,
                UnsafeUtility.AlignOf<AnimationValue>(),
                allocator);
            
            UnsafeUtility.MemClear(_ptr, UnsafeUtility.SizeOf<AnimationValue>() * length);
            _isCreated = true;
        }
        
        // Acessador de array
        public AnimationValue this[int index]
        {
            get
            {
                CheckBounds(index);
                return UnsafeUtility.ReadArrayElement<AnimationValue>(_ptr, index);
            }
            set
            {
                CheckBounds(index);
                UnsafeUtility.WriteArrayElement(_ptr, index, value);
            }
        }
        
        // Método para definir um valor tipado diretamente
        public void Set<T>(int index, T value) where T : struct
        {
            CheckBounds(index);
            
            if (typeof(T) == typeof(float))
                this[index] = new AnimationValue((float)(object)value);
            else if (typeof(T) == typeof(int))
                this[index] = new AnimationValue((int)(object)value);
            else if (typeof(T) == typeof(bool))
                this[index] = new AnimationValue((bool)(object)value);
            else if (typeof(T) == typeof(float3))
                this[index] = new AnimationValue((float3)(object)value);
            else if (typeof(T) == typeof(quaternion))
                this[index] = new AnimationValue((quaternion)(object)value);
            else
                throw new ArgumentException($"Type {typeof(T)} is not supported");
        }
        
        // Método para obter um valor tipado
        public T Get<T>(int index) where T : struct
        {
            CheckBounds(index);
            return this[index].GetValue<T>();
        }
        
        // Verificação de limites
        private void CheckBounds(int index)
        {
            if (!_isCreated)
                throw new ObjectDisposedException("UnsafeAnimationValueArray has been disposed");
                
            if (index < 0 || index >= _length)
                throw new IndexOutOfRangeException();
        }
        
        // Propriedade para verificar comprimento
        public int Length => _length;
        
        // Propriedade para verificar se foi inicializado
        public bool IsCreated => _isCreated;
        
        // Disposição dos recursos
        public void Dispose()
        {
            if (!_isCreated)
                return;
                
            UnsafeUtility.Free(_ptr, _allocator);
            _ptr = null;
            _isCreated = false;
        }
    }

    // Struct para gerenciar uma pose completa (coleção de valores de animação)
    public struct NativePose : IDisposable
    {
        private UnsafeAnimationValueArray _values;
        
        // Construtor para criar uma pose com um número específico de elementos
        public NativePose(int elementCount, Allocator allocator)
        {
            _values = new UnsafeAnimationValueArray(elementCount, allocator);
        }
        
        // Define valores para elementos específicos da pose
        public void SetValue<T>(int index, T value) where T : struct
        {
            _values.Set(index, value);
        }
        
        // Obtém valores de elementos específicos da pose
        public T GetValue<T>(int index) where T : struct
        {
            return _values.Get<T>(index);
        }
        
        // Acesso direto ao array interno
        public AnimationValue this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
        
        // Propriedade para verificar comprimento
        public int Length => _values.Length;
        
        // Propriedade para verificar se foi inicializado
        public bool IsCreated => _values.IsCreated;
        
        // Disposição dos recursos
        public void Dispose()
        {
            _values.Dispose();
        }
    }

    public struct BoneMapping
    {
        public NativeArray<>
    }
}