using FAuth = Firebase.Auth;

namespace MAUI_LDDP.Services
{
	public class FirebaseAuthService
	{
		private readonly FAuth.FirebaseAuthClient _firebaseAuthClient;

		public FirebaseAuthService(FAuth.FirebaseAuthClient firebaseAuthClient)
		{
			_firebaseAuthClient = firebaseAuthClient;
		}

		public async Task<string> SignInWithEmailPasswordAsync(string email, string password)
		{
			try
			{
				var userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
				return userCredential.User.Uid;
			}
			catch (FAuth.FirebaseAuthException ex)
			{
				return $"Error: {ex.Reason}";
			}
		}

		public async Task<string> RegisterWithEmailPasswordAsync(string email, string password)
		{
			try
			{
				var userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
				return userCredential.User.Uid;
			}
			catch (FAuth.FirebaseAuthException ex)
			{
				return $"Error: {ex.Reason}";
			}
		}
	}
}
