using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<StudentItemViewModel> _Students;
        private List<StudentItemViewModel> _allStudents;
        private string _filter;

        #endregion


        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }
               
      

        public ObservableCollection<StudentItemViewModel> Students
        {
            get { return this._Students; }
            set { this.SetValue(ref this._Students, value); }
        }


       
        public string Filter
        {
            get { return this._filter; }
            set

            {
                this.SetValue(ref this._filter, value);
                this.GetStudentByFilter();

            }

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
                

                var responseDTO = await _apiService.RequestAPI<List<StudentItemViewModel>>(Endpoints.URL_BASE_UNIVERSITY_API,
                   Endpoints.GET_STUDENTS,
                   null,
                   ApiService.Method.Get);

                this._allStudents = (List<StudentItemViewModel>)responseDTO.Data;
                this.Students= new ObservableCollection<StudentItemViewModel>(this._allStudents);
                this.IsRefreshing= false;

               
                

            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificacion", ex.Message, "Cancel");
            }
        }


        void GetStudentByFilter()
        {
            var Students = this._allStudents;
            if (!string.IsNullOrEmpty(this.Filter))
                Students = Students.Where(x => x.FullName.ToLower().Contains(this.Filter)).ToList();
            this.Students = new ObservableCollection<StudentItemViewModel>(Students);

        }

        #endregion


        #region Commands

        public Command RefreshCommand { get; set; }
        #endregion

    }

}
