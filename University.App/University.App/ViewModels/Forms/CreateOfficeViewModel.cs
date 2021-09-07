using System;
using System.Collections.Generic;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateOfficeViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string _location;
        private bool _isEnabled;
        private bool _isRunning;

        private List<InstructorDTO> _instructor;
        private InstructorDTO _instructorSelected;

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

       

        public string Location
        {
            get { return this._location; }
            set { this.SetValue(ref this._location, value); }
        }


        public  List<InstructorDTO> Instructors
        {
            get { return this._instructor; }
            set { this.SetValue(ref this._instructor, value); }
        }

        public InstructorDTO IntructorSelected
        {
            get { return this._instructorSelected; }
            set { this.SetValue(ref this._instructorSelected, value); }
        }
        #endregion

        #region Constructor
        public CreateOfficeViewModel()
        {
            this._apiService = new ApiService();
            this.CreateOfficeCommand = new Command(CreateOffice);
            this.GetInstructorsCommand= new Command(GetInstructor);
            this.GetInstructorsCommand.Execute(null);

            this.IsEnable = true;

           

        }
        #endregion

        #region Methods

        async void GetInstructor()
        {
            try
            {
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

                var responseDTO = await _apiService.RequestAPI<List<InstructorDTO>>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_INSTRUCTORS,
                    null,
                    ApiService.Method.Get);

                this.Instructors = (List<InstructorDTO>)responseDTO.Data;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }


        }
        async void CreateOffice()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Location)||
                    this.IntructorSelected == null)
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
                var officeDTO = new OfficeDTO
                {
                   InstructorID = this.IntructorSelected.ID,
                   Location = this.Location

                };
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<OfficeDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.POST_OFFICES,
                    officeDTO,
                    ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

                this.Location = String.Empty;

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
        public Command CreateOfficeCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }

        #endregion

    }
}
