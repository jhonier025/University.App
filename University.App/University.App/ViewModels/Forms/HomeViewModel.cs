using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
   public class HomeViewModel : BaseViewModel
    {

        public Command GetCourses Command ()

        async void GoTCoursePage()
        {
            MainViewModel.GetInstance = new CoursesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CousesPage());
        }

    }
}
