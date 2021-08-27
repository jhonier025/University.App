﻿using Xamarin.Forms;
using University.App.ViewModels.Forms;
using University.App.Views.Forms;

namespace University.App.ViewModels
{
    public class MainViewModel
    {
        public CoursesViewModel Courses { get; set; }
        public StudentsViewModel Students{get; set;}
        public CreateCourseViewModel CreateCourse { get; set; }
        public MainViewModel()
        {
            instance = this;
            this.Courses = new CoursesViewModel();
            this.Students = new StudentsViewModel();

            this.CreateCourseCommand = new Command(GoToCreateCourse);

        }

        #region Commands
        
          public Command CreateCourseCommand { get; set; }
        #endregion

        #region Methods
        async void GoToCreateCourse()
        {
            GetInstance().CreateCourse = new CreateCourseViewModel();
            await Application.Current.MainPage.Navigation.PushAsync( new CreateCoursePage());
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance() 
        {
            if (instance == null)
                return new MainViewModel();
            return instance;
        
        }
        #endregion
    }

}