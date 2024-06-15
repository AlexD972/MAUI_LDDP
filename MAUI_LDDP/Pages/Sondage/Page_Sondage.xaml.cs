using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using MAUI_LDDP.Services;
using MAUI_LDDP.Pages.Accueil;

namespace MAUI_LDDP.Pages.Sondage;

public partial class Page_Sondage : ContentPage
{
	private readonly FirestoreDb _database;
	private Sondage_class sondage_info;

	private bool _isCurrentUserCreator;

	public string NameJ { get; private set; }
	public string Token { get; private set; }

	public Page_Sondage(FirestoreDb database, Sondage_class sondage_token, bool isCurrentUserCreator)
	{
		_database = database;
		InitializeComponent();
		//Récupération des informations du sondage
		Sondage_class sondage_info = new Sondage_class
		{
			Createur = sondage_token.Createur,
			DateJ = sondage_token.DateJ,
			Fini = sondage_token.Fini,
			NameJ = sondage_token.NameJ,
			Token = sondage_token.Token
		};
		NameJ = sondage_token.NameJ;
		Token = sondage_token.Token;
		//Affichage de la liste des propositions dès l'ouverture de la page
		Button_Refresh_Clicked(null, null);
		_isCurrentUserCreator = isCurrentUserCreator;

		//Liaison des données de la page avec les données du sondage
		this.BindingContext = this;
	}

	private void Button_Parametres_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Parametres(), false);
	}

	private void Button_Create_Clicked(object sender, EventArgs e)
	{
		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous créer une proposition ?", "Oui", "Non");
			if (result)
			{
				string pollName = await this.DisplayPromptAsync("Nom du sondage", "Veuillez entrer le nom de la proposition", "OK", "Annuler");
				// Naviguez vers la nouvelle page pour choisir la date
				var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();

				//REMPLACER PAR CREATION PROPOSITION
				//await Navigation.PushAsync(new Page_Creation_Sondage(database, pollName), false);
			}
		});
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>(); 
		Navigation.PushAsync(new Page_Sondage(database, sondage_info, _isCurrentUserCreator), false);
	}

	//Navigue vers la page de la caméra
	private void Button_Camera_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Camera(database), false);
	}

	//Récupère les propositions du sondage
	private async void Button_Refresh_Clicked(object sender, EventArgs e)
	{
		// ActivityIndicator actif
		loadingIndicator.IsRunning = true;
		loadingIndicator.IsVisible = true;

		CollectionReference proposition_coll = _database.Collection("Proposition");
		QuerySnapshot snapshot_proposition = await proposition_coll.GetSnapshotAsync();

		List<Proposition_class> propositions = new List<Proposition_class>();

		// Parcours des documents de la collection Proposition afin d'afficher les propositions du sondage selectionné
		foreach (DocumentSnapshot document in snapshot_proposition.Documents)
		{
			if (document.Exists)
			{
				Dictionary<string, object> data = document.ToDictionary();

				if (data["Token"] as string == Token)
				{
					Proposition_class proposition = new Proposition_class
					{
						IdP = Convert.ToInt32(data["IdP"]),
						NameP = data["NameP"] as string,
						Token = data["Token"] as string
					};

					propositions.Add(proposition);
				}
			}
		}
		Liste_Proposition.ItemsSource = propositions;

		// ActivityIndicator inactif
		loadingIndicator.IsRunning = false;
		loadingIndicator.IsVisible = false;
	}
}