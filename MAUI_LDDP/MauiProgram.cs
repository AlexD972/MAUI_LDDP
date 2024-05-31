using Microsoft.Extensions.Logging;
using Camera.MAUI;
using Firebase.Auth.Repository;
using Firebase.Auth;
using MAUI_LDDP.Services;
using MAUI_LDDP.Helpers;


namespace MAUI_LDDP
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();

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

			var config = new FirebaseAuthConfig
			{
				ApiKey = "AIzaSyD7q-kS4aVMTfy0UDFsLvSIl5_iJGMnMzc",
				AuthDomain = "ionicapp-71182.firebaseapp.com",
				Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[]
				{
					new Firebase.Auth.Providers.GoogleProvider().AddScopes("email"),
					new Firebase.Auth.Providers.EmailProvider()
				},
				UserRepository = new FileUserRepository("FirebaseSample")
			};

			var client = new Firebase.Auth.FirebaseAuthClient(config);
			builder.Services.AddSingleton(client);
			builder.Services.AddSingleton<FirebaseAuthService>();

			var app = builder.Build();
			ServiceHelper.ServiceProvider = app.Services;
			return app;
		}
	}
}
