using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using University.App.Helpers;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class DepartmentsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<DepartmentItemViewModel> _department;
        private List<DepartmentItemViewModel> _alldepartament;
        private string _filter;

        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<DepartmentItemViewModel> Department
        {
            get { return this._department; }
            set { this.SetValue(ref this._department, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set

            {
                this.SetValue(ref this._filter, value);
                this.GetDepatmentByFilter();

            }

        }

        #endregion


        #region Constructor
        public DepartmentsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetDepartment);
            this.RefreshCommand.Execute(null);
        }
        #endregion


        #region Methods
        async void GetDepartment()
        {
            try
            {
                this.IsRefreshing = true;
                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Notification",
                        "No internet connection",
                        "Cancel");
                    return;
                }

                var responseDTO = await _apiService.RequestAPI<List<DepartmentItemViewModel>>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_DEPARTMENTS,
                    null,
                    ApiService.Method.Get);

                this._alldepartament = (List<DepartmentItemViewModel>)responseDTO.Data;
                this.Department = new ObservableCollection<DepartmentItemViewModel>(this._alldepartament);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }
        }

        void GetDepatmentByFilter()
        {
            var department= this._alldepartament;
            if (!string.IsNullOrEmpty(this.Filter))
                department = department.Where(x => x.DepartmentID.ToLower().Contains(this.Filter)).ToList();
            this.Department = new ObservableCollection<DepartmentItemViewModel>(department);

        }
        #endregion

        #region Commands
        public Command RefreshCommand { get; set; }
        #endregion
    }
}
