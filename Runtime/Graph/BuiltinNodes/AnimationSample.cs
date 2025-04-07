using Anymotion.Core;

namespace Anymotion.Graph.BuiltinNodes
{
    public struct AnimationSample : INode
    {
        public GraphInput<NativeClip> NativeClip;
        public GraphInput<float> Time;
        public GraphOutput<NativePose> OutputPose;
        
        public void Execute(in NativeGraph graph)
        {
            foreach (var track in NativeClip.Value.MotionTracks)
            {
                var index = 0;
                var value = GetSampleValue(track, Time.Value);
                OutputPose.Value.SetValue(index, track);
            }
        }

        private float GetSampleValue(NativeMotionTrack track, float time)
        {
            return track.Curve.Evaluate(time);
        }
    }
}