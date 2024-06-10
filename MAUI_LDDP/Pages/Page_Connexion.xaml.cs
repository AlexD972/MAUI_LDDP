using MAUI_LDDP.Services;
using MAUI_LDDP.Helpers;

using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Firebase.Auth;
using MAUI_LDDP;

namespace MAUI_LDDP.Pages;

public partial class Page_Connexion : ContentPage
{
	private readonly FirebaseAuthService _authService;

	private readonly FirestoreDb _database;

	public Page_Connexion(FirestoreDb database)
	{
		InitializeComponent();

		_authService = ServiceHelper.ServiceProvider.GetService<FirebaseAuthService>();
		_database = database;

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
	private async void Button_Connexion_Clicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text;
		var password = PasswordEntry.Text;

		//A ENLEVER
		if (email == null || password == null)
		{
			await Navigation.PushAsync(new Page_Accueil());
		}

		
		// Create a reference to a collection
		CollectionReference coll = _database.Collection("TESTMAUI");

		// Create a document to add to the collection
		Dictionary<string, object> city = new Dictionary<string, object>
		{
			{ "name", "LA" },
			{ "state", "CA" },
			{ "Country", "USA" }
		};

		// Add the document to the collection asynchronously
		await coll.AddAsync(city);





		////A GARDER
		//var result = await _authService.SignInWithEmailPasswordAsync(email, password);
		////await DisplayAlert("Login Result", result, "OK");

		//if (!string.IsNullOrWhiteSpace(result) && !result.Contains(" "))
		//{
		//	await Navigation.PushAsync(new Page_Accueil());
		//}
		//else
		//{
		//	await DisplayAlert("Erreur de connexion", "Probl�me de connexion. Essayez de nouveau.", "OK");
		//}
	}

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