using System;
using University.App.Helpers;
using University.App.Views.Forms;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class StudentItemViewModel : StudentDTO
    {
        #region Fields
        private ApiService _apiService;
        #endregion


        #region Methods

        async void EditStudent()
        {
            MainViewModel.GetInstance().EditStudent = new EditStudentViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditStudentPage());

        }

        async void DeleteStudent()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Notification",
                        "Delete confirm",
                        "Yes",
                        "No");

                if (!answer)
                    return;

                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification",
                        "No internet connection",
                        "Cancel");
                    return;
                }

                var message = "The process is successful";
                var responseDTO = await _apiService.RequestAPI<StudentDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.DELETE_STUDENTS + this.ID,
                    null,
                    ApiService.Method.Delete);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                await Application.Current.MainPage.DisplayAlert("Notification",
                   message,
                    "Cancel");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }


        }
        #endregion

        #region Comands

        public Command EditStudentCommand { get; set; }
        public Command DeleteStudentCommand { get; set; }
        #endregion


        #region Contructor
        public StudentItemViewModel()
        {
            this._apiService = new ApiService();
            
                this._apiService = new ApiService();
                this.EditStudentCommand = new Command(EditStudent);
                this.DeleteStudentCommand = new Command(DeleteStudent);
            

        }
        #endregion
    }
}
