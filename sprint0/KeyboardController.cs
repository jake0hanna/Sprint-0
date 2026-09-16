using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using sprint0.Input;

namespace sprint0
{
    public class KeyboardController : interfaces.IController
    {
        public List<InputType> HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            List<InputType> input = new();

            if (keyboardState.IsKeyDown(Keys.Left)) input.Add(InputType.Left);
            if (keyboardState.IsKeyDown(Keys.Right)) input.Add(InputType.Right);
            if (keyboardState.IsKeyDown(Keys.Up)) input.Add(InputType.Up);
            if (keyboardState.IsKeyDown(Keys.Down)) input.Add(InputType.Down);
            if (keyboardState.IsKeyDown(Keys.Escape)) input.Add(InputType.Exit);
            if (keyboardState.IsKeyDown(Keys.Space)) input.Add(InputType.Fireball);

            return input;

        }

    }


}