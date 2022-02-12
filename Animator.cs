using Microsoft.Xna.Framework;

namespace WalkingGame
{
    public abstract class Animator
    {
        public abstract Animation Animation { get; }
        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }
    }
}
