using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using MAUI_LDDP.Services;
using MAUI_LDDP.Pages.Accueil;

namespace MAUI_LDDP.Pages.Sondage;

public partial class Page_Resultats : ContentPage
{
	private readonly FirestoreDb _database;
	private Sondage_class sondage_info;

	public string NameJ { get; private set; }
	public string Token { get; private set; }

	public Page_Resultats(FirestoreDb database, Sondage_class sondage_token)
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
		NameJ = "Résultats " + sondage_token.NameJ;
		Token = sondage_token.Token;
		//Affichage des résultats dès l'ouverture de la page
		Button_Refresh_Clicked(null, null);

		//Liaison des données de la page avec les données du sondage
		this.BindingContext = this;
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
						Pourcentage = Convert.ToInt32(data["Pourcentage"]),
						Token = data["Token"] as string
					};

					propositions.Add(proposition);
				}
			}
		}
		// On trie les propositions par pourcentage de manière décroissante
		propositions = propositions.OrderByDescending(p => p.Pourcentage).ToList();

		Liste_Proposition.ItemsSource = propositions;

		// ActivityIndicator inactif
		loadingIndicator.IsRunning = false;
		loadingIndicator.IsVisible = false;
	}

	private void Button_Parametres_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Parametres(), false);
	}

	private void Button_Sondage_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		bool isCurrentUserCreator = GlobalUID.UserUID == sondage_info.Createur;

		Navigation.PushAsync(new Page_Sondage(database, sondage_info, isCurrentUserCreator));
	}
}