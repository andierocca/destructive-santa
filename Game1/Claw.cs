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
	/*Claws are objects of the Weapon class.
	 *Represented by a non-moving scratch image on the screen 
	 *They should appear where the player is at the time of the button press and kill any enemies that they touch.  
	 */

	public class Claw: Weapon
	{
		private static GraphicsContext graphics;
		private Vector3 vel;
	
		public Claw(GraphicsContext g, Texture2D t, Vector3 p) :base(g,t,p)
		{
			graphics = g;
			sprite.Position = p;
			vel= (new Vector3(0, -1, 0));
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

