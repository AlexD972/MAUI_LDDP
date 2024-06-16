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
		sondage_info = new Sondage_class
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

	private void Button_Resultats_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Resultats(database, sondage_info), false);
	}

	private void Button_Back_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Accueil(database), false);
	}

	private void Button_Create_Clicked(object sender, EventArgs e)
	{
		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous créer une proposition ?", "Oui", "Non");
			if (result)
			{
				string propName = await this.DisplayPromptAsync("Nom de la proposition sondage", "Veuillez entrer le nom de la proposition", "OK", "Annuler");

				// Création d'une référence à la collection Proposition
				CollectionReference coll = _database.Collection("Proposition");

				// Récupération de tous les documents avec le même token
				QuerySnapshot snapshot = await coll.WhereEqualTo("Token", Token).GetSnapshotAsync();

				// On compte le nombre de ces documents et on ajoute 1 pour obtenir le prochain numéro
				int nextNumber = snapshot.Count + 1;

				// Création d'un document avec des données
				Dictionary<string, object> proposition = new Dictionary<string, object>
				{
					{ "IdP", nextNumber},
					{ "NameP", propName},
					{ "Pourcentage", 0},
					{ "Token", Token }
				};

				// Création un nouvel ID pour le document
				string newId = Token + "_" + nextNumber;

				// Ajoute le document avec le nouvel ID à la collection Proposition
				DocumentReference docRef = coll.Document(newId);
				await docRef.SetAsync(proposition);
				Button_Refresh_Clicked(null, null);
			}
		});
		Button_Refresh_Clicked(null, null);
	}

	private void Button_Delete_Clicked(object sender, EventArgs e)
	{
		//var proposition = (Proposition_class)e.SelectedItem;
		//if (proposition == null)
		//	return;
		var button = (Button)sender;
		var proposition = (Proposition_class)button.CommandParameter;

		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous supprimer la proposition ?", "Oui", "Non");
			if (result)
			{
				// Créer une référence à la collection Proposition
				CollectionReference coll = _database.Collection("Proposition");

				// Créer une référence au document que vous voulez supprimer
				DocumentReference docRef = coll.Document(proposition.Token + "_" + proposition.IdP);

				// Supprimer le document
				await docRef.DeleteAsync();
				Button_Refresh_Clicked(null, null);
			}
		});
		Button_Refresh_Clicked(null, null);
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