using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace FinalProject
{
    public class StartScene : GameScene
    {
        //Variables
        private SpriteBatch spriteBatch;
        Song background;
        Texture2D _bar, _backGround, kungfu, firstScreen;
        GameManager _gameManager;
        Caracter fighter, enemy;
        bool _IsFighting = false;
        const int _spaceBethirgFighters = 35;
        Song golpe;
        Song golpe1;
        Song win;

        public StartScene(Game1 game) : base(game)
        {
            this.spriteBatch = game.SpriteBatch;

            //Background song
            background = game.Content.Load<Song>("songs/tocarDuranteJogo");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(background);

            //Load Images
            _bar = game.Content.Load<Texture2D>("Image/barra");
            _backGround = game.Content.Load<Texture2D>("Image/bg");
            firstScreen = game.Content.Load<Texture2D>("Image/initialScreen");
            kungfu = game.Content.Load<Texture2D>("Image/kungfu");

            //Set the frames for the player one 
            fighter = new Caracter("goodGuy", 100, 317);
            fighter.AddAction("walk");                     //moviment walk
            fighter.action[0].addFrame(0, 0, 100);      
            fighter.action[0].addFrame(40, 0, 100);     
            fighter.AddAction("jump");                     //moviment jump
            fighter.action[1].addFrame(80, 0, 100);
            fighter.AddAction("kick");                     //moviment kick
            fighter.action[2].addFrame(80, 27 * 3, 50);
            fighter.action[2].addFrame(40, 27 * 3, 100);
            fighter.action[2].addFrame(80, 27 * 3, 50);
            fighter.AddAction("die");                      //moviment die
            fighter.action[3].addFrame(325, 26 * 3, 50);

            //Set the frames for the player two 
            enemy = new Caracter("badGuy", 500, 317);
            enemy.AddAction("walk");                       //moviment walk
            enemy.action[0].addFrame(0, 32 * 4, 100);
            enemy.action[0].addFrame(40, 32 * 4, 100);
            enemy.AddAction("die");                        //moviment die
            enemy.action[1].addFrame(8 * 42, 32 * 4, 100);
            enemy.AddAction("kick");                       //moviment kick
            enemy.action[2].addFrame(260, 32 * 4, 50);
            enemy.action[2].addFrame(300, 32 * 4, 100);
            enemy.action[2].addFrame(260, 32 * 4, 50);
            enemy.AddAction("jump");                       //moviment jump
            enemy.action[3].addFrame(180, 32 * 4, 100);

            //Songs
            golpe = game.Content.Load<Song>("Songs/mulherApanhando2");
            golpe1 = game.Content.Load<Song>("Songs/mulherApanhado1");
            win = game.Content.Load<Song>("Songs/win1");

            //Initialize GameManager
            _gameManager = new GameManager();
        }


        public override void Update(GameTime gameTime)
        {

            //check if it is fighting 
            if (_IsFighting)
            {
                Fighter();
                EnemyFunction();
                CheckSxreenLimits();
                IsGameOver();
            }
            else
            {
                //waiting in the first screen for the user press space
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _IsFighting = true;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(background);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //draw the screens
            GraphicsDevice.Clear(Color.AliceBlue);
            if (_IsFighting)
            {
                _gameManager.AddBackground(spriteBatch, _backGround);
                _gameManager.AddCharacter(spriteBatch, enemy, kungfu);
                _gameManager.AddCharacter(spriteBatch, fighter, kungfu);
                _gameManager.AddLifeBar(spriteBatch, _bar, 100, fighter.lifePoints);
                _gameManager.AddLifeBar(spriteBatch, _bar, 360, enemy.lifePoints);
            }
            else
            {
                _gameManager.AddBackground(spriteBatch, firstScreen);
            }
            base.Draw(gameTime);
        }


        public void Fighter()
        {
            double delayTime = (DateTime.Now - fighter.lastUpdate).TotalMilliseconds;

            if (fighter.delay != fighter.GetActionIndex("walk"))
            {

                if (fighter.delay == fighter.GetActionIndex("jump"))
                {
                    JumpSequence(fighter);
                }
                if (fighter.delay == fighter.GetActionIndex("kick"))
                {
                    //colision
                    if (fighter.currenFrame == 1)
                    {
                        if (enemy.positionX - fighter.positionX <= _spaceBethirgFighters + 10)
                        {
                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(golpe);
                            enemy.lifePoints -= 5;
                            if (enemy.lifePoints <= 0)
                            {
                                enemy.lifePoints = 0;
                                Dead(enemy);
                            }
                            else
                            {
                                //enemy screen effect
                                if (enemy.shiftCounter <= 10)
                                {
                                    enemy.positionX += enemy.shift;
                                    CheckXPosition(enemy);
                                    enemy.shift *= -1;
                                    enemy.shiftCounter++;
                                }
                                else
                                    enemy.shiftCounter = 0;
                            }
                        }
                    }

                    if ((int)delayTime > fighter.action[fighter.delay].frames[fighter.currenFrame].time)
                    {
                        KickSequency(fighter);
                    }
                }
            }
            else
            {
                if ((int)delayTime > fighter.action[fighter.GetActionIndex("walk")].frames[fighter.currenFrame].time)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                            Jump(fighter, 1);
                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                            Jump(fighter, -1);
                        if (!Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
                            Jump(fighter, 0);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Kick(fighter);
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                            GoodGuyWalkMoviment(1);

                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                            GoodGuyWalkMoviment(-1);
                    }
                }
            }

        }

        // logic for the BadGuy (System) 
        public void EnemyFunction()
        {
            if (enemy.lifePoints <= 0)
                return;

            double delayTime = (DateTime.Now - enemy.lastUpdate).TotalMilliseconds;
            Random rnd = new Random();

            if (enemy.delay != enemy.GetActionIndex("walk")) //walking
            {
                if (enemy.delay == enemy.GetActionIndex("jump")) //jumping
                    JumpSequence(enemy);
                if (enemy.delay == enemy.GetActionIndex("kick")) //kicking
                {
                    
                    if (enemy.currenFrame == 1)
                    {
                        if (enemy.positionX - fighter.positionX < _spaceBethirgFighters + 20) //kicked!
                        {

                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(golpe1);
                            fighter.lifePoints -= 5;
                            if (fighter.lifePoints <= 0)
                            {
                                fighter.lifePoints = 0;
                                Dead(fighter);
                            }
                            else
                            {
                                //bad guy effects
                                if (fighter.shiftCounter <= 10)
                                {
                                    fighter.positionX += enemy.shift;
                                    CheckXPosition(fighter);
                                    fighter.shift *= -1;
                                    fighter.shiftCounter++;
                                }
                                else
                                    fighter.shiftCounter = 0;
                            }
                        }
                    }

                    if ((int)delayTime > enemy.action[enemy.delay].frames[enemy.currenFrame].time)
                    {
                        KickSequency(enemy);
                    }
                }
            }
            else
            {
                if ((int)delayTime > enemy.action[0].frames[enemy.currenFrame].time)
                {
                    if (rnd.NextDouble() <= 0.03)
                    {
                        if (rnd.NextDouble() <= 0.01)
                            Jump(enemy, 1);
                        else if (rnd.NextDouble() >= 0.02)
                            Jump(enemy, -1);
                        else
                            Jump(enemy, 0);
                    }
                    else if ((enemy.positionX - fighter.positionX < 150) && (rnd.NextDouble() > 0.99))
                    {
                        Kick(enemy);
                    }
                    else
                    {
                        if (enemy.positionX - fighter.positionX > 75)
                        {
                            if (rnd.NextDouble() < 0.5)
                            {
                                EnemyMoviment(-1);
                            }
                            else
                            {
                                EnemyMoviment(1);
                            }
                        }
                    }
                }
            }
        }

        
        void Dead(Caracter p)
        {
           
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(win);
            p.delay = p.GetActionIndex("die");
            p.currenFrame = 0;
            p.posicaoy = 317;
            p.lastUpdate = DateTime.Now;
        }
        void Kick(Caracter p)
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(golpe1);
            p.delay = p.GetActionIndex("kick");
            p.currentActionLocation = 0;
            p.currenFrame = 0;
            p.lastUpdate = DateTime.Now;
        }
        void KickSequency(Caracter p)
        {
            p.currenFrame++;
            if (p.currenFrame > 2)
            {
                p.delay = p.GetActionIndex("walk");
                p.currentActionLocation = 0;
                p.currenFrame = 0;
            }
            p.lastUpdate = DateTime.Now;
        }
        void Jump(Caracter p, int sentido)
        {
            p.delay = p.GetActionIndex("jump");
            p.currentActionLocation = sentido;
            p.currenFrame = 0;
            p.jump = 6.5;
            p.lastUpdate = DateTime.Now;
        }

        void JumpSequence(Caracter p)
        {
            p.posicaoy -= (int)p.jump;
            if (p.posicaoy >= 317)
            {
                p.posicaoy = 317;
                p.delay = p.GetActionIndex("walk");
                p.currentActionLocation = 0;
                p.currenFrame = 0;
                return;
            }
            p.jump = p.jump - 0.2;
            p.positionX += p.currentActionLocation * 3;
            CheckXPosition(p);
        }
        void EnemyMoviment(int direction)
        {
            if (direction == -1) //left direction
            {
                if (enemy.positionX > fighter.positionX + 30)
                {
                    enemy.positionX = enemy.positionX - 15;
                    CheckXPosition(enemy);
                }
            }
            else //right direction
            {
                if (enemy.positionX < 500)
                {
                    enemy.positionX = enemy.positionX + 15;
                    CheckXPosition(enemy);
                }
            }
            enemy.currenFrame++;
            if (enemy.currenFrame > 1)
                enemy.currenFrame = enemy.frame_inicial;
            enemy.lastUpdate = DateTime.Now;
        }

        //Function for Good Guy Walking
        void GoodGuyWalkMoviment(int direction)
        {
            if (direction == 1)
            {
                fighter.positionX += 15;
                CheckXPosition(fighter);
            }
            else
            {
                fighter.positionX -= 15;
                CheckXPosition(fighter);
            }
            fighter.currenFrame++;
            if (fighter.currenFrame > 1)
            {
                fighter.currenFrame = fighter.frame_inicial;
            }
            fighter.lastUpdate = DateTime.Now;
        }

        void CheckXPosition(Caracter p)
        {
            //Bad guy moving
            if (p == enemy)
            {
                if (enemy.positionX < fighter.positionX + _spaceBethirgFighters)
                {
                    enemy.positionX = fighter.positionX + _spaceBethirgFighters;
                }
            }
            //Good guy moving
            if (p == fighter)
            {
                if (fighter.positionX > enemy.positionX - _spaceBethirgFighters)
                {
                    fighter.positionX = enemy.positionX - _spaceBethirgFighters;
                }
            }
        }

        void ReStartGame()
        {
            fighter.positionX = 100;
            fighter.posicaoy = 317;
            fighter.frame_inicial = 0;
            fighter.currenFrame = 0;
            fighter.shift = 15;
            fighter.shiftCounter = 0;
            fighter.delay = 0;
            fighter.currentActionLocation = 0; //-1 = esq, 0 = neutro; 1 = dir (utilizado no salto)
            fighter.lifePoints = 100;
            fighter.jump = 0;

            enemy.positionX = 500;
            enemy.posicaoy = 317;
            enemy.frame_inicial = 0;
            enemy.currenFrame = 0;
            enemy.shift = 15;
            enemy.shiftCounter = 0;
            enemy.delay = 0;
            enemy.currentActionLocation = 0; //-1 = esq, 0 = neutro; 1 = dir (utilizado no salto)
            enemy.lifePoints = 100;
            enemy.jump = 0;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(background);

            _IsFighting = false;
        }
        void IsGameOver()
        {
            if (fighter.delay == fighter.GetActionIndex("die"))
            {
                double elapsed_time_sec = (DateTime.Now - fighter.lastUpdate).TotalSeconds;
                if (elapsed_time_sec >= 3)
                {
                    ReStartGame();

                }
            }
            else if (enemy.delay == enemy.GetActionIndex("die"))
            {
                double elapsed_time_sec = (DateTime.Now - enemy.lastUpdate).TotalSeconds;
                if (elapsed_time_sec >= 3)
                {
                    ReStartGame();
                }
            }
        }
        void CheckSxreenLimits()
        {
            if (fighter.positionX < 5)
                fighter.positionX = 5;
            if (enemy.positionX > 500)
                enemy.positionX = 500;
        }
    }
}



