using MAUI_LDDP.Pages;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;

namespace MAUI_LDDP
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var database = MauiProgram.CreateMauiApp().Services.GetRequiredService<FirestoreDb>();

			MainPage = new NavigationPage(new Page_Connexion(database));
		}
	}
}
