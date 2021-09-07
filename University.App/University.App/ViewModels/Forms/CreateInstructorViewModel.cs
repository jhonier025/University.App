using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
     public class CreateInstructorViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string _lastName;
        private string _firstMidName;
        private DateTime _hireDate;
        private string _fullName;
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

      
        public string LastName
        {
            get { return this._lastName; }
            set { this.SetValue(ref this._lastName, value); }
        }

        public string FirstMidName
        {
            get { return this._firstMidName; }
            set { this.SetValue(ref this._firstMidName, value); }
        }

        public DateTime Hiredate
        {
            get { return this._hireDate; }
            set { this.SetValue(ref this._hireDate, value); }
        }

        public string FullName
        {
            get { return this._fullName; }
            set { this.SetValue(ref this._fullName, value); }
        }


        #endregion

        #region Constructor
        public CreateInstructorViewModel()
        {
            this._apiService = new ApiService();
            this.CreateInstructorCommand = new Command(CreateInstructor);
            this.IsEnable = true;

        }
        #endregion

        #region Methods
        async void CreateInstructor()
        {
            try
            {
                if (this.LastName == null)
                        
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
                var instructorDTB = new InstructorDTO
                {
                      LastName = this.LastName,
                      FirstMidName = this.FirstMidName,
                      HireDate = this.Hiredate
                   

                };
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<InstructorDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.POST_INSTRUCTOR,
                    instructorDTB,
                    ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;


                this.LastName = this.FirstMidName = this.FullName = string.Empty;


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
        public Command CreateInstructorCommand { get; set; }
        #endregion

    }
}
