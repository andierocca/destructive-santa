/*Andie Rocca & Kevin Markley
 * CSE 1302
 * Project 3
 */

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Game1
{
	public abstract class GameObj
	{
		
		private Vector3 pos,vel;
		public Sprite sprite;
		private bool alive;
		
		public GameObj (GraphicsContext g, Texture2D t, Vector3 p)
		{
			sprite= new Sprite(g,t);
			pos=p;
			sprite.Position=pos;
			vel= Vector3.One;
			alive = true;
		}
		
		public abstract void Update();
		
		public abstract void Render();
		
		public Vector3 Pos
		{
			get{ return sprite.Position;}	
		}
		
		public void die()
		{
			alive = false;	
		}
		
		public bool isAlive()
		{
			return alive;
		}
	}
}

