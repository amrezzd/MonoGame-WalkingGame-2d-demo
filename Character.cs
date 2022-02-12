using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkingGame
{
    public class Character
    {
        private static Texture2D s_characterSheetTexture;

        private CharacterAnimator _animator = new CharacterAnimator();
        private float _moveSpeed = 200;

        public Character(GraphicsDevice graphicsDevice)
        {
            if (s_characterSheetTexture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/charactersheet.png"))
                {
                    s_characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
        }

        public enum CharacterState
        {
            WalkLeft,
            WalkRight,
            WalkUp,
            WalkDown,
            StandLeft,
            StandRight,
            StandUp,
            StandDown
        }

        public float X { get; set; }
        public float Y { get; set; }

        private CharacterState _state = CharacterState.StandDown;

        public CharacterState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                _animator.OnStateChanged(_state);
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 targetVelocity = new Vector2(mouseState.X - this.X, mouseState.Y - this.Y);
                
                if (targetVelocity.X != 0 || targetVelocity.Y != 0)
                {
                    targetVelocity.Normalize();
                    targetVelocity *= _moveSpeed;
                }
                this.X += targetVelocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.Y += targetVelocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Math.Abs(targetVelocity.X) > Math.Abs(targetVelocity.Y))
                {
                    this.State = targetVelocity.X > 0 ? CharacterState.WalkRight :
                        CharacterState.WalkLeft;
                }
                else
                {
                    this.State = targetVelocity.Y > 0 ? CharacterState.WalkDown :
                        CharacterState.WalkUp;
                }
            }
            else
            {
                if (this.State == CharacterState.WalkLeft)
                {
                    this.State = CharacterState.StandLeft;
                }
                else if (this.State == CharacterState.WalkRight)
                {
                    this.State = CharacterState.StandRight;
                }
                else if (this.State == CharacterState.WalkUp)
                {
                    this.State = CharacterState.StandUp;
                }
                else if (this.State == CharacterState.WalkDown)
                {
                    this.State = CharacterState.StandDown;
                }
            }

            _animator.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(s_characterSheetTexture,
               new Rectangle(new Vector2(X, Y).ToPoint(), new Point(50, 50)),
               _animator.Animation.SourceRectangle,
               Color.White);
        }


        private class CharacterAnimator : Animator
        {

            private Animation _walkLeft = new Animation(
                new AnimationFrame(new Rectangle(48, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(64, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(48, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(80, 0, 16, 16), 0.1)
                );

            private Animation _walkRight = new Animation(
                new AnimationFrame(new Rectangle(96, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(112, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(96, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(128, 0, 16, 16), 0.1)
                );

            private Animation _walkUp = new Animation(
                new AnimationFrame(new Rectangle(160, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(176, 0, 16, 16), 0.1)
                );

            private Animation _walkDown = new Animation(
                new AnimationFrame(new Rectangle(16, 0, 16, 16), 0.1),
                new AnimationFrame(new Rectangle(32, 0, 16, 16), 0.1)
                );

            private Animation _standLeft = new Animation(
                new AnimationFrame(new Rectangle(48, 0, 16, 16), 1));
            private Animation _standRight = new Animation(
                new AnimationFrame(new Rectangle(96, 0, 16, 16), 1));
            private Animation _standUp = new Animation(
                new AnimationFrame(new Rectangle(144, 0, 16, 16), 1));
            private Animation _standDown = new Animation(
                new AnimationFrame(new Rectangle(0, 0, 16, 16), 1));

            private Animation _currentAnimation;

            public CharacterAnimator()
            {
                _currentAnimation = _standDown;
            }

            public override Animation Animation => _currentAnimation;

            public void OnStateChanged(CharacterState state)
            {
                _currentAnimation = state switch
                {
                    CharacterState.WalkLeft => _walkLeft,
                    CharacterState.WalkRight => _walkRight,
                    CharacterState.WalkUp => _walkUp,
                    CharacterState.WalkDown => _walkDown,
                    CharacterState.StandLeft => _standLeft,
                    CharacterState.StandRight => _standRight,
                    CharacterState.StandUp => _standUp,
                    CharacterState.StandDown => _standDown,
                    _ => _standDown
                };
            }
        }
    }
}
