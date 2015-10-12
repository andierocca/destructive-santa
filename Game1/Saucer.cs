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
	/*Saucers extend meteors.
	 * They inherit the avoid neighbors method. 
	 * However, they target the player.
	 */
	
	public class Saucer: Meteor
	{
		private Player player;
		private Vector3 vel;
		private static GraphicsContext graphics;
		private static Random gen= new Random();
			
		public Saucer(GraphicsContext g, Texture2D t, Vector3 p, Player pl) :base(g,t,p, pl)
		{
			player = pl;
			vel= (new Vector3(.5f*(float)gen.Next(3,5),.7f*(float)gen.Next(3,5),0));
			graphics=g;
		}
			
		public override void Update()
		{		
			avoidNeighbors();
			sprite.Position+=vel;
			if(sprite.Position.X<0 || sprite.Position.X> graphics.Screen.Width-64)
				vel.X*=-1;
			if(sprite.Position.Y<0 || sprite.Position.Y> graphics.Screen.Height-64)
				vel.Y*=-1;
			
			if(Vector3.Distance(player.pos, this.Pos) < 100)
			{
				double angle = Math.Atan2(player.Pos.Y - sprite.Position.Y, player.Pos.X - sprite.Position.X);		
				vel = new Vector3((float)Math.Cos (angle), (float)Math.Sin (angle), 0);
				base.avoidNeighbors();
				sprite.Position+=vel;
			}
		}
	}
}

