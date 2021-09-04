using System;
using University.App.Helpers;
using University.App.Views.Forms;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CourseItemViewModel : CourseDTO
    {
        #region Fields
        private ApiService _apiService;
        #endregion


        #region Methods

        async void EditCourse()
        {
           MainViewModel.GetInstance().EditCourse = new EditCourseViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage());
        
       }
        async void DeleteCourses()
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
                var responseDTO = await _apiService.RequestAPI<CourseDTO>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.DELETE_COURSES + this.CourseID,
                    null,
                    ApiService.Method.Delete);

                if (responseDTO.Code <= 200 || responseDTO.Code > 299)
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

        public Command EditCourseCommand { get; set; }
        public Command DeleteCourseCommand { get; set; }
        #endregion


        #region Contructor
        public CourseItemViewModel()
        {
            this._apiService = new ApiService();
            this.EditCourseCommand = new Command(EditCourse);
            this.DeleteCourseCommand = new Command(DeleteCourses);
        }
        #endregion
    }
}
