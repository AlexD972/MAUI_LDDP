using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using System.Reflection;
using Camera.MAUI;

namespace MAUI_LDDP
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();

			//FirebaseApp.Create(new AppOptions()
			//{
			//	Credential = GoogleCredential.FromFile("Resources/Raw/google-services.json"),
			//});

			//FirebaseApp.Create(new AppOptions()
			//{
			//	Credential = GoogleCredential.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Test_Dev_Mob_1.google-services.json"))
			//});

			builder
				.UseMauiApp<App>()
				.UseMauiCameraView()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("Ribeye-Regular.ttf", "RibeyeRegular");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
