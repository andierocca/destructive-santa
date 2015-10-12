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
	public class Weapon: GameObj
	{
		/*The player implements objects from this class, Weapon.
		 * Weapons should be able to kill enemies - all objects that are of the star class or its extended classes.
		 */
		
		private static GraphicsContext graphics;
		public bool isActive;
		
		public Weapon(GraphicsContext g, Texture2D t, Vector3 p) :base(g,t,p)
		{
			graphics = g;

		}
		
		public void killEnemies()
		{
			foreach(GameObj other in AppMain.pieces)
			{
				if(this.isActive)
				{
					//if the weapon is touching a star, the star should die along with the weapon.
					if((other is Star) && (other != this) && (Vector3.Distance (other.Pos, sprite.Position) < 30) && other.isAlive())
					{
						other.die ();
						this.die ();
						AppMain.score ++;
						AppMain.newLevel = false;
					}
				}
			}
		}
		
		public override void Update (){}
		
		public override void Render ()
		{
			sprite.Render();
		}
		
		
	}
}

