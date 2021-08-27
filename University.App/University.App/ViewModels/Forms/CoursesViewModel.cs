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
    public class CoursesViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<CourseItemViewModel> _courses;
        private List<CourseItemViewModel> _allcourses;
        private string _filter;

         #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<CourseItemViewModel> Courses
        {
            get { return this._courses; }
            set { this.SetValue(ref this._courses, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set
            
            {
                this.SetValue(ref this._filter, value);
                this.GetCoursesByFilter();
               
            }
           
        }

        #endregion

        #region Constructor
        public CoursesViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetCourses);
            this.RefreshCommand.Execute(null);
        }
        #endregion

        #region Methods
        async void GetCourses()
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

                var responseDTO = await _apiService.RequestAPI<List<CourseItemViewModel>>(Endpoints.URL_BASE_UNIVERSITY_API,
                    Endpoints.GET_COURSES,
                    null,
                    ApiService.Method.Get);

                this._allcourses = (List<CourseItemViewModel>)responseDTO.Data;
                this.Courses = new ObservableCollection<CourseItemViewModel>(this._allcourses);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "Cancel");
            }
        }

        void GetCoursesByFilter()
        {
            var courses = this._allcourses;
            if (!string.IsNullOrEmpty(this.Filter))
                courses = courses.Where(x => x.Title.ToLower().Contains(this.Filter)).ToList();
            this.Courses = new ObservableCollection<CourseItemViewModel>(courses);

        }
        #endregion

        #region Commands
        public Command RefreshCommand { get; set; }
        #endregion
    }
}
