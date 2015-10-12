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
	public class Player : GameObj
	{
		private int speed;
		private static GraphicsContext graphics;
		public Vector3 pos;
		private Texture2D clawTex= new Texture2D ("/Application/assets/up_present.png", false);
		private Texture2D meowTex = new Texture2D("/Application/assets/left_present.png", false);
		private Texture2D hairballTex = new Texture2D("/Application/assets/right_present.png", false);
		private int coolDown;
		
		private bool canUseClaw;
		private bool canUseMeow;
		private bool canUseHairball;
		
		public Player (GraphicsContext g, Texture2D t, Vector3 p) :base(g,t,p)
		{
			speed =4;
			graphics = g;
			pos = p;
			coolDown = 0;
			canUseClaw = false;
			canUseHairball = false;
			canUseMeow = false;
		}
		
		public override void Update ()
		{

			pos = new Vector3(sprite.Position.X, sprite.Position.Y, 0);
			
			foreach(GameObj g in AppMain.pieces)
			{
				if(g is Star && g.isAlive())
				{
					if(Vector3.Distance (pos, g.Pos) < 30)
					{
						AppMain.gameOver = true;	
					}
				}
			}
			
			
			if(Vector3.Distance (pos, AppMain.hb.Pos) < 30)
			{
				canUseHairball = true;
				AppMain.hb.die();	
			}
			
			if(Vector3.Distance (pos, AppMain.cl.Pos) < 30)
			{
				canUseClaw = true;
				AppMain.cl.die();	
			}
			
			if(Vector3.Distance (pos, AppMain.m.Pos) < 30)
			{
				canUseMeow = true;
				AppMain.m.die();	
			}
			
			var gamePadData = GamePad.GetData (0);
			if ((gamePadData.Buttons & GamePadButtons.Left) != 0) {
				sprite.Position.X-=speed;
			}
			if ((gamePadData.Buttons & GamePadButtons.Right) != 0) {
				sprite.Position.X+=speed;
			}
			if ((gamePadData.Buttons & GamePadButtons.Up) != 0) {
				sprite.Position.Y-=speed;
			}
			if ((gamePadData.Buttons & GamePadButtons.Down) != 0) {
				sprite.Position.Y+=speed;
			}
			
			if (coolDown <= 0) //to prevent the user from using weapons too often
			{
				if ((gamePadData.Buttons & GamePadButtons.Square) != 0){
					if(canUseClaw)
					{
						Claw c = new Claw(graphics, clawTex, pos);
						c.isActive = true;
						Use (c);
						AppMain.presentSoundPlayer.Play();
					}
				}
				if ((gamePadData.Buttons & GamePadButtons.Triangle) != 0){
					if(canUseMeow)
					{
						SonicMeow m1 = new SonicMeow(graphics, meowTex, pos, new Vector3(-1, 0, 0));
						m1.isActive = true;
						Use (m1);
						AppMain.presentSoundPlayer.Play();
					}
				}
				if ((gamePadData.Buttons & GamePadButtons.Circle) != 0){
					if(canUseHairball)
					{
						HairBall h = new HairBall(graphics, hairballTex, pos);
						h.isActive = true;
						Use (h);
						AppMain.presentSoundPlayer.Play();
					}
				}
				coolDown = 25;
			}
			coolDown--;
		}

		public override void Render()
		{
			sprite.Render();	
		}
		
		public void Use(Weapon w)
		{
			//add to the wait list to render & update
			AppMain.waiting.Add (w);	
		}
	}
}

