using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Coursework.Models
{
    public class EmployeeFeeOperations

    {
        private SQLiteAsyncConnection _Connection;
        private Database _dbModel;
        public EmployeeFeeOperations()
        {
            _dbModel = new Database();
            _Connection = _dbModel.GetConnection();
            _Connection.CreateTableAsync<EmployeeFee>();
        }
        public async Task<List<EmployeeFee>> GetEmployeeFeeAsync()
        {
            return await _Connection.Table<EmployeeFee>().OrderBy(x=>x.FirstName).ToListAsync();
        }
        public async Task DeleteFees(EmployeeFee employee)
        {
            await _Connection.DeleteAsync(employee);
        }
        public async Task AddFees(EmployeeFee employee)
        {
            await _Connection.InsertAsync(employee);
        }
        public async Task UpdateFees(EmployeeFee employee)
        {
            await _Connection.UpdateAsync(employee);
        }
        public async Task<EmployeeFee> GetEmployeeFee(int id)
        {
            return await _Connection.FindAsync<EmployeeFee>(id);
        }
        public async Task<List<EmployeeFee>> GetPaidClaim()
        {
            return await _Connection.QueryAsync<EmployeeFee>("SELECT * FROM EmployeeFee WHERE HasExpenseBeenPaid = ?", true);
        }
        public async Task<List<EmployeeFee>> GetUnPaidClaim()
        {
            return await _Connection.QueryAsync<EmployeeFee>("SELECT * FROM EmployeeFee WHERE HasExpenseBeenPaid = ?", false);
        }
    }
}
