using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Coursework.ViewModel;
using Coursework.AppView;

namespace Coursework.AppView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeClaimPage : ContentPage
    {
        public EmployeeClaimPage(EmployeeFeeVM employeeFeeVM)
        {
            InitializeComponent();
            ViewModel = employeeFeeVM;
            Title = (employeeFeeVM.TypeofExpense == null) ? "New Claim" : "Edit Claim";
            if(employeeFeeVM.TypeofExpense == null)
            {
                PayingClaims.IsEnabled = false;
                DateofPayingClaims.IsEnabled = false;
            }
            else if (employeeFeeVM.TypeofExpense != null)
            {
                PayingClaims.IsEnabled = true;
                DateofPayingClaims.IsEnabled = true;
            }
        }
        public EmployeeFeeVM ViewModel
        {
            get { return BindingContext as EmployeeFeeVM; }
            set { BindingContext = value; }
        }

        protected override void OnDisappearing()
        {
            ViewModel.CancelCommand.Execute(null);
            base.OnDisappearing();
        }
    }
}