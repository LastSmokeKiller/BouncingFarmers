using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace BouncingFarmers
{
    public class PlayerSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private Vector2 position = new Vector2(200, 200);


        private bool horizontal;
        private bool vertical;

        float radius;
        float scale;
        Vector2 origin;
        Body body;

        public int score = 0;

        public bool Colliding { get; protected set; }


        public Color color { get; set; } = Color.White;

        public PlayerSprite(float radius, Body body)
        {
            this.body = body;
            this.radius = radius;
            scale = radius / 32;
            origin = new Vector2(32, 32);
            this.body.OnCollision += CollisionHandler;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Farmer");
        }

        public void Update(GameTime gameTime)
        {
            Colliding = false;

            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            body.Position += gamePadState.ThumbSticks.Left * 100 * new Vector2(1, -1);
            if (gamePadState.ThumbSticks.Left.X < 0)
            {
                horizontal = true;
                vertical = false;
            }
            if (gamePadState.ThumbSticks.Left.X > 0)
            {
                horizontal = false;
                vertical = false;
            }

            if(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                body.Position += new Vector2(0, -5);
                horizontal = false;
                vertical = false;
            }
            if(keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                body.Position += new Vector2(0, 5) ;
                horizontal = false;
                vertical = true;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                body.Position += new Vector2(-5, 0);
                horizontal = true;
                vertical = false;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                body.Position += new Vector2(5, 0);
                horizontal = false;
                vertical = false;
            }


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = (Colliding) ? Color.Orange : Color.Blue;
            spriteBatch.Draw(texture, body.Position, null, color, body.Rotation, origin, scale, SpriteEffects.None, 0);
        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            
            Colliding = true;
            score += 1;
            return true;
        }
    }
}
