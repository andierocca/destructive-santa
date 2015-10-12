using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace Game1
{
	public class MenuDisplay
	{
		private GraphicsContext graphics;
		private Label announcement;
		private Label announcement2;
		private Label announcement3;
		private Label announcement4;
		
		public string Announcement
		{
			get { return announcement.Text; }
			set { announcement.Text = value; }
		}
		public string Announcement2
		{
			get { return announcement2.Text; }
			set { announcement2.Text = value; }
		}
		public string Announcement3
		{
			get { return announcement3.Text; }
			set { announcement3.Text = value; }
		}
		public string Announcement4
		{
			get { return announcement4.Text; }
			set { announcement4.Text = value; }
		}
		
		public MenuDisplay (GraphicsContext graphicsContext)
		{
			graphics = graphicsContext;
			UISystem.Initialize(graphics);
			Scene scene = new Scene();
			
			announcement = new Label();
			announcement.X = 0;
			announcement.Y = graphics.Screen.Rectangle.Height / 2 - announcement.TextHeight / 2 - 200;
			announcement.Width = graphics.Screen.Rectangle.Width;
			announcement.HorizontalAlignment = HorizontalAlignment.Center;
			announcement.Text = "TBA";
			scene.RootWidget.AddChildLast(announcement);
			
			announcement2 = new Label();
			announcement2.X = 0;
			announcement2.Y = graphics.Screen.Rectangle.Height / 2 - announcement2.TextHeight / 2;
			announcement2.Width = graphics.Screen.Rectangle.Width;
			announcement2.HorizontalAlignment = HorizontalAlignment.Center;
			announcement2.Text = "TBA";
			scene.RootWidget.AddChildLast(announcement2);
			
			announcement3 = new Label();
			announcement3.X = 0;
			announcement3.Y = graphics.Screen.Rectangle.Height / 2 - announcement3.TextHeight / 2 + 50;
			announcement3.Width = graphics.Screen.Rectangle.Width;
			announcement3.HorizontalAlignment = HorizontalAlignment.Center;
			announcement3.Text = "TBA";
			scene.RootWidget.AddChildLast(announcement3);
			
			announcement4 = new Label();
			announcement4.X = 0;
			announcement4.Y = graphics.Screen.Rectangle.Height / 2 - announcement4.TextHeight / 2 + 100;
			announcement4.Width = graphics.Screen.Rectangle.Width;
			announcement4.HorizontalAlignment = HorizontalAlignment.Center;
			announcement4.Text = "TBA";
			scene.RootWidget.AddChildLast(announcement4);
			
			UISystem.SetScene(scene, null);
		}
		
		public void Render()
		{
			UISystem.Render();
		}
	}
}

