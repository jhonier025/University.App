using System;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
     public class CreateCourseViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private int _courseID;
        private string _title;
        private int _credits;
        private bool _isEnabled;
        private bool _isRunning;

        #endregion


        #region Properties
        public bool IsEnable
        {
            get { return this._isEnabled; }
            set { this.SetValue(ref this._isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this._isRunning; }
            set { this.SetValue(ref this._isRunning, value); }
        }

        public int CoursesID
        {
            get { return this._courseID; }
            set { this.SetValue(ref this._courseID, value); }
        }

        public string  Title
        {
            get { return this._title; }
            set { this.SetValue(ref this._title, value); }
        }
        public int Credits
        {
            get { return this._credits; }
            set { this.SetValue(ref this._credits, value); }
        }
        #endregion


        #region Constructor
        public CreateCourseViewModel()
        {   
            this._apiService = new ApiService();
            this.CreateCourseCommand = new Command(CreateCourses);
            this.IsEnable = true;
            
        }
        #endregion


        #region Methods
        async void CreateCourses()
        {
            try
              {
                if (string.IsNullOrEmpty(this.Title)  ||

                        this.Credits == 0 ||
                        this.CoursesID == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification",
                        "The fields are required",
                        "Cancel");
                    return;
                
                }

                this.IsEnable = false;
                this.IsRunning = true;

                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsEnable = false;
                    this.IsRunning = true;
                    
                    await Application.Current.MainPage.DisplayAlert("Notification",
                        "No internet connection",
                        "Cancel");
                    return;
                }
                var courseDTO = new CourseDTO
                {
                    CourseID = this.CoursesID,
                    Title = this.Title,
                    Credits = this.Credits

                };
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<CourseDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_COURSES,
                    courseDTO,
                    ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

                this.CoursesID = this.CoursesID = 0;
                this.Title = String.Empty;

                await Application.Current.MainPage.DisplayAlert("Notification",
                    message,
                    "Cancel");

            }
            catch (Exception ex)
            {
                this.IsEnable = false;
                this.IsRunning = true;

                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }
        }
        #endregion

        #region Commands
        public Command CreateCourseCommand { get; set; }
        #endregion

    }
}
