using Xamarin.Forms;
using University.App.ViewModels.Forms;
using University.App.Views.Forms;

namespace University.App.ViewModels
{
    public class MainViewModel
    {
        public CoursesViewModel Courses { get; set; }
        public StudentsViewModel Students{get; set;}
        public CreateCourseViewModel CreateCourse { get; set; }
        public CreateStudentsViewModel CreateStudent{ get; set; }
        public EditCourseViewModel EditCourse { get; set; }
        public EditStudentViewModel EditStudent { get; set; }
        public CreateOfficeViewModel CreateOffice { get; set; }
        public HomeViewModel Home { get; set; }



        public MainViewModel()
        {
            instance = this;
         
            this.Courses = new CoursesViewModel();
            this.Students = new StudentsViewModel();
            this.CreateOffice = new CreateOfficeViewModel();

            this.CreateCourseCommand = new Command(GoToCreateCourse);
            this.CreateStudentCommand = new Command(GoToCreateStudent);


        }

        #region Commands
        
          public Command CreateCourseCommand { get; set; }
          public Command CreateStudentCommand { get; set; }


        #endregion

        #region Methods
        async void GoToCreateCourse()
        {
            GetInstance().CreateCourse = new CreateCourseViewModel();
            await Application.Current.MainPage.Navigation.PushAsync( new CreateCoursePage());
        }

        async void GoToCreateStudent()
        {
            GetInstance().CreateStudent = new CreateStudentsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CreateStudentPagexaml());
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
