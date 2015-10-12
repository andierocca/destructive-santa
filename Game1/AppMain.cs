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
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Imaging;
using System.IO;
using Sce.PlayStation.HighLevel.UI;
using System.Diagnostics;

namespace Game1
{
	public class AppMain
	{
		/*In this game, the player is Santa who tries to 
		 * destroy cars, houses, and police with presents
		 * 
		 * D to shoot presents right
		 * A to shoot presents up
		 * W to shoot presents up
		 * Arrows to move
		 */
		
		private static GraphicsContext graphics;
		public static List<GameObj> pieces;
		private static Sprite go, bg, alpha, selector, elf1, elf2, tree;
		private static Random gen;
		
		public static List<Weapon> waiting; //list of weapons waiting to render
		
		public static Claw cl;
		public static HairBall hb;
		public static SonicMeow m;
		private static Player p;
		
		public static bool gameOver;
		
		private static Texture2D gameOverTex, alphabetTex, selectorTex;
		
		private static String holder; //high score holder
		public static int score, highscore;
		private static Label scoreLabel, hsLabel;
		
		private static Texture2D elf1Tex, elf2Tex, treeTex;
		
		private enum GameState {Menu, Playing, Paused, Dead, Quit, HighScoreView, HighScoreAdd};
		private static GameState currentGameState;
		private static bool isPlaying;
		
		private static MenuDisplay menuDisplay;
		
		private static Stopwatch clock;
		private static long startTime;
		private static long endTime;
		private static long timeDelta;
		
		public static bool newLevel;
		
		private static int coolDown;
		
		private static Bgm backgroundMusic;
		public static Sound presentSound;
		private static BgmPlayer backgroundPlayer;
		public static SoundPlayer presentSoundPlayer;
		
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (isPlaying) {
				startTime = clock.ElapsedMilliseconds;
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
				endTime = clock.ElapsedMilliseconds;
				timeDelta = endTime - startTime;
			}
		}

		public static void Initialize ()
		{
			
			clock = new Stopwatch();
			clock.Start();
			
			backgroundMusic = new Bgm("/Application/assets/music.mp3");
			backgroundPlayer = backgroundMusic.CreatePlayer();
			backgroundPlayer.Loop = true;
			backgroundPlayer.Play();
			
			presentSound = new Sound("/Application/assets/jinglebell.wav");
			presentSoundPlayer = presentSound.CreatePlayer();
			
			isPlaying = true;
			
			// Set up the graphics system
			graphics = new GraphicsContext ();
			gen= new Random();
			
			NewGame(0);

			currentGameState = GameState.Menu;
			menuDisplay = new MenuDisplay(graphics);
		}
		
		public static void NewGame(int sc)
		{
			gameOver = false;
			newLevel = true;
			score = sc;
			
			elf1Tex = new Texture2D("/Application/assets/elf1.png", false);
			elf2Tex = new Texture2D("/Application/assets/elf2.png", false);
			treeTex = new Texture2D("/Application/assets/tree.png", false);
			Texture2D bgTex = new Texture2D ("/Application/assets/snow.png", false);
			Texture2D catTex = new Texture2D ("/Application/assets/santa.png", false);
			Texture2D starTex= new Texture2D ("/Application/assets/house.png", false);
			Texture2D meteorTex= new Texture2D ("/Application/assets/car.png", false);
			Texture2D saucerTex= new Texture2D ("/Application/assets/police.png", false);
			Texture2D clawTex= new Texture2D ("/Application/assets/up_present.png", false);
			Texture2D meowTex = new Texture2D("/Application/assets/left_present.png", false);
			Texture2D hairballTex = new Texture2D("/Application/assets/right_present.png", false);
			gameOverTex = new Texture2D("/Application/assets/game_over.png", false);
			alphabetTex = new Texture2D("/Application/assets/alphabet.png", false);
			selectorTex = new Texture2D("/Application/assets/selector.png", false);
			
			alpha = new Sprite(graphics, alphabetTex);
			alpha.Position = new Vector3((graphics.Screen.Rectangle.Width / 2 - alpha.Width / 2)+340, 
			                          graphics.Screen.Rectangle.Height / 2 - alpha.Height / 2, 0);
			selector = new Sprite(graphics, selectorTex);
			selector.Position = new Vector3((graphics.Screen.Rectangle.Width / 2 - selector.Width / 2) - 175, 
			                          graphics.Screen.Rectangle.Height / 2 - selector.Height / 2, 0);
			
			go = new Sprite (graphics, gameOverTex);
			bg = new Sprite (graphics, bgTex);
			pieces = new List<GameObj> ();
			waiting = new List<Weapon> (); //weapons waiting to be rendered
			p = new Player (graphics, catTex, new Vector3 (30, 450, 0));
			pieces.Add (p);
			
			//add the enemies
			for( int i=0; i<3;i++){
				pieces.Add (new Star (graphics, starTex, new Vector3 (gen.Next(200,900),gen.Next(200,400),0), p));
				pieces.Add (new Star (graphics, starTex, new Vector3 (gen.Next(200,900),gen.Next(200,400),0), p));	
				pieces.Add (new Meteor (graphics, meteorTex, new Vector3(gen.Next(200,900),gen.Next(200,400),0), p));								
			}
			pieces.Add (new Saucer (graphics, saucerTex, new Vector3(gen.Next(200,900),gen.Next(200,400),0), p));
			
			//add weapons to the screne
			cl = new Claw(graphics, clawTex, new Vector3(gen.Next(200,900),gen.Next(200,400),0));
			cl.isActive = false;
			pieces.Add (cl);
			
			m = new SonicMeow(graphics, meowTex, new Vector3(gen.Next(200,900),gen.Next(200,400),0), new Vector3(0,0,0));
			m.isActive = false;
			pieces.Add (m);
			
			hb = new HairBall(graphics, hairballTex, new Vector3(gen.Next(200,900),gen.Next(200,400),0));
			hb.isActive = false;
			pieces.Add (hb);
			
			//to display the score
			UISystem.Initialize (graphics);
			Scene scene = new Scene ();
			
			scoreLabel = new Label (); //current score
			scoreLabel.X = 320;
			scoreLabel.Y = 10;
			scoreLabel.Width = 300;
			scoreLabel.Text = "Score: " + score;
			scene.RootWidget.AddChildLast (scoreLabel);
			
			LoadHighScore ();
			hsLabel = new Label (); //high score
			hsLabel.X = 620;
			hsLabel.Y = 10;
			hsLabel.Width = 300;
			hsLabel.Text = "High Score: " + highscore;
			scene.RootWidget.AddChildLast (hsLabel);
			
			UISystem.SetScene (scene, null);
		}

		public static void Update ()
		{
			switch(currentGameState)
			{
			case GameState.Dead : UpdateDead(); break;
			case GameState.Menu : UpdateMenu(); break;
			case GameState.Playing : UpdatePlaying (); break;
			case GameState.HighScoreView : UpdateHighScoreView(); break;
			case GameState.HighScoreAdd : UpdateHighScoreAdd(); break;
			case GameState.Quit : UpdateQuit(); break;
			}
		}
		
		public static void UpdateMenu()
		{
			var gamePadData = GamePad.GetData(0);
			
			if((gamePadData.Buttons & GamePadButtons.Enter) != 0)
			{
				NewGame(0);
				currentGameState = GameState.Playing;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Select) != 0)
			{
				menuDisplay = new MenuDisplay(graphics);
				currentGameState = GameState.HighScoreView;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Back) != 0)
			{
				currentGameState = GameState.Quit;
			}
		}
		
		public static void UpdateHighScoreView()
		{
			var gamePadData = GamePad.GetData(0);
			
			if((gamePadData.Buttons & GamePadButtons.Enter) != 0)
			{
				menuDisplay = new MenuDisplay(graphics);
				currentGameState = GameState.Menu;
			}
		}
		
		public static void UpdateHighScoreAdd()
		{
			var gamePadData = GamePad.GetData(0);
			DrawHUD ();
			
			
			
			float pos = selector.Position.X;
			
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				if(pos <= 781)
				{
					selector.Position.X += 2.65f;
				}
			}
			
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				
				if(pos >= 252.4)
				{
					selector.Position.X -= 2.65f;
				}
			}
			
			if((gamePadData.Buttons & GamePadButtons.Enter) != 0)
			{
				if (coolDown <= 0) {
					if(holder.Length < 3)
					{
						holder += GetLetterAt(selector.Position.X);
						coolDown = 10;
					}
					else
					{
						UpdateHighScore();	
					}
				}
				
			}
			
			if((gamePadData.Buttons & GamePadButtons.Select) != 0) 
			{
				UpdateHighScore();
				menuDisplay = new MenuDisplay(graphics);
				currentGameState = GameState.Menu;
				
			}
			
			if((gamePadData.Buttons & GamePadButtons.Back) != 0)
			{
				UpdateHighScore();
				currentGameState = GameState.Quit;
			}
			coolDown--;
			
		}
		
		public static char GetLetterAt(float pos)
		{
			bool addOne = false;
			if (pos >= 440.5)
			{
				addOne = true;	
			}
			char[] alphabet = new char[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
			int index = (int)((pos - 252.2)/21.2);
			
			if(addOne)
				return alphabet[index +1];
			
			return alphabet[index];
		}
		
		public static void UpdateQuit()
		{
			isPlaying = false;
		}
		
		public static void UpdateDead()
		{
			var gamePadData = GamePad.GetData(0);
			
			if((gamePadData.Buttons & GamePadButtons.Enter) != 0)
			{
				menuDisplay = new MenuDisplay(graphics);
				currentGameState = GameState.Menu;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Back) != 0)
			{
				currentGameState = GameState.Quit;
			}
		}
		
		public static void UpdatePlaying()
		{
			var gamePadData = GamePad.GetData(0);
			
			
			foreach (GameObj g in pieces)
				if(g.isAlive ()) //If an object has been killed, it should not update.
				{
					g.Update ();
				}
			scoreLabel.Text = "Score: " + score;
			
			//Allows the player to restart.
			if(gameOver == true)
			{
				menuDisplay = new MenuDisplay(graphics);
				currentGameState = GameState.Dead;
			}
			
			if(score % 10 == 0 && !newLevel)
			{
				NewGame (score);
			}
		}

		public static void Render ()
		{
			switch(currentGameState)
			{
			case GameState.Dead : RenderDead(); break;
			case GameState.Menu : RenderMenu(); break;
			case GameState.Paused : RenderPaused(); break;
			case GameState.Playing : RenderPlaying(); break;
			case GameState.HighScoreView : RenderHighScoreView(); break;
			case GameState.HighScoreAdd : RenderHighScoreAdd(); break;
			}
		}
		
		public static void RenderHighScoreView()
		{
			graphics.SetClearColor (0.3f, 0.5f, 0.7f, 0.0f);
			graphics.Clear ();
			
			tree = new Sprite(graphics, treeTex);
			tree.Position = new Vector3(50, 200, 0);
			tree.Render ();
			
			menuDisplay.Announcement = null;
			menuDisplay.Announcement2 = holder + ": " + highscore;
			menuDisplay.Announcement3 = null;
			menuDisplay.Announcement4 = "Press Enter to restart or Back to quit.";
			menuDisplay.Render();
			
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		public static void RenderHighScoreAdd()
		{
			graphics.SetClearColor (0.3f, 0.5f, 0.7f, 0.0f);
			graphics.Clear ();
			
			
			alpha.Scale = new Vector2(.5f, .5f);
			alpha.Render();
			
			
			selector.Scale = new Vector2(.5f, .5f);
			selector.Render ();
			
			DrawHUD ();

			menuDisplay.Announcement = "Move left and right over your three initials and enter to select.";
			menuDisplay.Announcement2 = null;
			menuDisplay.Announcement3 = holder;
			menuDisplay.Announcement4 = "Press Select to return to the Main Menu and Back to quit.";
			menuDisplay.Render();
					
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		
		public static void RenderDead()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			bg.Render ();
			go.Position = new Vector3(graphics.Screen.Rectangle.Width / 2 - go.Width / 2, 
			                          graphics.Screen.Rectangle.Height / 2 - go.Height / 2, 0);
			go.Render();
			DrawHUD();
			
			if(score >= highscore)
			{
				holder = "";
				highscore = score;
				currentGameState = GameState.HighScoreAdd;					
			}
			
			else
			{
				menuDisplay.Announcement = null;
				menuDisplay.Announcement2 = null;
				menuDisplay.Announcement3 = null;
				menuDisplay.Announcement4 = "Press Enter to return to the Main Menu or the Back button to quit.";
				menuDisplay.Render();
				
				// Present the screen
				graphics.SwapBuffers ();
			}
		}
		
		public static void RenderMenu()
		{
			// Clear the screen
			graphics.SetClearColor (0.6f, 0.1f, 0.1f, 0.0f);
			graphics.Clear ();
			
			elf1 = new Sprite(graphics, elf1Tex);
			elf1.Position = new Vector3(600, 100, 0);
			
			elf2 = new Sprite(graphics, elf2Tex);
			elf2.Position = new Vector3(50, 100, 0);
			
			elf2.Render ();
			elf1.Render ();
			
			menuDisplay.Announcement = "Destructive Santa!";
			menuDisplay.Announcement2 = "Press Enter to play the game.";
			menuDisplay.Announcement3 = "Press Select to See the High Score.";
			menuDisplay.Announcement4 = "Press Back to quit the game.";
			menuDisplay.Render();
			
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		public static void RenderPaused()
		{
			//Sets what text will appear on the screen.
			menuDisplay.Announcement = "Paused - Press Start to resume";
			menuDisplay.Announcement2 = null;
			menuDisplay.Announcement3 = null;
			menuDisplay.Announcement4 = "Press Enter to return to the Main Menu.";
			menuDisplay.Render();
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		public static void RenderPlaying()
		{
			if(!gameOver)
			{
				// Clear the screen
				graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
				graphics.Clear ();
				bg.Render ();
				DrawHUD();
				foreach (GameObj g in pieces)
				{
					if(g.isAlive()) //If an object has been killed, it should not render.
					{
						g.sprite.Render ();
					}
				}
				// Present the screen
				graphics.SwapBuffers ();
				
				//add the moving weapons after the other objects are done rendering			
				foreach (Weapon w in waiting)
				{
					pieces.Add (w);
				}
				waiting = new List<Weapon>();
				
			}
		}
		
		public static void DrawHUD()
		{
			UISystem.Render ();
		}
		
		private static void LoadHighScore()
		{
			StreamReader sr = new StreamReader("/Documents/highscore.txt");
			//holder of the current high score
			String read = sr.ReadLine ();
			holder = read.Substring(0,3);
			highscore = Int32.Parse(read.Substring(3));
			sr.Close();
		}
		
		private static void UpdateHighScore()
		{
			StreamWriter sw = new StreamWriter("/Documents/highscore.txt");
			int missingLetters = 3 - holder.Length;
			
			for(int i=0; i<missingLetters; i++)
			{
				holder+= " ";
			}
				
			sw.WriteLine(holder + highscore);
			sw.Close();
		}
	}
}
