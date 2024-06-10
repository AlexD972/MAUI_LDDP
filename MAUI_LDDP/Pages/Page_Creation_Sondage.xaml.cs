using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Creation_Sondage : ContentPage
{
	private readonly FirestoreDb _database;

	private string _pollName;
	public Page_Creation_Sondage(FirestoreDb database, string pollName)
	{
		InitializeComponent();

		_database = database;

		_pollName = pollName;
		PollNameLabel.Text += _pollName;
	}

	private async void OnConfirmClicked(object sender, EventArgs e)
	{
		// Créer une référence à la collection journee
		CollectionReference coll = _database.Collection("Journee");

		// Créer un document avec des données
		Dictionary<string, object> sondage = new Dictionary<string, object>
		{
			{ "Createur", "" },
			{ "DateJ", PollDatePicker.Date.ToUniversalTime() },
			{ "Fini", "" },
			{ "NameJ", _pollName },
			{ "Token", "" }
		};

		// Ajoute le document à la collection journee
		await coll.AddAsync(sondage);

		await Navigation.PopAsync();  // Retour à la page précédente
	}
}