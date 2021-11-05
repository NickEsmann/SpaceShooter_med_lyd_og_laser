using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter
{
    class Enemy : GameObject
    {
        private Random random;
        private SoundEffectInstance effect;
        
        public Enemy()
        {
            random = new Random();
            offset = Vector2.Zero;
            color = Color.White;
        }

        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[4];

            sprites[0] = content.Load<Texture2D>("enemyBlack1");
            sprites[1] = content.Load<Texture2D>("enemyBlue1");
            sprites[2] = content.Load<Texture2D>("enemyGreen1");
            sprites[3] = content.Load<Texture2D>("enemyRed1");

            effect = content.Load<SoundEffect>("SFX_Powerup_01").CreateInstance();

            Respawn();
            
        }

        public override void Update(GameTime gametime)
        {
            Move(gametime);

            if (position.Y > GameWorld.Screensize.Y)
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            int index = random.Next(0, 4);
            sprite = sprites[index];

            velocity = new Vector2(0, 1);
            speed = random.Next(10, 100);
            position.X = random.Next(0, (int)GameWorld.Screensize.X - sprite.Width);
            position.Y = 0;

            effect.Play();

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Laser)
            {
                GameWorld.Destroy(other);
                Respawn();
            }
        }
    }
}
