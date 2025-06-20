using CAIT.SQLHelper;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Const;

namespace QuanLyCuaHangDoChoiOnline.Repositories
{
    public class AccountRes
    {
        public static List<Account> GetAll()
        {
            object[] value = { };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_GetAll", value);
            List<Account> lstResult = new List<Account>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Account account = new Account();
                    account.UserName = dr["UserName"].ToString();
                    account.Password = dr["Password"].ToString();
                    account.FullName = dr["FullName"].ToString();
                    account.Age = string.IsNullOrEmpty(dr["Age"].ToString()) ? 0 : int.Parse(dr["Age"].ToString());
                    account.Gender = string.IsNullOrEmpty(dr["Gender"].ToString()) ? 0 : int.Parse(dr["Gender"].ToString());
                    account.Address = dr["Address"].ToString();
                    account.Email = dr["Email"].ToString();
                    account.Phone = "0" + dr["Phone"].ToString();
                    account.Authority = string.IsNullOrEmpty(dr["Authority"].ToString()) ? 0 : int.Parse(dr["Authority"].ToString());


                    lstResult.Add(account);
                }
            }
            return lstResult;
        }
        public static Account CheckLogin(string UserName, string Password)
        {
            object[] value = { UserName };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_GetWithUser", value);
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                var acc = new Account();
                acc.UserName = result.Rows[0]["UserName"].ToString();
                acc.Password = result.Rows[0]["Password"].ToString();
                if (Password.Equals(acc.Password))
                {
                    acc.FullName = result.Rows[0]["FullName"].ToString();
                    acc.Authority = string.IsNullOrEmpty(result.Rows[0]["Authority"].ToString()) ? 0 : int.Parse(result.Rows[0]["Authority"].ToString());
                    return acc;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static Account GetAccountWithUser(string UserName)
        {
            object[] value = { UserName };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_GetWithUser", value);
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                var acc = new Account();
                acc.UserName = result.Rows[0]["UserName"].ToString();
                acc.FullName = result.Rows[0]["FullName"].ToString();
                acc.Age = string.IsNullOrEmpty(result.Rows[0]["Age"].ToString()) ? 0 : int.Parse(result.Rows[0]["Age"].ToString());
                acc.Gender = string.IsNullOrEmpty(result.Rows[0]["Gender"].ToString()) ? 0 : int.Parse(result.Rows[0]["Gender"].ToString());
                acc.Email = result.Rows[0]["Email"].ToString();
                acc.Address = result.Rows[0]["Address"].ToString();
                int phone = string.IsNullOrEmpty(result.Rows[0]["Phone"].ToString()) ? 0 : int.Parse(result.Rows[0]["Phone"].ToString());
                acc.Phone = "0"+phone.ToString();
                acc.Authority = string.IsNullOrEmpty(result.Rows[0]["Authority"].ToString()) ? 0 : int.Parse(result.Rows[0]["Authority"].ToString());

                return acc;
            }
            return new Account();
        }
        public static Account GetAccountWithEmail(string Email)
        {
            object[] value = { Email };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_GetWithEmail", value);
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                var acc = new Account();
                acc.FullName = result.Rows[0]["FullName"].ToString();
                acc.Email = result.Rows[0]["Email"].ToString();
                acc.Address = result.Rows[0]["Address"].ToString();
                int phone = string.IsNullOrEmpty(result.Rows[0]["Phone"].ToString()) ? 0 : int.Parse(result.Rows[0]["Phone"].ToString());
                acc.Phone = "0" + phone.ToString();
                return acc;
            }
            return new Account();
        }
        public static bool Account_Create(Account account)
        {
            object[] value = { account.UserName,account.Password,account.FullName,account.Age, 
                account.Gender,account.Address,account.Email,account.Phone,account.Authority };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_Create", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static bool Account_Update(Account account)
        {
            object[] value = { account.UserName,account.FullName,account.Age,
                account.Gender,account.Address,account.Email,account.Phone,account.Authority };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_Update", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static bool Account_Delete(string userName)
        {
            object[] value = { userName };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Account_Delete", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
    }
}
