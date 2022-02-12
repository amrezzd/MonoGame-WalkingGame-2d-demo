using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace WalkingGame
{
    public class Animation
    {
        private readonly List<AnimationFrame> _frames = new List<AnimationFrame>();
        private double _animationTimeFrame;

        public Animation(params AnimationFrame[] frames)
        {
            foreach (var animationFrame in frames)
            {
                _frames.Add(animationFrame);
            }
        }

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

        public Rectangle SourceRectangle
        {
            get
            {
                if (_frames.Count == 0)
                {
                    return Rectangle.Empty;
                }

                AnimationFrame currentFrame = null;
                double accumulatedTime = 0;

                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.Duration > _animationTimeFrame)
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
                    currentFrame = _frames[0];
                }

                return currentFrame.SourceRectangle;
            }
        }

        public void Update(GameTime gameTime)
        {
            double timeFrame = _animationTimeFrame + gameTime.ElapsedGameTime.TotalSeconds;
            if (timeFrame > Duration)
            {
                timeFrame = 0;
            }
            _animationTimeFrame = timeFrame;
        }
    }
}
