using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Reflection.Metadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace FinalProject
{
	public class GameManager
	{
        //Add background
        public void AddBackground(SpriteBatch spriteBatch, Texture2D backGround)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, new Rectangle(0, 0, 640, 480),Color.White);
            spriteBatch.End();
        }

        //Add character
        public void AddCharacter(SpriteBatch spriteBatch, Caracter c, Texture2D texture)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(c.positionX, c.posicaoy, 100, 100), new Rectangle( c.action[c.delay].frames[c.currenFrame].set_x, c.action[c.delay].frames[c.currenFrame].set_y, 39, 39),Color.White);
            spriteBatch.End();
        }

        //Add Life Bar
        public void AddLifeBar(SpriteBatch spriteBatch, Texture2D bar, int x, int lifeScore)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bar, new Rectangle(x, 62, (int)(lifeScore * 1.8), 13),Color.White);
            spriteBatch.End();
        }
    }


}

