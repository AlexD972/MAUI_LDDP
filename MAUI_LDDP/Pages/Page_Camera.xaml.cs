using Camera.MAUI;
using Google.Cloud.Firestore;
using MAUI_LDDP.Services;

namespace MAUI_LDDP.Pages;

public partial class Page_Camera : ContentPage
{
	private readonly FirestoreDb _database;

	public Page_Camera(FirestoreDb database)
	{
		InitializeComponent();
		_database = database;
	}

	private void cameraView_CamerasLoaded(object sender, EventArgs e)
	{
		if (cameraView.Cameras.Count > 0)
		{
			cameraView.Camera = cameraView.Cameras.First();
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await cameraView.StopCameraAsync();
				await cameraView.StartCameraAsync();
			});
		}
	}

	private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
		});
	}

	private void Button_Add_Clicked(object sender, EventArgs e)
	{

		Dispatcher.Dispatch(async () =>
		{
			string token = await this.DisplayPromptAsync("Ajouter un sondage", "Veuillez entrer le code d'accès du sondage", "OK", "Annuler");
			// Créer une référence à la collection User
			CollectionReference coll = _database.Collection("User");

			// Créer un document avec des données
			Dictionary<string, object> sondage = new Dictionary<string, object>
		{
			{ "IdUser", GlobalUID.UserUID },
			{ "Token", token }
		};

			// Ajoute le document à la collection journee
			DocumentReference docRef = await coll.AddAsync(sondage);
		});
		//Navigation.PushAsync(new Page_Accueil(), false);
	}
}