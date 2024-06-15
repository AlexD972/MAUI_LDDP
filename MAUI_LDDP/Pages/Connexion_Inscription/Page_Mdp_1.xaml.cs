using Google.Cloud.Firestore;

namespace MAUI_LDDP.Pages.Connexion_Inscription;

public partial class Page_Mdp_1 : ContentPage
{
	public Page_Mdp_1()
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
	private void Button_Inscription_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Inscription(), false);
	}

	private void Button_Connexion_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();

		Navigation.PushAsync(new Page_Connexion(database), false);
	}

	private void Button_Reinitialiser_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Mdp_2(), false);
	}
	
}