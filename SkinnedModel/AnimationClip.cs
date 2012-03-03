using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModelPipeline
{
    public class AnimationClip
    {
        // Total length of the clip
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }

        // List of keyframes for all bones, sorted by time
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }

        public AnimationClip(TimeSpan Duration, List<Keyframe> Keyframes)
        {
            this.Duration = Duration;
            this.Keyframes = Keyframes;
        }

        private AnimationClip()
        {
        }
    }
}
