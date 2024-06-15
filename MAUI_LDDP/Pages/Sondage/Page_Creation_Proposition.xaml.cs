using Google.Cloud.Firestore;
using MAUI_LDDP.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages.Sondage;

public partial class Page_Creation_Proposition : ContentPage
{
	private readonly FirestoreDb _database;

	private string _pollName;
	public Page_Creation_Proposition(FirestoreDb database, string pollName)
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
			{ "Createur", GlobalUID.UserUID },
			{ "DateJ", PollDatePicker.Date.ToUniversalTime() },
			{ "Fini", false },
			{ "NameJ", _pollName }
		};

		// Ajoute le document à la collection journee
		DocumentReference docRef = await coll.AddAsync(sondage);

		// Obtient l'ID du document et l'utilise comme token
		string token = docRef.Id;

		// Ajoute le token au document
		await docRef.UpdateAsync("Token", token);

		await Navigation.PopAsync();  // Retour à la page précédente
	}
}