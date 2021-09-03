using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            MainPage = new NavigationPage(new CreateOfficePage ());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
