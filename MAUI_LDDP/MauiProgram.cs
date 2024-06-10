using Microsoft.Extensions.Logging;
using Camera.MAUI;
using Firebase.Auth.Repository;
using Firebase.Auth;
using MAUI_LDDP.Services;
using MAUI_LDDP.Helpers;
using Google.Cloud.Firestore;


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

			// Lecture du json de configuration de Firebase
			using (var stream = FileSystem.OpenAppPackageFileAsync("ionicapp.json").GetAwaiter().GetResult())
			{
				// Définit un repertoire local pour le fichier
				var localFilePath = Path.Combine(FileSystem.CacheDirectory, "ionicapp.json");

				// Sauvegarde le fichier dans le repertoire local
				using (var fileStream = File.Create(localFilePath))
				{
					stream.CopyTo(fileStream);
				} 

				// Définit la variable d'environnement pour le fichier de configuration
				Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localFilePath);
			}

			// Initialisation de FireStore avec l'id du projet firestore
			FirestoreDb database = FirestoreDb.Create("ionicapp-71182");

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

			//FireStore
			builder.Services.AddSingleton(database);

			var app = builder.Build();
			ServiceHelper.ServiceProvider = app.Services;
			return app;
		}
	}
}
