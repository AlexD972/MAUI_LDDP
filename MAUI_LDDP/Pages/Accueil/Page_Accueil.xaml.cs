using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using MAUI_LDDP.Services;
using MAUI_LDDP.Pages.Sondage;

namespace MAUI_LDDP.Pages.Accueil;

public partial class Page_Accueil : ContentPage
{
	private readonly FirestoreDb _database;

	//Constructeur de la page d'accueil
	public Page_Accueil(FirestoreDb database)
	{
		_database = database;
		InitializeComponent();
		//Affichage de la liste des sondages d�s l'ouverture de la page
		Button_Refresh_Clicked(null, null);
	}

	//Navigue vers la page des param�tres du compte
	private void Button_Parametres_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Parametres(), false);

	}

	//Navigue vers la page de cr�ation de sondage
	private void Button_Create_Clicked(object sender, EventArgs e)
	{
		
		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous cr�er un sondage ?", "Oui", "Non");
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

	//Navigue vers la page de la cam�ra
	private void Button_Camera_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Camera(database), false);
	}

	//R�cup�re les sondages cr��s par l'utilisateur et ceux auxquels il a �t� invit�
	private async void Button_Refresh_Clicked(object sender, EventArgs e)
	{
		CollectionReference journee_coll = _database.Collection("Journee");
		QuerySnapshot snapshot_journee = await journee_coll.GetSnapshotAsync();

		CollectionReference user_coll = _database.Collection("User");
		QuerySnapshot snapshot_user = await user_coll.GetSnapshotAsync();

		List<Sondage_class> sondages = new List<Sondage_class>();

		// Parcours des documents de la collection Journee afin d'ajouter les sondages cr��s par l'utilisateur
		foreach (DocumentSnapshot document in snapshot_journee.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["Createur"] as string == GlobalUID.UserUID)
				{
					Sondage_class sondage = new Sondage_class
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
		// Parcours des documents de la collection User afin d'ajouter les sondages auxquels l'utilisateur a �t� invit�
		foreach (DocumentSnapshot document in snapshot_user.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["IdUser"] as string == GlobalUID.UserUID)
				{
					string token = data["Token"] as string;

					// R�cup�rez le document du sondage � partir de la collection "Journee" en utilisant le token
					DocumentReference docRef = _database.Collection("Journee").Document(token);
					DocumentSnapshot snapshot_journee2 = await docRef.GetSnapshotAsync();

					if (snapshot_journee2.Exists)
					{
						Dictionary<string, object> data_journee = snapshot_journee2.ToDictionary();

						Sondage_class sondage = new Sondage_class
						{
							Createur = data_journee["Createur"] as string,
							DateJ = data_journee["DateJ"] is Timestamp timestamp ? timestamp.ToDateTime() : DateTime.MinValue,
							Fini = data_journee["Fini"] is bool fini ? fini : false,
							NameJ = data_journee["NameJ"] as string,
							Token = data_journee["Token"] as string
						};
						//V�rification afin d'�viter les doublons
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

	//Enl�ve la s�lection de l'�l�ment apr�s un appui, une couleur orange d�sagr�able apparaissait sinon
	private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		var sondage = (Sondage_class)e.SelectedItem;
		if (sondage == null)
			return;

		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();

		// Naviguer vers une nouvelle page
		Navigation.PushAsync(new Page_Sondage(database, sondage));

		// D�s�lectionner l'�l�ment
		Liste_Sondage.SelectedItem = null;

		//Ancien code
		//if (e.SelectedItem == null)
		//	return;

		//// D�s�l�ctionne l'�l�ment
		//((ListView)sender).SelectedItem = null;
	}


	// M�thode pour quitter l'application en appuyant sur le bouton retour
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