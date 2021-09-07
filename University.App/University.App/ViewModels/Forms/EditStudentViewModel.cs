using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditStudentViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private StudentDTO _student;
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

        public StudentDTO Student
        {
            get { return this._student; }
            set { this.SetValue(ref this._student, value); }
        }


        #endregion

        #region Constructor
        public EditStudentViewModel(StudentDTO student)
        {
            this._apiService = new ApiService();
            this.EditStudentCommand = new Command(EditStudent);
            this.IsEnable = true;
            this.Student = student;

        }
        #endregion

        #region Methods
        async void EditStudent()
        {
            try
            {
                if (this.Student.ID == 0 )
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

                this.Student.FullName = null;
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<StudentDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.PUT_STUDENTS + this.Student.ID,
                    this.Student,
                    ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

               

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
        public Command EditStudentCommand { get; set; }
        #endregion
    }
}
