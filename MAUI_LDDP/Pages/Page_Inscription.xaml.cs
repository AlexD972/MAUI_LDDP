namespace MAUI_LDDP.Pages;

public partial class Page_Inscription : ContentPage
{
	public Page_Inscription()
	{
		InitializeComponent();
		Background = new LinearGradientBrush
		{
			StartPoint = new Point(0.5, 0),
			EndPoint = new Point(0.5, 1),
			GradientStops = new GradientStopCollection
				{
					new GradientStop(Color.FromArgb("#26CCE5"), 0.0f),
					new GradientStop(Color.FromArgb("#39C1EF"), 0.5f),
					new GradientStop(Color.FromArgb("#372A65"), 1.0f)
				}
		};
	}

	private void Button_Connexion_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Connexion(), false);
	}
}