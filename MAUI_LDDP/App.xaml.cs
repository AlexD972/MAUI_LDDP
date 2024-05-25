using MAUI_LDDP.Pages;
namespace MAUI_LDDP
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new Page_Connexion());
		}
	}
}
