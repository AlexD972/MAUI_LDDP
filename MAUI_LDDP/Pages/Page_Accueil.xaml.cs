using Google.Cloud.Firestore;
using MAUI_LDDP.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Accueil : ContentPage
{
	private readonly FirestoreDb _database;

	public Page_Accueil(FirestoreDb database)
	{
		_database = database;
		InitializeComponent();
		//Affichage de la liste des sondages dès l'ouverture de la page
		Button_Refresh_Clicked(null, null);
	}

	private void Button_Parametres_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Parametres(), false);

	}

	private void Button_Create_Clicked(object sender, EventArgs e)
	{
		
		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous créer un sondage ?", "Oui", "Non");
			if (result)
			{
				string pollName = await this.DisplayPromptAsync("Nom du sondage", "Veuillez entrer le nom du sondage", "OK", "Annuler");
				// Naviguez vers la nouvelle page pour choisir la date
				var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();

				await Navigation.PushAsync(new Page_Creation_Sondage(database, pollName), false);
			}
		});
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Accueil(database), false);
	}

	private void Button_Camera_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Camera(), false);
	}

	private async void Button_Refresh_Clicked(object sender, EventArgs e)
	{
		CollectionReference collRef = _database.Collection("Journee");
		QuerySnapshot snapshot = await collRef.GetSnapshotAsync();

		List<Sondage> sondages = new List<Sondage>();
		foreach (DocumentSnapshot document in snapshot.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["Createur"] as string == GlobalUID.UserUID)
				{
					Sondage sondage = new Sondage
					{
						Createur = data["Createur"] as string,
						DateJ = data["DateJ"] is DateTime dateTime ? dateTime : DateTime.MinValue,
						Fini = data["Fini"] is bool fini ? fini : false,
						NameJ = data["NameJ"] as string,
						Token = data["Token"] as string
					};

					sondages.Add(sondage);
				}
			}
		}
		Liste_Sondage.ItemsSource = sondages;

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