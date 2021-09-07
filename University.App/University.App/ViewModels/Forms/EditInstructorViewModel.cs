using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditInstructorViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private InstructorDTO _instructor;
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

        public InstructorDTO Instructor
        {
            get { return this._instructor; }
            set { this.SetValue(ref this._instructor, value); }
        }


        #endregion


        #region Constructor
        public EditInstructorViewModel(InstructorDTO instructor)
        {
            this._apiService = new ApiService();
            this.EditInstructorCommand = new Command(EditInstructor);
            this.IsEnable = true;
            this.Instructor = instructor;

        }
        #endregion


        #region Methods
        async void EditInstructor()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Instructor.LastName) ||

                        this.Instructor.ID == 0 )
                        
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
                var responseDTO = await _apiService.RequestAPI<InstructorDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.PUT_INSTRUCTOR + this.Instructor.ID,
                    this.Instructor,
                    ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

                this.Instructor.ID = this.Instructor.ID = 0;
                this.Instructor.LastName = String.Empty;

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
        public Command EditInstructorCommand { get; set; }
        #endregion
    }
}
