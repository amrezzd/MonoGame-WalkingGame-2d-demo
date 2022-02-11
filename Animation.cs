using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WalkingGame
{
    public class Animation
    {
        private List<AnimationFrame> _frames = new List<AnimationFrame>();
        private double _animationTime;

        public double Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in _frames)
                {
                    totalSeconds += frame.Duration;
                }
                return totalSeconds;
            }
        }

        public Rectangle TextureRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;
                double accumulatedTime = 0;

                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.Duration > _animationTime)
                    {
                        currentFrame = frame;
                        break;
                    } 
                    else
                    {
                        accumulatedTime += frame.Duration;
                    }
                }

                if (currentFrame == null)
                {
                    currentFrame = _frames.LastOrDefault();
                }

                if (currentFrame == null)
                {
                    return Rectangle.Empty;
                }

                return currentFrame.SourceRectangle;
            }
        }

        public void AddFrame(Rectangle sourceRect, double duration)
        {
            _frames.Add(new AnimationFrame(sourceRect, duration));
        }

        public void Update(GameTime gameTime)
        {
            double secondsToAnimation = _animationTime + gameTime.ElapsedGameTime.TotalSeconds;
            if (secondsToAnimation > Duration)
            {
                secondsToAnimation = 0;
            }
            _animationTime = secondsToAnimation;
        }
    }
}
