using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter
{
    class Player : GameObject
    {
        private Vector2 spawnOffset;
        private Texture2D laser;
        private bool canFire;
        private int fireTrigger;
        private SoundEffectInstance effect;
        
        
        public Player()
        {
            speed = 150;
            velocity = Vector2.Zero;
            fps = 10;
            color = Color.White;
            canFire = true;
            fireTrigger = 0;
            spawnOffset = new Vector2(-25, -105);
        }

        public override void Update(GameTime gametime)
        {
            HandleInput();
            Move(gametime);            
            Animate(gametime);
            ScreenWarp();
            ScreenLimits();
        }

        public void HandleInput()
        {
            velocity = Vector2.Zero;
            KeyboardState keyState = Keyboard.GetState();

            if(keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
            }
            if(keyState.IsKeyDown(Keys.Space) & canFire)
            {
                effect.Play();
                canFire = false;
                GameWorld.Instantiate(new Laser(laser, new Vector2(position.X + spawnOffset.X , position.Y + spawnOffset.Y)));
            }

            if (!canFire && fireTrigger < 50)
            {
                fireTrigger++;
            }
            else
            {
                canFire = true;
                fireTrigger = 0;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
        }
               

        public override void LoadContent(ContentManager content)
        {
            effect = content.Load<SoundEffect>("8bit_bomb_explosion").CreateInstance();
            sprites = new Texture2D[4];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>((i + 1) + "fwd");
            }

            sprite = sprites[0];

            this.position = new Vector2(GameWorld.Screensize.X / 2, GameWorld.Screensize.Y - sprite.Height / 2);
            this.origin = new Vector2(sprite.Height / 2, sprite.Width / 2);
            this.offset.X = (-sprite.Width / 2)  - 20;
            this.offset.Y = -sprite.Height / 2;

            laser = content.Load<Texture2D>("laserGreen03");
        }

        private void ScreenWarp()
        {
            if (position.X > GameWorld.Screensize.X + sprite.Width)
            {
                position.X = -sprite.Width;
            }
            else if (position.X < -sprite.Width)
            {
                position.X = GameWorld.Screensize.X + sprite.Width;
            }
        }

        private void ScreenLimits()
        {
            if (position.Y - sprite.Height / 2 < 0)
            {
                position.Y = sprite.Height / 2;
            }
            else if ( position.Y > GameWorld.Screensize.Y)
            {
                position.Y = GameWorld.Screensize.Y;
            }
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Enemy)
            {
                color = Color.Green;
            }
        }
    }
}
