using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Coursework.ViewModel;

namespace Coursework
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = new ViewModelMainPage();
            InitializeComponent();
        }
        public ViewModelMainPage ViewModel
        {
            get { return BindingContext as ViewModelMainPage; }
            set { BindingContext = value; }
        }
        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        void OnClaimSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectEmployeeData.Execute(e.SelectedItem);
            ListView list = sender as ListView;
            list.SelectedItem = -1;
        }
    }
}
