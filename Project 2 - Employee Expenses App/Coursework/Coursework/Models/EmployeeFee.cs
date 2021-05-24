using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;

namespace Coursework.Models
{
    public class EmployeeFee
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string Surname { get; set; }
        [MaxLength(255)]
        public DateTime DateofExpense { get; set; }
        public DateTime DateofExpenseAdded { get; set; }
        public string TypeofExpense { get; set; }
        [MaxLength(255)]
        public string DetailsofExepense { get; set; }
        public double Cost { get; set; }
        [MaxLength(255)]
        public Boolean VAT { get; set; }
        public double VATCalc { get; set; }
        public double WithoutVAT { get; set; }
        public string ReceiptPhoto { get; set; }
        public Boolean HasExpenseBeenPaid { get; set; }
        public DateTime DateExpenseWasPaid { get; set; }
        public string YesNo { get; set; }
    }
}
