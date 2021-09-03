using System;
using System.Collections.Generic;
using System.Text;
using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
   public class HomeViewModel : BaseViewModel
    {
               
            
            #region Constructor
            public HomeViewModel()
            {
                this.GetCoursesCommand = new Command(GoToCoursesPage);
                this.GetStudentsCommand = new Command(GoToStudentsPage);
            }
            #endregion

            #region Commands
            public Command GetCoursesCommand { get; set; }
            public Command GetStudentsCommand { get; set; }
            #endregion

            #region Methods
            async void GoToCoursesPage()
            {
                MainViewModel.GetInstance().Courses = new CoursesViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new CoursesPage());
            }

            async void GoToStudentsPage()
            {

            }
            #endregion
        }



    }
