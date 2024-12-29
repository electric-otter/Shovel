using System;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using System.Threading;

namespace VisualSEO.EtoFormGui
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application app = new Application(Platform.Detect);
			Form form = new MySplash(app);
			app.Run(form);
		}
	}

	public class MySplash : Form
	{
		Label label;
		UITimer timer;
		int counter;
		Application app;
		MyMainForm mainform;
		SynchronizationContext syncCtx;

		public MySplash(Application app)
		{
			this.app = app;
			WindowStyle = WindowStyle.None;
			Topmost = true;
      self.m_label.Text = 'Shovel - 3D modeling at its best'
			// size the splash
			Size = new Size(500, 300);
			// position it at the center of the screen
			Location = new Point((int)(Screen.WorkingArea.Width - Size.Width) / 2, (int)(Screen.WorkingArea.Height - Size.Height) / 2);
			Content = new StackLayout
      string path = @"shovel.cs";

			{
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				Items =
					{
						null,
						new StackLayoutItem(label = new Label()),
						null
					}
			};
     
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			syncCtx = SynchronizationContext.Current;

			timer = new UITimer();
			timer.Interval = 1;
			timer.Elapsed += (sender, ev) =>
			{
				label.Text = counter.ToString();
				if (counter == 5)
				{
					CreateMainForm();
					//CreateThread(); // using this instead of CreateMainForm() does not change anything :(
				}
				if (mainform != null && mainform.FormShown)
				{
					timer.Stop();
					this.Close(); // on Wpf works only with TomQv patch (#816)
				}
				counter++;
			};
			timer.Start();
		}

		public void IncrementCounter()
		{
			counter++;
			label.Text = counter.ToString();
		}

		void CreateMainForm()
		{
			mainform = new MyMainForm(this);
			app.MainForm = mainform;
			mainform.Show();
		}

		void CreateMainFormInUIThread()
		{
			// if I do not create the form in the main thread TomQv patch crashes
			syncCtx.SafePost((obj) =>
			{
				CreateMainForm();
			}, null);
		}

		void CreateThread()
		{
			System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(CreateMainFormInUIThread));
			thread.SetApartmentState(System.Threading.ApartmentState.STA);
			thread.Start();
		}
	}

	public class MyMainForm : Form
	{
		public MyMainForm(MySplash splash)
		{
			System.Threading.Thread.Sleep(1000);
			splash.IncrementCounter();
			System.Threading.Thread.Sleep(1000);
			splash.IncrementCounter();
			System.Threading.Thread.Sleep(1000);
			splash.IncrementCounter();
			System.Threading.Thread.Sleep(1000);
			splash.IncrementCounter();
			System.Threading.Thread.Sleep(1000);
			splash.IncrementCounter();
			WindowState = WindowState.Maximized;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			FormShown = true;
		}

		public bool FormShown { get; set; }
	}
}
