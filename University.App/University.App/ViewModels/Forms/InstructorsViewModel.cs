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
    public class InstructorsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<InstructorItemViewModel> _instructor;
        private List<InstructorItemViewModel> _allinstrutor;
        private string _filter;

        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<InstructorItemViewModel> Instructor
        {
            get { return this._instructor;}
            set { this.SetValue(ref this._instructor, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set

            {
                this.SetValue(ref this._filter, value);
                this.GetInstructorByFilter();

            }

        }

        #endregion

        #region Constructor
        public InstructorsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetInstructors);
            this.RefreshCommand.Execute(null);
        }
        #endregion

        #region Methods
        async void GetInstructors()
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

                var responseDTO = await _apiService.RequestAPI<List<InstructorItemViewModel>>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_INSTRUCTOR,
                    null,
                    ApiService.Method.Get);

                this._allinstrutor = (List<InstructorItemViewModel>)responseDTO.Data;
                this.Instructor = new ObservableCollection<InstructorItemViewModel>(this._allinstrutor);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }
        }

        void GetInstructorByFilter()
        {
            var instructors = this._allinstrutor;
            if (!string.IsNullOrEmpty(this.Filter))
                instructors = instructors.Where(x => x.LastName.ToLower().Contains(this.Filter)).ToList();
            this.Instructor = new ObservableCollection<InstructorItemViewModel>(instructors);

        }
        #endregion

        #region Commands
        public Command RefreshCommand { get; set; }
        #endregion

    }
}
