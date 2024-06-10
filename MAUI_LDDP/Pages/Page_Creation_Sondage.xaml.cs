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
		// Cr�er une r�f�rence � la collection journee
		CollectionReference coll = _database.Collection("Journee");

		// Cr�er un document avec des donn�es
		Dictionary<string, object> sondage = new Dictionary<string, object>
		{
			{ "Createur", "" },
			{ "DateJ", PollDatePicker.Date.ToUniversalTime() },
			{ "Fini", "" },
			{ "NameJ", _pollName },
			{ "Token", "" }
		};

		// Ajoute le document � la collection journee
		await coll.AddAsync(sondage);

		await Navigation.PopAsync();  // Retour � la page pr�c�dente
	}
}