using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Parametres : ContentPage
{
	public Page_Parametres()
	{
		InitializeComponent();
	}

	private void Button_Accueil_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Accueil(), false);
	}

	private void Button_Parametres_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Page_Parametres(), false);
	}
}