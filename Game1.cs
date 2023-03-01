using FinalProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MenuScene _menuScene;
        private HelpScene _helpScene;
        private CreditScene _creditScene;
        private StartScene _startScene;

        public SpriteBatch SpriteBatch { get => _spriteBatch; set => _spriteBatch = value; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
       
        private void hideAllScenes()
        {
            foreach (GameScene item in Components)
                item.hide();

        }
        protected override void Initialize()
        {
            
            SharedVars.stage = new Vector2(_graphics.PreferredBackBufferWidth = 635, _graphics.PreferredBackBufferHeight = 475);
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

           //Menu Components
            _startScene = new StartScene(this);
            this.Components.Add(_startScene);
            _menuScene = new MenuScene(this);
            this.Components.Add(_menuScene);
            _helpScene = new HelpScene(this);
            this.Components.Add(_helpScene);
            _creditScene = new CreditScene(this);
            this.Components.Add(_creditScene);
            hideAllScenes();
            _menuScene.show();


        }


        protected override void Update(GameTime gameTime)
        {
            
            KeyboardState ks = Keyboard.GetState();
            int index = 0;
            if (_menuScene.Enabled)
            {
               
                index = _menuScene.MenuComponent._SelectedIndex;
                if (index == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAllScenes();
                    Exit();
                }
                else if (index == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    _menuScene.hide();
                    _startScene.show();
                }

                else if (index == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    _menuScene.hide();
                    _helpScene.show();
                }

                else if (index == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    _menuScene.hide();
                    _creditScene.show();
                }
            }

            else if (ks.IsKeyDown(Keys.Escape))
            {
              
                _startScene.hide();
                _creditScene.hide();
                _helpScene.hide();
                _menuScene.show();
                
            }              

            base.Update(gameTime);
        }

        //Draw the Menu
       protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightPink);       
            base.Draw(gameTime);
        }
    }
}