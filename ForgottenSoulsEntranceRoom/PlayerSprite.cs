using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Path = System.IO.Path;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common.Decomposition;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using tainicom.Aether.Physics2D.Common.PhysicsLogic;

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
        private Body body;


        public int score = 0;

        public bool Colliding { get; protected set; }

        

        public Color color { get; set; } = Color.White;

        public PlayerSprite(float radius, Body body)
        {
            this.body = body;
            body.BodyType = BodyType.Dynamic;
            this.radius = radius;
            scale = radius / 32;
            origin = new Vector2(32, 32);
            this.body.OnCollision += CollisionHandler;

        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Chicken");
        }



        public void Update(GameTime gameTime)
        {
            Colliding = false;

            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();



            body.LinearVelocity += gamePadState.ThumbSticks.Left * 100 * new Vector2(1, -1);
            body.Position += gamePadState.ThumbSticks.Left * 2 * new Vector2(1, -1);

            if(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                body.LinearVelocity += new Vector2(0,-100);
                body.Position += new Vector2(0, -2);

            }
            if(keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                body.LinearVelocity += new Vector2(0, 100);
                body.Position += new Vector2(0, 2);

            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                body.LinearVelocity += new Vector2(-100, 0);
                body.Position += new Vector2(-2, 0);

            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                body.LinearVelocity += new Vector2(100, 0);
                body.Position += new Vector2(2, 0);

            }



        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = (Colliding) ? Color.Orange : Color.White;
            spriteBatch.Draw(texture, body.Position, null, color, body.Rotation, origin, scale, SpriteEffects.None, 0);
        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            fixture.Restitution = 10.0f;
            Colliding = true;
            if(other.Body.BodyType == BodyType.Dynamic) score += 1;
            return true;
        }
    }
}
