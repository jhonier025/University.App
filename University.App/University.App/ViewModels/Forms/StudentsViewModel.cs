using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class StudentsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        
        
        private ObservableCollection<StudentDTO> _Students;
        #endregion


        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }
               
      

        public ObservableCollection<StudentDTO> Students
        {
            get { return this._Students; }
            set { this.SetValue(ref this._Students, value); }
        }
        #endregion


        #region Constructor
        public StudentsViewModel()
        {
            this._apiService= new ApiService();
            
            this.RefreshCommand = new Command(GetStudent);
            this.RefreshCommand.Execute(null);


        }


        #endregion

        
        #region Methods
        async void GetStudent(object obj)
        {
            try
            {

                this.IsRefreshing = true;
                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Notificacion",
                        "No internet connection",
                        "Cancel");

                    return;
                }
                

                var responseDTO = await _apiService.RequestAPI<List<StudentDTO>>(Endpoints.URL_BASE_UNIVERSITY_API,
                   Endpoints.GET_STUDENTS,
                   null,
                   ApiService.Method.Get);

                this.Students= new ObservableCollection<StudentDTO>((List<StudentDTO>)responseDTO.Data);
                this.IsRefreshing= false;

               
                

            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificacion", ex.Message, "Cancel");
            }
        }

        #endregion
        #region Commands

        public Command RefreshCommand { get; set; }
        #endregion

    }

}
