using Camera.MAUI;

namespace MAUI_LDDP.Pages;

public partial class Page_Camera : ContentPage
{
	public Page_Camera()
	{
		InitializeComponent();
	}

	private void cameraView_CamerasLoaded(object sender, EventArgs e)
	{
		if (cameraView.Cameras.Count > 0)
		{
			cameraView.Camera = cameraView.Cameras.First();
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await cameraView.StopCameraAsync();
				await cameraView.StartCameraAsync();
			});
		}
	}

	private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
		});
	}

	private void Button_Add_Clicked(object sender, EventArgs e)
	{

		Dispatcher.Dispatch(async () =>
		{
			var result = await this.DisplayAlert("Confirmation", "Voulez-vous ajouter un sondage avec un code ?", "Oui", "Non");
			if (result)
			{
				//Navigation.PushAsync(new Page_Parametres(), false);
			}
		});
		//Navigation.PushAsync(new Page_Accueil(), false);
	}
}