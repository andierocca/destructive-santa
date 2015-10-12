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
	/*Stars are enemies.
	 * They do not move. They act more as obstacles.
	 */	
	
	public class Star: GameObj
	{
		private static Random gen= new Random();
		private static GraphicsContext graphics;
		private static Player player;	
		private Vector3 vel;
		
		public Star (GraphicsContext g, Texture2D t, Vector3 p, Player pl) :base(g,t,p)
		{
			graphics=g;
			player = pl;
			vel= (new Vector3(.5f*(float)gen.Next(1,5),.7f*(float)gen.Next(1,5),0));
		}
		
		public override void Update(){
			if(Vector3.Distance(player.pos, this.Pos) < 100)
			{
				double angle = Math.Atan2(player.Pos.Y - sprite.Position.Y, player.Pos.X - sprite.Position.X);		
				vel = new Vector3((float)Math.Cos (angle), (float)Math.Sin (angle), 0);
				avoidNeighbors();
				sprite.Position+=vel;
			}
		}
		
		public override void Render()
		{
			sprite.Render();	
		}
		
		public void avoidNeighbors()
		{
			Vector3 avoidanceVector = new Vector3 (0, 0, 0);
			int nearNeighborCount = 0;
			Vector3 oldVel = vel;
			foreach (GameObj z in AppMain.pieces)
			{
				if(z is Meteor)
					{
					if((z != this) && (Vector3.Distance (z.Pos, this.Pos) < 100)){
						nearNeighborCount++;
						avoidanceVector += Vector3.Subtract (Pos, z.Pos) * 10.0f / Vector3.Distance (z.Pos, this.Pos);
					}
				}
				if (nearNeighborCount > 0) 
					{
					vel = oldVel * 0.8f + avoidanceVector.Normalize () * 0.2f;
					}
					else 
					{
					vel = oldVel; 
				}
			  }
		}
	}
}

