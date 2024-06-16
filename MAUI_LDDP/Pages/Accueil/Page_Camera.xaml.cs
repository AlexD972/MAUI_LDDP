using Camera.MAUI;
using Google.Cloud.Firestore;
using MAUI_LDDP.Services;

namespace MAUI_LDDP.Pages.Accueil;

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
			string token = await this.DisplayPromptAsync("Ajouter un sondage", "Veuillez entrer le code d'acc�s du sondage", "OK", "Annuler");

			// Cr�e une r�f�rence � la collection Journee
			CollectionReference journeeColl = _database.Collection("Journee");

			// V�rifie si le token existe dans la collection Journee
			QuerySnapshot snapshot = await journeeColl.WhereEqualTo("Token", token).GetSnapshotAsync();

			if (snapshot.Count > 0)
			{
				// Cr�e une r�f�rence � la collection User
				CollectionReference userColl = _database.Collection("User");

				// Cr�e un document avec des donn�es
				Dictionary<string, object> sondage = new Dictionary<string, object>
				{
					{ "IdUser", GlobalUID.UserUID },
					{ "Token", token }
				};

				// Ajoute le document � la collection User
				DocumentReference docRef = await userColl.AddAsync(sondage);
			}
			else
			{
				var result = await this.DisplayAlert("Erreur", "Ce sondage n'existe pas, voulez vous r�essayer", "Oui", "Non");
				if (result)
				{
					Button_Add_Clicked(null, null);
				}
			}
		});
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Accueil(database), false);
	}
}