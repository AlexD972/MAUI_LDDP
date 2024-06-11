using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Accueil : ContentPage
{

	public Page_Accueil()
	{
		InitializeComponent();
		
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
		Navigation.PushAsync(new Page_Accueil(), false);
	}

	private void Button_Camera_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Camera(), false);
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