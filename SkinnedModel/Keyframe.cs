using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SkinnedModelPipeline
{
    public class Keyframe
    {
        // Index of the bone this keyframe animates
        [ContentSerializer]
        public int Bone { get; private set; }

        // Time from the beginning of the animation of this keyframe
        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        // Bone transform for this keyframe
        [ContentSerializer]
        public Matrix Transform { get; private set; }

        public Keyframe(int Bone, TimeSpan Time, Matrix Transform)
        {
            this.Bone = Bone;
            this.Time = Time;
            this.Transform = Transform;
        }

        private Keyframe()
        {
        }
    }
}
