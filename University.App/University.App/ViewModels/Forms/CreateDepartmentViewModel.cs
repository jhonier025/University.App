using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateDepartmentViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string  _name;
        private int _departmentID;
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



        public string  Name
        {
            get { return this._name; }
            set { this.SetValue(ref this._name, value); }
        }

        public int DepartmentID
        {
            get { return this._departmentID; }
            set { this.SetValue(ref this._departmentID, value); }
        }



        public List<InstructorDTO> Instructors
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
        public CreateDepartmentViewModel()
        {
            this._apiService = new ApiService();
            this.CreateDepartmentCommand = new Command(CreateDepartament);
            this.GetInstructorsCommand = new Command(GetInstructor);
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
        async void CreateDepartament()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Name) ||
                    this.DepartmentID == 0 ||
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
                var departmenDTO = new DepartmentDTO
                {
                    InstructorID = this.IntructorSelected.ID,
                    DepartmentID = this.DepartmentID,
                    Name = this.Name

                };
                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<DepartmentDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.POST_DEPARTMENTS,
                    departmenDTO,
                    ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnable = false;
                this.IsRunning = true;

                this.DepartmentID = this.DepartmentID = 0;
                this.Name = String.Empty;

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
        public Command CreateDepartmentCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }

        #endregion
    }
}
