using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using sprint0.Input;

namespace sprint0
{
    public class MouseController : interfaces.IController
    {
        private ButtonState previousLeftButton = ButtonState.Released;

        public List<InputType> HandleInput()
        {
            List<InputType> input = new List<InputType>();
            ButtonState currentLeftButton = Mouse.GetState().LeftButton;
            bool clicked = currentLeftButton == ButtonState.Pressed &&
                           previousLeftButton == ButtonState.Released;
            previousLeftButton = currentLeftButton;

            if (clicked)
            {
                input.Add(InputType.Kill);
            }

            return input;

        }

    }
}
