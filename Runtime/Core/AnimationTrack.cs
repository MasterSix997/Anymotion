using System;
using System.Linq;
using Anymotion.Utils;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

namespace Anymotion.Core
{
    [Serializable]
    public class MotionClipAsset : ScriptableObject
    {
        [CanBeNull] public AnimationClip originalClip;
        public float lenght;
        public ManagedMotionTrack[] motionTracks;
        
        public NativeClip ToNative()
        {
            return new NativeClip
            {
                Lenght = lenght,
                MotionTracks = new NativeArray<NativeMotionTrack>(motionTracks.Select(x => x.ToNative()).ToArray(), Allocator.Persistent),
            };
        }
    }
    
    public struct NativeClip : IDisposable
    {
        public float Lenght;
        public NativeArray<NativeMotionTrack> MotionTracks;
        
        public void Dispose()
        {
            MotionTracks.Dispose();
        }
    }
    
    [Serializable]
    public class ManagedMotionTrack
    {
        public int componentTypeHash;
        public int propertyHash;
        public AnimationCurve curve;
        public ObjectRefKey[] objectRefKeys;
        
        public NativeMotionTrack ToNative()
        {
            return new NativeMotionTrack
            {
                ComponentTypeHash = componentTypeHash,
                PropertyHash = propertyHash,
                Curve = curve.ToNative(),
                ObjectRefKeys = new NativeArray<ObjectRefKey>(objectRefKeys, Allocator.Persistent),
            };
        }
    }

    public struct NativeMotionTrack : IDisposable
    {
        public int ComponentTypeHash;
        public int PropertyHash;
        public NativeCurve Curve;
        public NativeArray<ObjectRefKey> ObjectRefKeys;

        public void Dispose()
        {
            Curve.Dispose();
            ObjectRefKeys.Dispose();
        }
    }

    [Serializable]
    public struct ObjectRefKey
    {
        public float Time;
        public int AssetId; // reference of ID predefined
    }
}