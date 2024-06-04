using MAUI_LDDP.Services;
using MAUI_LDDP.Helpers;

using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Firebase.Auth;

namespace MAUI_LDDP.Pages;

public partial class Page_Connexion : ContentPage
{
	private readonly FirebaseAuthService _authService;

	FirestoreDb database;

	public Page_Connexion()
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


		// Read the JSON file from the app package
		using (var stream = await FileSystem.OpenAppPackageFileAsync("ionicapp.json"))
		{
			// Define a local path to save the file
			var localFilePath = Path.Combine(FileSystem.CacheDirectory, "ionicapp.json");

			// Save the stream to the local path
			using (var fileStream = File.Create(localFilePath))
			{
				await stream.CopyToAsync(fileStream);
			} // Ensure the fileStream is closed before using the file

			// Set the environment variable with the local file path
			Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localFilePath);
		}

		// Initialize Firestore with your project ID
		database = FirestoreDb.Create("ionicapp-71182");

		// Create a reference to a collection
		CollectionReference coll = database.Collection("TESTMAUI");

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
		//	await DisplayAlert("Erreur de connexion", "Problème de connexion. Essayez de nouveau.", "OK");
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