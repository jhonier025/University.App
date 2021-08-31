using System;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditCourseViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private CourseDTO _course;
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

        public CourseDTO Course
        {
            get { return this._course; }
            set { this.SetValue(ref this._course, value); }
        }

       
        #endregion


        #region Constructor
        public EditCourseViewModel(CourseDTO course)
        {
            this._apiService = new ApiService();
            this.EditCourseCommand = new Command(EditCourses);
            this.IsEnable = true;
            this.Course = course;

        }
        #endregion


        #region Methods
        async void EditCourses()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Course.Title) ||

                        this.Course.Credits == 0 ||
                        this.Course.CourseID == 0)
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
                
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<CourseDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.PUT_COURSES + this.Course.CourseID,
                    this.Course,
                    ApiService.Method.Put);

                if (responseDTO.Code >= 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

                this.Course.CourseID = this.Course.CourseID = 0;
                this.Course.Title = String.Empty;

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
        public Command EditCourseCommand { get; set; }
        #endregion
    }
}
