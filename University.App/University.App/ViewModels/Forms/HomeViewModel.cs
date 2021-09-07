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
            this.GetInstructorCommand = new Command(GoToInstructorPage);
            this.GetDepartmentCommand = new Command(GoToDepartmentPage);
            this.GetOfficeCommand = new Command(GoToOfficePage);
        }
        #endregion

        #region Commands
        public Command GetCoursesCommand { get; set; }
        public Command GetStudentsCommand { get; set; }
        public Command GetInstructorCommand { get; set; }
        public Command GetDepartmentCommand { get; set; }
        public Command GetOfficeCommand { get; set; }

        #endregion

        #region Methods
        async void GoToCoursesPage()
        {
            MainViewModel.GetInstance().Courses = new CoursesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CoursesPage());
        }

        async void GoToStudentsPage()
        {
            MainViewModel.GetInstance().Students = new StudentsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new StudentsPage());

        }

        async void GoToInstructorPage()
        {
            MainViewModel.GetInstance().Instructors = new InstructorsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InstructorPage());
        }

        async void GoToDepartmentPage()
        {
            MainViewModel.GetInstance().Departments = new DepartmentsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new DepartmentPage());

        }

        async void GoToOfficePage()
        {
            MainViewModel.GetInstance().Office = new OfficeViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new OfficePage());

        }
        #endregion

    }
}