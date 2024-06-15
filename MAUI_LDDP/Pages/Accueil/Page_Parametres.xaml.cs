using Google.Cloud.Firestore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using MAUI_LDDP.Pages.Accueil.Parametres;

namespace MAUI_LDDP.Pages.Accueil;

public partial class Page_Parametres : ContentPage
{
	public Page_Parametres()
	{
		InitializeComponent();
	}

	private void Button_Accueil_Clicked(object sender, EventArgs e)
	{
		var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();
		Navigation.PushAsync(new Page_Accueil(database), false);
	}

	private void Button_A_propos_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_A_propos(), false);
	}
}