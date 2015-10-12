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
	public class HairBall: Weapon
	{	
		private static GraphicsContext graphics;
		private Vector3 vel;
		
		public HairBall(GraphicsContext g, Texture2D t, Vector3 p) :base(g,t,p)
		{
			graphics = g;
			vel= (new Vector3(1, 0, 0));
			sprite.Position = p;
		}
		
		public override void Update ()
		{
			if(this.isActive)
			{
				sprite.Position += vel;
				
				if(sprite.Position.X<0 || sprite.Position.X> graphics.Screen.Width-64)
					this.die ();
				if(sprite.Position.Y<0 || sprite.Position.Y> graphics.Screen.Height-64)
					this.die ();
				
				killEnemies ();
			}
		}
	}
}

