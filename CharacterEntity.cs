using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkingGame
{
    public class CharacterEntity
    {
        private class CharacterAnimator : Animator
        {
            private static Texture2D s_characterSheetTexture;

            private readonly CharacterEntity _characterEntity;

            private Animation _walkLeft;
            private Animation _walkRight;
            private Animation _walkUp;
            private Animation _walkDown;

            private Animation _standLeft;
            private Animation _standRight;
            private Animation _standUp;
            private Animation _standDown;


            private Animation _currentAnimation;

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
                    _currentAnimation = value switch
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

            public CharacterAnimator(GraphicsDevice graphicsDevice, CharacterEntity characterEntity)
            {
                if (s_characterSheetTexture == null)
                {
                    using (var stream = TitleContainer.OpenStream("Content/charactersheet.png"))
                    {
                        s_characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
                    }
                }

                _characterEntity = characterEntity;
                _standDown = new Animation();
                _standDown.AddFrame(new Rectangle(0, 0, 16, 16), 1);

                _standUp = new Animation();
                _standUp.AddFrame(new Rectangle(144, 0, 16, 16), 1);

                _standLeft = new Animation();
                _standLeft.AddFrame(new Rectangle(48, 0, 16, 16), 1);

                _standRight = new Animation();
                _standRight.AddFrame(new Rectangle(96, 0, 16, 16), 1);

                _walkLeft = new Animation();
                _walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), 0.1);
                _walkLeft.AddFrame(new Rectangle(64, 0, 16, 16), 0.1);
                _walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), 0.1);
                _walkLeft.AddFrame(new Rectangle(80, 0, 16, 16), 0.1);

                _walkRight = new Animation();
                _walkRight.AddFrame(new Rectangle(96, 0, 16, 16), 0.1);
                _walkRight.AddFrame(new Rectangle(112, 0, 16, 16), 0.1);
                _walkRight.AddFrame(new Rectangle(96, 0, 16, 16), 0.1);
                _walkRight.AddFrame(new Rectangle(128, 0, 16, 16), 0.1);

                _walkUp = new Animation();
                _walkUp.AddFrame(new Rectangle(160, 0, 16, 16), 0.1);
                _walkUp.AddFrame(new Rectangle(176, 0, 16, 16), 0.1);

                _walkDown = new Animation();
                _walkDown.AddFrame(new Rectangle(16, 0, 16, 16), 0.1);
                _walkDown.AddFrame(new Rectangle(32, 0, 16, 16), 0.1);
                _currentAnimation = _standDown;
            }


            public override void Update(GameTime gameTime)
            {
                _currentAnimation.Update(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(s_characterSheetTexture,
               new Rectangle(new Vector2(_characterEntity.X, _characterEntity.Y).ToPoint(), new Point(50, 50)),
               _currentAnimation.TextureRectangle,
               Color.White);
            }
        }


        private enum CharacterState
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

        private CharacterAnimator _animationController;
        private float _moveSpeed = 200;

        public CharacterEntity(GraphicsDevice graphicsDevice)
        {
            _animationController = new CharacterAnimator(graphicsDevice, this);
        }

        public float X { get; set; }
        public float Y { get; set; }

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
                    _animationController.State = targetVelocity.X > 0 ? CharacterState.WalkRight :
                        CharacterState.WalkLeft;
                }
                else
                {
                    _animationController.State = targetVelocity.Y > 0 ? CharacterState.WalkDown :
                        CharacterState.WalkUp;
                }
            }
            else
            {
                if (_animationController.State == CharacterState.WalkLeft)
                {
                    _animationController.State = CharacterState.StandLeft;
                }
                else if (_animationController.State == CharacterState.WalkRight)
                {
                    _animationController.State = CharacterState.StandRight;
                }
                else if (_animationController.State == CharacterState.WalkUp)
                {
                    _animationController.State = CharacterState.StandUp;
                }
                else if (_animationController.State == CharacterState.WalkDown)
                {
                    _animationController.State = CharacterState.StandDown;
                }
            }

            _animationController.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _animationController.Draw(spriteBatch);
        }

    }
}
