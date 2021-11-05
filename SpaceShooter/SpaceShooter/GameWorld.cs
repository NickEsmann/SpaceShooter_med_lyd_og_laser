using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;
        private static Vector2 screensize;
        private Texture2D collisionTexture;
        private Song backgroundMusic;

        public static Vector2 Screensize { get => screensize; set => screensize = value; }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screensize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


        }

        protected override void Initialize()
        {
            gameObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            deleteObjects = new List<GameObject>();
            gameObjects.Add(new Enemy());
            gameObjects.Add(new Enemy());
            gameObjects.Add(new Enemy());
            gameObjects.Add(new Player());            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("CollisionTexture");
            backgroundMusic = Content.Load<Song>("BlindShift");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(this.Content);
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameObjects.AddRange(newObjects);
            newObjects.Clear();

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);

                foreach (GameObject other in gameObjects)
                {
                    go.CheckCollision(other);
                }
            }

            foreach (GameObject go in deleteObjects)
            {
                gameObjects.Remove(go);
            }
            deleteObjects.Clear();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);

                #if DEBUG
                DrawCollisionBox(go);
                #endif

            }

            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        private void DrawCollisionBox(GameObject go)
        {
            
            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);

            spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

        public static void Instantiate(GameObject go)
        {
            newObjects.Add(go);
        }

        public static void Destroy(GameObject go)
        {
            deleteObjects.Add(go);
        }
    }
}
