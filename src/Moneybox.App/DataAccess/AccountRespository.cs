using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Moneybox.App.DataAccess
{
    public class AccountRespository : IAccountRepository
    {


        private AccountRespository()
        {
        
        }

        
        // mock account collection
        private readonly List<Account> _accounts = new List<Account>
            {
                new Account{Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), User =new User(){ Id=new Guid("12223344-5566-7788-99AA-BBCCDDEEFF00"), Name="Test1" ,Email = "test1@gmail.com" } ,Balance = 100 ,Withdrawn=50 , PaidIn=50  },
                new Account{Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF01"), User =new User(){ Id=new Guid("12223344-5566-7788-99AA-BBCCDDEEFF01"), Name="Test1" ,Email = "test1@gmail.com" }, Balance = 100 ,Withdrawn=40, PaidIn=50  },
            };

        /// <summary>
        /// Method to get detail from accounts
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Account GetAccountById(Guid accountId)
        {           
            try
            {
                var data = _accounts.FirstOrDefault(x => x.Id == accountId);
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Method: GetAccountByID: Errorwhile retreving details",ex.InnerException);
            }           
        }

        /// <summary>
        /// Method to update the details in accounts
        /// </summary>
        /// <param name="account"></param>
        public void Update(Account account)
        {   
            try
            {
                var data = _accounts.FirstOrDefault(x => x.Id == account.Id);
                if (data != null)
                {                        
                    data.Balance = account.Balance;
                    data.Withdrawn = account.Withdrawn;
                    data.Balance = account.PaidIn;                            
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Method: Update: Errorwhile updating details", ex.InnerException);
            }
        }
    }
}
