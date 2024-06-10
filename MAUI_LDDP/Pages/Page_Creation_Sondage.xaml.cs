using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MAUI_LDDP.Pages;

public partial class Page_Creation_Sondage : ContentPage
{
	private string _pollName;
	public Page_Creation_Sondage(string pollName)
	{
		InitializeComponent();
		_pollName = pollName;
		PollNameLabel.Text += _pollName;
	}

	private async void OnDateSelected(object sender, DateChangedEventArgs e)
	{
		// Utilisez _pollName et e.NewDate comme vous le souhaitez
		await Navigation.PopAsync();  // Retour à la page précédente
	}
}