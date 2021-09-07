using Android.App;
using Android.OS;
using System.Threading.Tasks;

namespace University.App.Droid
{
    [Activity(MainLauncher = true,
              NoHistory = true,
              Theme = "@style/MyTheme.Splash")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Task.Delay(1800);
            StartActivity(typeof(MainActivity));

        }
    }
}