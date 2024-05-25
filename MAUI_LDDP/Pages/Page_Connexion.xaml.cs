//using FirebaseAdmin.Auth;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Connexion : ContentPage
{
	public Page_Connexion()
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
	private void Button_Mdp_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Mdp_1(), false);
	}
	private void Button_Connexion_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Accueil(), false);
		//var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
		//Application.Current.MainPage = new NavigationPage(new Page_Accueil());
		//var result = await SignInWithEmailPassword(emailEntry.Text, passwordEntry.Text);
		//await DisplayAlert("Sign In", result, "OK");
	}

	//public async Task<string> SignInWithEmailPassword(string email, string password)
	//{
	//	try
	//	{
	//		auth.
	//		var user = await auth.SignInWithEmailAndPasswordAsync(email, password);
	//		return $"User signed in: {user.User.Uid}";
	//	}
	//	catch (FirebaseAuthException ex)
	//	{
	//		return $"Error: {ex.Reason}";
	//	}
	//}

	protected override bool OnBackButtonPressed()
	{
		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous vraiment quitter l'application ?", "Oui", "Non");
			if (result)
			{
				Environment.Exit(0);
			}
		});
		return true;
	}
}