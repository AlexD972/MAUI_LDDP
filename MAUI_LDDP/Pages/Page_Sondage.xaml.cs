using Google.Cloud.Firestore;
using MAUI_LDDP.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Sondage : ContentPage
{
	private readonly FirestoreDb _database;

	public Page_Sondage(FirestoreDb database)
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
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Camera(database), false);
	}

	private async void Button_Refresh_Clicked(object sender, EventArgs e)
	{
		CollectionReference journee_coll = _database.Collection("Journee");
		QuerySnapshot snapshot_journee = await journee_coll.GetSnapshotAsync();

		CollectionReference user_coll = _database.Collection("User");
		QuerySnapshot snapshot_user = await user_coll.GetSnapshotAsync();

		List<Sondage> sondages = new List<Sondage>();

		// Parcours des documents de la collection Journee afin d'ajouter les sondages créés par l'utilisateur
		foreach (DocumentSnapshot document in snapshot_journee.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["Createur"] as string == GlobalUID.UserUID)
				{
					Sondage sondage = new Sondage
					{
						Createur = data["Createur"] as string,
						DateJ = data["DateJ"] is Timestamp timestamp ? timestamp.ToDateTime() : DateTime.MinValue,
						Fini = data["Fini"] is bool fini ? fini : false,
						NameJ = data["NameJ"] as string,
						Token = data["Token"] as string,
						IsCurrentUserCreator = GlobalUID.UserUID == (data["Createur"] as string)
					};

					sondages.Add(sondage);
				}
			}
		}
		// Parcours des documents de la collection User afin d'ajouter les sondages auxquels l'utilisateur a été invité
		foreach (DocumentSnapshot document in snapshot_user.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["IdUser"] as string == GlobalUID.UserUID)
				{
					string token = data["Token"] as string;

					// Récupérez le document du sondage à partir de la collection "Journee" en utilisant le token
					DocumentReference docRef = _database.Collection("Journee").Document(token);
					DocumentSnapshot snapshot_journee2 = await docRef.GetSnapshotAsync();

					if (snapshot_journee2.Exists)
					{
						Dictionary<string, object> data_journee = snapshot_journee2.ToDictionary();

						Sondage sondage = new Sondage
						{
							Createur = data_journee["Createur"] as string,
							DateJ = data_journee["DateJ"] is Timestamp timestamp ? timestamp.ToDateTime() : DateTime.MinValue,
							Fini = data_journee["Fini"] is bool fini ? fini : false,
							NameJ = data_journee["NameJ"] as string,
							Token = data_journee["Token"] as string
						};
						//Vérification afin d'éviter les doublons
						if (!sondages.Exists(s => s.Token == sondage.Token))
						{
							sondages.Add(sondage);
						}
					}
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