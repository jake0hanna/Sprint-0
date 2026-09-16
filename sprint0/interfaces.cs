using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sprint0.Input;


namespace interfaces
{
	public enum Animation
	{
		Idle,
		Moving,
		Dead
	}

	public interface IController
	{

		//bool isActive();

		List<InputType> HandleInput();





	}

	public interface ISprite
	{

		void LoadContent(ContentManager contentManager);
		void Update(Animation state, Vector2 movement);
		void Draw(SpriteBatch spriteBatch, Vector2 position);





	}

	public interface IPlayer
	{
		void Update();
		void LoadContent(ContentManager content);
		void Draw(SpriteBatch spriteBatch);
	}

}