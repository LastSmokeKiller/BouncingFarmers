using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BouncingFarmers
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D background;

        private List<FarmerSprite> farmers;
        private PlayerSprite player;
        private SpriteFont font;
        private World world;

        private SoundEffect bounce;
        private Song backgroundMusic;

        private int score;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            world = new World();
            world.Gravity = Vector2.Zero;

            var top = 0;
            var bottom = Constants.GAME_HEIGHT;
            var left = 0;
            var right = Constants.GAME_WIDTH;
            var edges = new Body[]
            {
                world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                world.CreateEdge(new Vector2( left,top), new Vector2(left, bottom)),
                world.CreateEdge(new Vector2( left, bottom), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(right,top), new Vector2(right, bottom))
            };
            foreach(var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1.0f);
            }

            System.Random random = new System.Random();
            farmers = new List<FarmerSprite>();
            for(int i = 0; i < 5; i++)
            {
                var radius = 25;
                var position = new Vector2(
                    random.Next(radius, Constants.GAME_WIDTH - radius),
                    random.Next(radius, Constants.GAME_HEIGHT - radius)
                    );
                var body = world.CreateCircle(radius, 1f, position, BodyType.Dynamic);

                body.LinearVelocity = new Vector2(
                    random.Next(-50, 50),
                    random.Next(-50, 50));
                body.SetRestitution(1.0f);
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                farmers.Add(new FarmerSprite(radius, body));
            }
            var playerRadius = 32;
            var playBody = world.CreateCircle(playerRadius, 1f, new Vector2(200,200), BodyType.Dynamic);
           
            player = new PlayerSprite(playerRadius, playBody);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            // TODO: use this.Content to load your game content here
            foreach(var farmer in farmers) farmer.LoadContent(Content);
            background = Content.Load<Texture2D>("Background");
            bounce = Content.Load<SoundEffect>("Hit_hurt18");
            backgroundMusic = Content.Load<Song>("bensound-moose");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            font = Content.Load<SpriteFont>("menufont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);
            player.color = Color.White;
            foreach (var farmer in farmers) farmer.Update(gameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            score = player.score;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            string Sscore = "Score = " + score.ToString();
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            foreach(var farmer in farmers) farmer.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font,Sscore ,new Vector2(20, 20), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
