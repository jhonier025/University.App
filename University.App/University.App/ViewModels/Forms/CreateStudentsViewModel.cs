using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateStudentsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string lastName;
        private string firstMidName;
        private DateTime enrollmentDate;
        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public string LastName
        {
            get { return this.lastName; }
            set { this.SetValue(ref this.lastName, value); }
        }

        public string FirstMidName
        {
            get { return this.firstMidName; }
            set { this.SetValue(ref this.firstMidName, value); }
        }

        public DateTime EnrollmentDate
        {
            get { return this.enrollmentDate; }
            set { this.SetValue(ref this.enrollmentDate, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        #endregion


        #region Constructor
        public CreateStudentsViewModel()
        {
            this._apiService = new ApiService();
            this.IsEnabled = true;
            this.EnrollmentDate = DateTime.UtcNow;

            this.CreateStudentCommand = new Command(CreateStudent);
        }

        #endregion

        #region Methods
        async void CreateStudent(object obj)
        {
            try
            {

                this.IsEnabled = false;
                this.IsEnabled = true;

                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;

                    await Application.Current.MainPage.DisplayAlert("Notificacion",
                        "No internet connection",
                        "Cancel");

                    return;
                }


                var studentDTO = new StudentDTO
                {
                    LastName = this.LastName,
                    FirstMidName = this.FirstMidName,
                    EnrollmentDate = this.EnrollmentDate
                };


                var responseDTO = await _apiService.RequestAPI<StudentDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                   Endpoints.POST_STUDENTS,
                   studentDTO,
                   ApiService.Method.Post);

                await Application.Current.MainPage.DisplayAlert("Notification",
                    "The Process is successful",
                    "Cancel");

                this.LastName = this.FirstMidName = string.Empty;
                this.EnrollmentDate = DateTime.UtcNow;

                this.IsEnabled = true;
                this.IsRunning = false;

            }
            catch (Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;

                await Application.Current.MainPage.DisplayAlert("Notificacion", ex.Message, "Cancel");
            }
        }

        #endregion

        #region Commands
        public Command CreateStudentCommand { get; set; }

        #endregion

    }
}
