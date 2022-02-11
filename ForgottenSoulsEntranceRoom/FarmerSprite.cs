using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Microsoft.Xna.Framework.Audio;


namespace BouncingFarmers
{
    public class FarmerSprite
    {
        private Vector2 position;
        float radius;
        float scale;
        Vector2 origin;
        Body body;

        private SoundEffect bounce;


        private Texture2D texture;
        private SpriteFont font;


        public bool DisplayMessage { get; set; } = false;

        public bool Colliding { get; protected set; }

        public FarmerSprite(float radius, Body body)
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
            font = content.Load<SpriteFont>("menufont");
            bounce = content.Load<SoundEffect>("Hit_Hurt18");
        }

        public void Update(GameTime gameTime)
        {
            Colliding = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, body.Position, null, Color.White, body.Rotation, origin, scale, SpriteEffects.None, 0 );
        }



        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            Colliding = true;
            bounce.Play();
            return true;
        }

    }
}
