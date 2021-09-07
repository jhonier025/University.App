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
    public class OfficeViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        
        private ObservableCollection<OfficeItemViewModel> _office;
        private List<OfficeItemViewModel> _alloffice;
        private string _filter;

        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

       

        public ObservableCollection<OfficeItemViewModel> Office
        {
            get { return this._office; }
            set { this.SetValue(ref this._office, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set

            {
                this.SetValue(ref this._filter, value);
                this.GetOfficeByFilter();

            }

        }

        #endregion

        #region Constructor
        public OfficeViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetOffice);
            this.RefreshCommand.Execute(null);
        }
        #endregion

        #region Methods
        async void GetOffice()
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

                var responseDTO = await _apiService.RequestAPI<List<OfficeItemViewModel>>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_OFFICES,
                    null,
                    ApiService.Method.Get);

                this._alloffice = (List<OfficeItemViewModel>)responseDTO.Data;
                this.Office = new ObservableCollection<OfficeItemViewModel>(this._alloffice);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }
        }

        void GetOfficeByFilter()
        {
            var office = this.Office;
            if (!string.IsNullOrEmpty(this.Filter))
               // office = office.Where(x => x.Instructor.ToLower().Contains(this.Filter)).ToList();
            this.Office = new ObservableCollection<OfficeItemViewModel>(office);

        }
        #endregion

        #region Commands
        public Command RefreshCommand { get; set; }
        #endregion
    }
}
