using utilities.Platforms.Windows;
namespace utilities;

public partial class MainPage : ContentPage
{
	private readonly List<char> _characters =  ['á', 'é', 'í', 'ó', 'ú', 'ü', 'ñ','Á','É','Í','Ó', 'Ú','Ü','Ñ','`'];
	public MainPage()
	{
		InitializeComponent();
		CreateButtons();
	}
	public async void CopyBtn(object? sender, EventArgs e)
	{
		if (sender is Button b)
		{
			var text = b.Text ?? "";
			await Clipboard.SetTextAsync(text);
		}
	}
	public void OnButtonCLick(object? sender, EventArgs e)
	{
		if(sender is Button button)
		{
			double end=1;
			double start=0.9;
			button.ScaleX = start;
			button.ScaleY = start;
			var animation = new Animation(v=>
				{
					button.ScaleX = v;
					button.ScaleY = v;
				},
				start,
				end
			);
			//refactor animation using statics and extended elements
			animation.Commit(button, "Button_Click_Animation");
		}
	}
	//should be in a different class
	public void CreateButtons()
	{
		int count = 1;
		int row=0,col=0; 
		foreach (var character in _characters)
		{
			var btn = new Button{
				Text = character.ToString() 
			};
			btn.Clicked += CopyBtn;//added using handler
			btn.Clicked+= OnButtonCLick;
			ButtonsLayout.Add(btn, col, row);
			//change after added btn element to grid because context not null
			btn.HandlerChanged += (_, __) =>
			{
				var ctx = btn.Handler?.MauiContext;
				if (ctx is not null)
					btn.SetCustomCursor(CursorIcon.Hand, ctx);
			};
			col++;
			if (count % 3 == 0)
			{
				col=0;
				row++;
			}
			count++;
		}   
	}
}

