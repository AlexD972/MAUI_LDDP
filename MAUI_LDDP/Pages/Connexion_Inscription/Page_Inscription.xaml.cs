using MAUI_LDDP.Services;
using MAUI_LDDP.Helpers;
using MAUI_LDDP.Pages.Accueil;
using Google.Cloud.Firestore;

namespace MAUI_LDDP.Pages.Connexion_Inscription;

public partial class Page_Inscription : ContentPage
{
	private readonly FirebaseAuthService _authService;
	public Page_Inscription()
	{
		InitializeComponent();
		_authService = ServiceHelper.ServiceProvider.GetService<FirebaseAuthService>();

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

	private async void Button_Inscription_Clicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text;
		var password = PasswordEntry.Text;
		var result = await _authService.CreateUserWithEmailAndPasswordAsync(email, password);
		//await DisplayAlert("Login Result", result, "OK");

		if (!string.IsNullOrWhiteSpace(result) && !result.Contains(" "))
		{
			var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
			await Navigation.PushAsync(new Page_Accueil(database));
		}
		else
		{
			await DisplayAlert("Erreur d'inscription", "Le mot de passe doit contenir 6 caractères au moins. Essayez de nouveau.", "OK");
		}
	}

	private void Button_Connexion_Clicked(object sender, EventArgs e)
	{
		var database = ServiceHelper.ServiceProvider.GetService<FirestoreDb>();

		Navigation.PushAsync(new Page_Connexion(database), false);
	}
}