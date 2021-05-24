using System;
using System.Collections.Generic;
using System.Text;
using Coursework.Models;
using SQLite;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using Coursework.PhotoServices;

namespace Coursework.ViewModel
{
    public class EmployeeFeeVM : INotifyPropertyChanged
    {
        private readonly EmployeeFeeOperations _StoreData;
        private SQLiteAsyncConnection _Connection;
        private Database _dbModel;
        private ImageSource _ClaimImage;
        private EmployeeFee _SaveClaim;
        private EmployeeFee _Employee;
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand PickPhotoCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EmployeeFeeVM(EmployeeFee employee = null)
        {
            _dbModel = new Database();
            _Connection = _dbModel.GetConnection();
            _Connection.CreateTableAsync<EmployeeFee>();
            _StoreData = new EmployeeFeeOperations();
            SaveCommand = new Command(async () => await Save());
            CancelCommand = new Command(cancelEdit);
            PickPhotoCommand = new Command(async () => await PickPhoto());
            _SaveClaim = new EmployeeFee();

            if (employee == null)
            {
                _Employee = new EmployeeFee();
            }
            else
            {
                _Employee = employee;
                _ClaimImage = ImageSource.FromFile(_Employee.ReceiptPhoto);
                OnPropertyChanged(nameof(CImage));
                backupEmployeeFee();
            }
        }
        public string YesNo
        {
            get { return !HasExpenseBeenPaid ? "Unpaid" : "Paid"; }
            set
            {
                _Employee.YesNo = value;
                OnPropertyChanged();
            }
        }
        void cancelEdit()
        {
            Debug.Write("Cancelled");
            restoreEmployeeFee();
            return;
        }

        async Task PickPhoto()
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                var libfolder = FileSystem.AppDataDirectory;
                var imgName = _Employee.ID + ".png";
                string fileName = Path.Combine(libfolder, imgName);

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                _Employee.ReceiptPhoto = fileName;
                _ClaimImage = ImageSource.FromFile(_Employee.ReceiptPhoto);
                OnPropertyChanged(nameof(CImage));
            }
        }
        async Task Save()
        {
            backupEmployeeFee();
            if (String.IsNullOrWhiteSpace(_Employee.FirstName) && String.IsNullOrWhiteSpace(_Employee.Surname))
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "Please enter the name.", "Ok");
                return;
            }
            if (_Employee.ID == 0)
            {
                await _StoreData.AddFees(_Employee);
                MessagingCenter.Send(this, "Claim Added", _Employee);
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await _StoreData.UpdateFees(_Employee);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private void backupEmployeeFee()
        {
            _SaveClaim.ID = _Employee.ID;
            _SaveClaim.FirstName = _Employee.FirstName;
            _SaveClaim.Surname = _Employee.Surname;
            _SaveClaim.DateofExpense = _Employee.DateofExpense;
            _SaveClaim.DateofExpenseAdded = _Employee.DateofExpenseAdded;
            _SaveClaim.TypeofExpense = _Employee.TypeofExpense;
            _SaveClaim.DetailsofExepense = _Employee.DetailsofExepense;
            _SaveClaim.Cost = _Employee.Cost;
            _SaveClaim.VAT = _Employee.VAT;
            _SaveClaim.VATCalc = _Employee.VATCalc;
            _SaveClaim.WithoutVAT = _Employee.WithoutVAT;
            _SaveClaim.ReceiptPhoto = _Employee.ReceiptPhoto;
            _SaveClaim.HasExpenseBeenPaid = _Employee.HasExpenseBeenPaid;
            _SaveClaim.YesNo = _Employee.YesNo;
            _SaveClaim.DateExpenseWasPaid = _Employee.DateExpenseWasPaid;
        }

        private void restoreEmployeeFee()
        {
            ID = _SaveClaim.ID;
            FirstName = _SaveClaim.FirstName;
            Surname = _SaveClaim.Surname;
            DateofExpense = _SaveClaim.DateofExpense;
            DateofExpenseAdded = _SaveClaim.DateofExpenseAdded;
            TypeofExpense = _SaveClaim.TypeofExpense;
            DetailsofExpense = _SaveClaim.DetailsofExepense;
            Cost = _SaveClaim.Cost;
            VAT = _SaveClaim.VAT;
            VATCalc = _SaveClaim.VATCalc;
            WithoutVAT = _SaveClaim.WithoutVAT;
            _Employee.ReceiptPhoto = _SaveClaim.ReceiptPhoto;
            _ClaimImage = ImageSource.FromFile(_Employee.ReceiptPhoto);
            OnPropertyChanged(nameof(CImage));
            HasExpenseBeenPaid = _SaveClaim.HasExpenseBeenPaid;
            YesNo = _SaveClaim.YesNo;
            DateExpenseWasPaid = _SaveClaim.DateExpenseWasPaid;
        }
        public int ID
        {
            get { return _Employee.ID; }
            set { _Employee.ID = value; }
        }
        public string FirstName
        {
            get { return _Employee.FirstName; }
            set
            {
                _Employee.FirstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string Surname
        {
            get { return _Employee.Surname; }
            set
            {
                _Employee.Surname = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string FullName
        {
            get { return $"{_Employee.FirstName} {_Employee.Surname}"; }
        }
        public DateTime DateofExpense
        {
            get { return _Employee.DateofExpense; }
            set
            {
                _Employee.DateofExpense = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateofExpenseAdded
        {
            get { return _Employee.DateofExpenseAdded; }
            set
            {
                _Employee.DateofExpenseAdded = DateTime.Now;
                OnPropertyChanged();
            }
        }
        public string TypeofExpense
        {
            get { return _Employee.TypeofExpense; }
            set
            {
                _Employee.TypeofExpense = value;
                OnPropertyChanged();
            }
        }

        public string DetailsofExpense
        {
            get { return _Employee.DetailsofExepense; }
            set
            {
                _Employee.DetailsofExepense = value;
                OnPropertyChanged();
            }
        }
        public double Cost
        {
            get { return _Employee.Cost; }
            set
            {
                _Employee.Cost = value;
                OnPropertyChanged();
            }
        }
        public Boolean VAT
        {
            get { return _Employee.VAT; }
            set
            {
                _Employee.VAT = value;
                OnPropertyChanged();
            }
        }
        public double VATCalc
        {
            get { return _Employee.VATCalc; }
            set
            {
                if (VAT == true)
                {
                    _Employee.VATCalc = Cost / (10 / 2);
                    OnPropertyChanged();
                }
                else
                {
                    _Employee.VATCalc = 0;
                }
            }
        }
        public double WithoutVAT
        {
            get { return _Employee.WithoutVAT; }
            set
            {
                if (VAT == true)
                {
                    _Employee.WithoutVAT = Cost - VATCalc;
                    OnPropertyChanged();
                }
                else
                {
                    _Employee.WithoutVAT = Cost;
                }
            }
        }
        public ImageSource CImage
        {
            get { return _ClaimImage; }
            private set { }
        }
        public Boolean HasExpenseBeenPaid
        {
            get { return _Employee.HasExpenseBeenPaid; }
            set
            {
                _Employee.HasExpenseBeenPaid = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateExpenseWasPaid
        {
            get { return _Employee.DateExpenseWasPaid; }
            set
            {
                _Employee.DateExpenseWasPaid = value;
                OnPropertyChanged();
            }
        }
    }
}