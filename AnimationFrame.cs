using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkingGame
{
    public class AnimationFrame
    {
        public AnimationFrame(Rectangle sourceRectangle, double Duration)
        {
            SourceRectangle = sourceRectangle;
            this.Duration = Duration;
        }

        public Rectangle SourceRectangle { get; private set; }
        public double Duration { get; private set; }
    }
}
