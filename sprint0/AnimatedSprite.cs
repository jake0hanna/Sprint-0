using interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace sprint0
{
    internal class AnimatedSprite : ISprite
    {
        private const int Scale = 3;
        private const int HorizontalOffset = 4;
        private const int FrameStrideX = 29;
        private const int FrameWidth = 28;
        private const int FrameHeight = 15;
        private const int FrameCrop = 2;
        private const int AnimationFrameDelay = 8;
        private static readonly int[] LeftRun = { 5, 4, 3 };
        private static readonly int[] RightRun = { 8, 9, 10 };

        private Texture2D texture;
        private bool facingRight = true;
        private int runFrame;
        private int animationTimer;
        private Rectangle currentSourceRectangle = NormalFrame(7);

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("mario");
        }

        public void Update(Animation state, Vector2 movement)
        {
            if (movement.X < 0)
                facingRight = false;
            else if (movement.X > 0)
                facingRight = true;

            if (state == Animation.Dead)
            {
                currentSourceRectangle = new Rectangle(0, 16, 14, 13);
                return;
            }

            if (state != Animation.Moving || movement == Vector2.Zero)
            {
                animationTimer = 0;
                runFrame = 0;
                currentSourceRectangle = NormalFrame(facingRight ? 7 : 6);
                return;
            }

            if (++animationTimer < AnimationFrameDelay)
                return;

            animationTimer = 0;
            int[] frames = facingRight ? RightRun : LeftRun;
            currentSourceRectangle = NormalFrame(frames[runFrame++ % frames.Length]);
        }

        private static Rectangle NormalFrame(int column)
        {
            return new Rectangle(
                FrameStrideX * column + FrameCrop,
                0,
                FrameWidth - FrameCrop * 2,
                FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (texture == null)
            {
                return;
            }

            Rectangle destinationRectangle = new Rectangle(
                (int)position.X + HorizontalOffset * Scale,
                (int)position.Y,
                currentSourceRectangle.Width * Scale,
                currentSourceRectangle.Height * Scale);

            spriteBatch.Draw(texture, destinationRectangle, currentSourceRectangle, Color.White);
        }
    }
}
