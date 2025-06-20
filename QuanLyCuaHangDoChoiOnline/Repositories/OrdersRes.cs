using CAIT.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Const;

namespace QuanLyCuaHangDoChoiOnline.Repositories
{
    public class OrdersRes
    {
        public static bool createOrders(Orders orders)
        {
            object[] value = { orders.OrderID, orders.UserName, orders.OrderPrice, orders.OrderStatus };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_Create", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static List<Orders> GetOrdersUser(string userName) 
        {
            object[] value = { userName };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_GetOrdersUser", value);
            List<Orders> lstResult = new List<Orders>();
            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                foreach (DataRow dr in result.Rows)
                {
                    Orders orders = new Orders();
                    orders.OrderID = dr["OrderID"].ToString();
                    orders.UserName = dr["UserName"].ToString();
                    orders.OrderPrice = string.IsNullOrEmpty(dr["OrderPrice"].ToString()) ? 0 : int.Parse(dr["OrderPrice"].ToString());
                    orders.OrderStatus = string.IsNullOrEmpty(dr["OrderStatus"].ToString()) ? 0 : int.Parse(dr["OrderStatus"].ToString());
                    lstResult.Add(orders);
                }
            }
            return lstResult;
        }
        public static Orders GetOrdersUserOnStatus(string userName,int status)
        {
            object[] value = { userName, status };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_GetOrdersUserOnStatus", value);
            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                foreach (DataRow dr in result.Rows)
                {
                    Orders orders = new Orders();
                    orders.OrderID = dr["OrderID"].ToString();
                    orders.UserName = dr["UserName"].ToString();
                    orders.OrderPrice = string.IsNullOrEmpty(dr["OrderPrice"].ToString()) ? 0 : int.Parse(dr["OrderPrice"].ToString());
                    orders.OrderStatus = string.IsNullOrEmpty(dr["OrderStatus"].ToString()) ? 0 : int.Parse(dr["OrderStatus"].ToString());
                    return orders;
                }
            }
            return null;
        }
        public static Orders GetOrdersWithID(string id) 
        {
            object[] value = { id };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_GetOrdersWithID", value);
            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                foreach (DataRow dr in result.Rows)
                {
                    Orders orders = new Orders();
                    orders.OrderID = dr["OrderID"].ToString();
                    orders.UserName = dr["UserName"].ToString();
                    orders.OrderPrice = string.IsNullOrEmpty(dr["OrderPrice"].ToString()) ? 0 : int.Parse(dr["OrderPrice"].ToString());
                    orders.OrderStatus = string.IsNullOrEmpty(dr["OrderStatus"].ToString()) ? 0 : int.Parse(dr["OrderStatus"].ToString());
                    return orders;
                }
            }
            return null;
        }
        public static bool checkOrders(string userName)
        {
            Orders orders = GetOrdersUserOnStatus(userName, 1);
            if(orders == null)
            {
                Orders orders1 = new Orders();
                Random rnd = new Random();
                int id = rnd.Next(100000,999999);
                while (GetOrdersWithID(id.ToString()) != null)
                {
                    id = rnd.Next(100000, 999999);
                }
                orders1.OrderID = id.ToString();
                orders1.UserName = userName;
                orders1.OrderPrice = 0;
                orders1.OrderStatus = 1;
                createOrders(orders1);
                return true;
            }

            return true;
        }
        public static bool Orders_Update(Orders orders)
        {
            object[] value = { orders.OrderID, orders.UserName,orders.OrderPrice,orders.OrderStatus};
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_Update", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static bool Orders_Delete(string OrderID)
        {
            object[] value = { OrderID };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Orders_Delete", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
    }
}
