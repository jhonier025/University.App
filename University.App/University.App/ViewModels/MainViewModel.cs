using Xamarin.Forms;
using University.App.ViewModels.Forms;
using University.App.Views.Forms;

namespace University.App.ViewModels
{
    public class MainViewModel
    {

        #region Courses
         public CoursesViewModel Courses { get; set; }
         public CreateCourseViewModel CreateCourse { get; set; }
         public EditCourseViewModel EditCourse { get; set; }
        #endregion

        #region Students
        public StudentsViewModel Students{get; set;}
        public CreateStudentsViewModel CreateStudent{ get; set; }
        public EditStudentViewModel EditStudent { get; set; }

        #endregion


        #region Office
         public CreateOfficeViewModel CreateOffice { get; set; }
        #endregion


        #region Home
        public HomeViewModel Home { get; set; }

        #endregion

        #region Departments

        public DepartmentsViewModel Departments { get; set; }
        public EditDepatmentViewModel EditDepartments { get; set; }
        public CreateDepartmentViewModel CreateDepartment { get; set; }
        #endregion


        public MainViewModel()
        {
            instance = this;
            this.Home = new HomeViewModel();
            this.Courses = new CoursesViewModel();
            this.Students = new StudentsViewModel();
            this.CreateOffice = new CreateOfficeViewModel();
            this.Departments = new DepartmentsViewModel();

            this.CreateCourseCommand = new Command(GoToCreateCourse);
            this.CreateStudentCommand = new Command(GoToCreateStudent);
            this.CreateDepartmentCommand = new Command(GoToCreateDepartment); 


        }

        #region Commands
        
          public Command CreateCourseCommand { get; set; }
          public Command CreateStudentCommand { get; set; }
          public Command CreateDepartmentCommand { get; set; }



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

        async void GoToCreateDepartment()
        {
            GetInstance().CreateDepartment = new CreateDepartmentViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CreateDepartmentPage());
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
