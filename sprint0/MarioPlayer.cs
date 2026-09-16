using System;
using System.Collections.Generic;
using interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sprint0.Input;

namespace sprint0
{
    internal class MarioPlayer : IPlayer
    {
        private readonly IController keyboardController;
        private readonly IController mouseController;
        private readonly ISprite sprite;
        private readonly Rectangle movementBounds;

        private const float MaxHorizontalSpeed = 4f, HorizontalAcceleration = 0.6f, HorizontalFriction = 0.8f;
        private const float DeathFallSpeed = 6f, Gravity = 0.5f, JumpVelocity = -10f;
        private const float SpriteWidth = 72f, SpriteHeight = 45f;

        private Vector2 position;
        private bool isDying;
        private float horizontalVelocity;
        private float verticalVelocity;


        public MarioPlayer(Rectangle movementBounds)
        {
            keyboardController = new KeyboardController();
            mouseController = new MouseController();
            sprite = new AnimatedSprite();
            this.movementBounds = movementBounds;
            Reset();
        }

        public void Update()
        {
            if (isDying)
            {
                UpdateDeath();
                return;
            }

            List<InputType> keyboardInput = keyboardController.HandleInput();
            List<InputType> mouseInput = mouseController.HandleInput();

            if (keyboardInput.Contains(InputType.Exit) || mouseInput.Contains(InputType.Exit))
                Environment.Exit(0);

            if (mouseInput.Contains(InputType.Kill))
            {
                isDying = true;
                horizontalVelocity = verticalVelocity = 0f;
                sprite.Update(Animation.Dead, Vector2.Zero);
                return;
            }

            if (keyboardInput.Contains(InputType.Up) && IsGrounded())
            {
                verticalVelocity = JumpVelocity;
            }

            UpdateHorizontalVelocity(keyboardInput);
            Vector2 movement = new(horizontalVelocity, 0f);
            position.X += horizontalVelocity;
            verticalVelocity += Gravity;
            position.Y += verticalVelocity;

            if (position.Y >= GroundPosition())
            {
                position.Y = GroundPosition();
                verticalVelocity = 0f;
            }

            float leftBoundary = movementBounds.Left;
            float rightBoundary = movementBounds.Right - SpriteWidth;
            position.X = MathHelper.Clamp(position.X, leftBoundary, rightBoundary);
            if (position.X == leftBoundary || position.X == rightBoundary)
            {
                horizontalVelocity = 0f;
            }
            sprite.Update(movement == Vector2.Zero ? Animation.Idle : Animation.Moving, movement);
        }

        private void UpdateDeath()
        {
            position.Y += DeathFallSpeed;
            sprite.Update(Animation.Dead, Vector2.Zero);
            if (position.Y > movementBounds.Bottom + 60)
            {
                isDying = false;
                Reset();
                sprite.Update(Animation.Idle, Vector2.Zero);
            }
        }

        private void Reset()
        {
            horizontalVelocity = verticalVelocity = 0f;
            position = new Vector2(movementBounds.Center.X, GroundPosition());
        }

        private float GroundPosition()
        {
            return movementBounds.Bottom - SpriteHeight;
        }

        private bool IsGrounded()
        {
            return position.Y >= GroundPosition();
        }

        private void UpdateHorizontalVelocity(List<InputType> input)
        {
            bool movingLeft = input.Contains(InputType.Left);
            bool movingRight = input.Contains(InputType.Right);

            if (movingLeft && !movingRight)
            {
                horizontalVelocity -= HorizontalAcceleration;
            }
            else if (movingRight && !movingLeft)
            {
                horizontalVelocity += HorizontalAcceleration;
            }
            else
            {
                horizontalVelocity *= HorizontalFriction;
                if (Math.Abs(horizontalVelocity) < 0.05f)
                {
                    horizontalVelocity = 0f;
                }
            }

            horizontalVelocity = MathHelper.Clamp(
                horizontalVelocity,
                -MaxHorizontalSpeed,
                MaxHorizontalSpeed);
        }

        public void LoadContent(ContentManager content)
        {
            sprite.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
