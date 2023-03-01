using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
	public class Caracter
	{
        public string name;
        public DateTime lastUpdate = DateTime.Now;
        public int positionX = 100, posicaoy = 100,lifePoints = 100,shift = 15, shiftCounter = 0;
        public int currenFrame = 0, frame_inicial = 0, delay = 0, currentActionLocation = 0;
        public Action[] action = new Action[0];
        public double jump = 0; 

        //Constructor for the character
        public Caracter(string CaracterName, int x, int y)
        {
            name = CaracterName;
            positionX = x;
            posicaoy = y;
        }
        //Add an Action, here we create and array of action 
        public void AddAction(string name)
        {
            int length = this.action.Length;
            Array.Resize(ref this.action, length + 1);
            this.action[length] = new Action();
            this.action[length].name = name;
        }
        //Here we get the index of the action
        public int GetActionIndex(string nameOfTheAction)
        {
            for (int i = 0; i < this.action.Length; i++)
            {
                if (action[i].name == nameOfTheAction)
                    return i;
            }
            return -1;
        }
    }
    //Here is where we set the action and add it to the Frame 
    public class Action
    {
        public string name;
        public Frame[] frames = new Frame[0];
        public void addFrame(int set_x, int set_y, int time)
        {
            int lenght = this.frames.Length;
            Array.Resize(ref this.frames, lenght + 1);
            this.frames[lenght] = new Frame();
            this.frames[lenght].set_x = set_x;
            this.frames[lenght].set_y = set_y;
            this.frames[lenght].time = time;
        }
    }
    //Frame constructor 
    public class Frame
    {
        public int set_x;
        public int set_y;
        public int time;
    }
}
