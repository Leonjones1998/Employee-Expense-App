using System;
using System.Collections.Generic;
using System.Text;
using Coursework.Models;
using Coursework.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Coursework.AppView;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using SQLite;

namespace Coursework.ViewModel
{
    public class ViewModelMainPage
    {
        private EmployeeFeeOperations _StoreData;
        private bool _LoadData;
        private string _Search;
        private bool _Filter;
        public ObservableCollection<EmployeeFeeVM> Employees { get; private set; } = new ObservableCollection<EmployeeFeeVM>();
        private ObservableCollection<EmployeeFeeVM> _Employees { get; set; } = new ObservableCollection<EmployeeFeeVM>();

        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddEmployeeCommand { get; private set; }
        public ICommand SelectEmployeeData { get; private set; }
        public ICommand DeleteEmployeeData { get; private set; }
        public ICommand SearchClaimCommand { get; private set; }
        public ICommand FilterCommand { get; private set; }
        public ICommand FilterCommands { get; private set; }
        public ICommand ResetCommand { get; private set; }

        public string Search
        {
            get { return _Search; }
            set
            {
                _Search = value;
                if(_Search == "")
                {
                    SearchClaimCommand.Execute(null);
                }
            }
        }
        public bool Filter
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                if (_Filter != true)
                {
                    FilterCommands.Execute(null);
                }
            }
        }
        public ViewModelMainPage()
        {
            _StoreData = new EmployeeFeeOperations();
            LoadDataCommand = new Command(async () => await LoadData());
            AddEmployeeCommand = new Command(async () => await AddFees());
            SelectEmployeeData = new Command<EmployeeFeeVM>(async e => await SelectEmployeeFee(e));
            DeleteEmployeeData = new Command<EmployeeFeeVM>(async e => await DeleteEmployeeFees(e));
            SearchClaimCommand = new Command(SearchClaim);
            FilterCommand = new Command(GetPaidClaim);
            FilterCommands = new Command(GetUnPaidClaim);
            ResetCommand = new Command(Reset);

            MessagingCenter.Subscribe<EmployeeFeeVM, EmployeeFee>(this, "Claim Added", OnEmployeeAdded);
        }
        private async Task LoadData()
        {
            if (_LoadData)
                return;
            _LoadData = true;

            var employees = await _StoreData.GetEmployeeFeeAsync();

            foreach (var employee in employees)
            {
                EmployeeFeeVM newEmployees = new EmployeeFeeVM(employee);
                _Employees.Add(newEmployees);
                Employees.Add(newEmployees);
            }
        }
        private async Task AddFees()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new EmployeeClaimPage(new EmployeeFeeVM()));
        }
        private async Task SelectEmployeeFee(EmployeeFeeVM employee)
        {
            if (employee == null)
                return;
            await Application.Current.MainPage.Navigation.PushAsync(new EmployeeClaimPage(employee));
        }

        private async Task DeleteEmployeeFees(EmployeeFeeVM employeeFeeVM)
        {
            if (await Application.Current.MainPage.DisplayAlert("Warning", $"Are you sure you want to remove {employeeFeeVM.FullName}?", "Yes", "No"))
            {
                Employees.Remove(employeeFeeVM);
                _Employees.Remove(employeeFeeVM);
                var employee = await _StoreData.GetEmployeeFee(employeeFeeVM.ID);
                await _StoreData.DeleteFees(employee);
            }
        }
        private void OnEmployeeAdded(EmployeeFeeVM source, EmployeeFee employee)
        {
            Employees.Add(new EmployeeFeeVM(employee));
            _Employees.Add(new EmployeeFeeVM(employee));
        }
        private void SearchClaim()
        {
            Employees.Clear();
            foreach(var employee in _Employees)
            {
                Employees.Add(employee);
            }

            if(_Search != "")
            {
                foreach(var employee in _Employees)
                {
                    if(!employee.FullName.Contains(_Search))
                    {
                        Employees.Remove(employee);
                    }
                }
            }
        }
        private void GetPaidClaim()
        {
            Employees.Clear();
            foreach (var employee in _Employees)
            {
                Employees.Add(employee);
            }

            if (_Filter != true)
            {
                foreach (var employee in _Employees)
                {
                    if(!employee.HasExpenseBeenPaid.Equals(_Filter))
                    {
                        Employees.Remove(employee);
                    }
                }
            }
        }
        private void GetUnPaidClaim()
        {
            Employees.Clear();
            foreach (var employee in _Employees)
            {
                Employees.Add(employee);
            }

            if (_Filter != true)
            {
                foreach (var employee in _Employees)
                {
                    if (employee.HasExpenseBeenPaid.Equals(_Filter))
                    {
                        Employees.Remove(employee);
                    }
                }
            }
        }
        private void Reset()
        {
            Employees.Clear();
            foreach (var employee in _Employees)
            {
                Employees.Add(employee);
            }
        }
    }
}
