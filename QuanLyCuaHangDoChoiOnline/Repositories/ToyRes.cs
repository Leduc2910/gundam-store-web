using CAIT.SQLHelper;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Const;

namespace QuanLyCuaHangDoChoiOnline.Repositories
{
    public class ToyRes
    {
        public static List<Toy> GetAll()
        {
            object[] value = { };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_GetAll", value);
            List<Toy> lstResult = new List<Toy>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Toy toy = new Toy();
                    toy.ToyID = dr["ToyID"].ToString();
                    toy.ToyName = dr["ToyName"].ToString();
                    toy.ToyTypeID = dr["ToyTypeID"].ToString();
                    toy.Brand = dr["Brand"].ToString();
                    toy.Description = dr["Description"].ToString();
                    toy.Image = dr["Image"].ToString();
                    toy.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 : int.Parse(dr["Price"].ToString());
                    toy.Quantity = string.IsNullOrEmpty(dr["Quantity"].ToString()) ? 0 : int.Parse(dr["Quantity"].ToString());
                    toy.OrderedQuantity = string.IsNullOrEmpty(dr["OrderedQuantity"].ToString()) ? 0 : int.Parse(dr["OrderedQuantity"].ToString());

                    lstResult.Add(toy);
                }
            }
            return lstResult;
        }
        public static List<Toy> GetToyWithSelling()
        {
            object[] value = { };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_ToyWithSelling", value);
            List<Toy> lstResult = new List<Toy>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Toy toy = new Toy();
                    toy.ToyID = dr["ToyID"].ToString();
                    toy.ToyName = dr["ToyName"].ToString();
                    toy.ToyTypeID = dr["ToyTypeID"].ToString();
                    toy.Brand = dr["Brand"].ToString();
                    toy.Description = dr["Description"].ToString();
                    toy.Image = dr["Image"].ToString();
                    toy.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 : int.Parse(dr["Price"].ToString());
                    toy.Quantity = string.IsNullOrEmpty(dr["Quantity"].ToString()) ? 0 : int.Parse(dr["Quantity"].ToString());
                    toy.OrderedQuantity = string.IsNullOrEmpty(dr["OrderedQuantity"].ToString()) ? 0 : int.Parse(dr["OrderedQuantity"].ToString());
                    lstResult.Add(toy);
                }
            }
            return lstResult;
        }

        public static List<Toy> GetTypeList(string ID)
        {
            object[] value = { ID };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_GetTypeList", value);
            List<Toy> lstResult = new List<Toy>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Toy toy = new Toy();
                    toy.ToyID = dr["ToyID"].ToString();
                    toy.ToyName = dr["ToyName"].ToString();
                    toy.ToyTypeID = dr["ToyTypeID"].ToString();
                    toy.Brand = dr["Brand"].ToString();
                    toy.Description = dr["Description"].ToString();
                    toy.Image = dr["Image"].ToString();
                    toy.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 : int.Parse(dr["Price"].ToString());
                    toy.Quantity = string.IsNullOrEmpty(dr["Quantity"].ToString()) ? 0 : int.Parse(dr["Quantity"].ToString());
                    toy.OrderedQuantity = string.IsNullOrEmpty(dr["OrderedQuantity"].ToString()) ? 0 : int.Parse(dr["OrderedQuantity"].ToString());
                    lstResult.Add(toy);
                }
            }
            return lstResult;
        }
        public static Toy ToyWithID(string ID)
        {
            object[] value = { ID };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_ToyWithID", value);
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Toy toy = new Toy();
                    toy.ToyID = dr["ToyID"].ToString();
                    toy.ToyName = dr["ToyName"].ToString();
                    toy.ToyTypeID = dr["ToyTypeID"].ToString();
                    toy.Brand = dr["Brand"].ToString();
                    toy.Description = dr["Description"].ToString();
                    toy.Image = dr["Image"].ToString();
                    toy.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 : int.Parse(dr["Price"].ToString());
                    toy.Quantity = string.IsNullOrEmpty(dr["Quantity"].ToString()) ? 0 : int.Parse(dr["Quantity"].ToString());
                    toy.OrderedQuantity = string.IsNullOrEmpty(dr["OrderedQuantity"].ToString()) ? 0 : int.Parse(dr["OrderedQuantity"].ToString());
                    return toy;
                }
            }
            return null;
        }

        public static List<Toy> ToyWithName(string name)
        {
            object[] value = { name };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_ToyWithName", value);
            List<Toy> lstResult = new List<Toy>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    Toy toy = new Toy();
                    toy.ToyID = dr["ToyID"].ToString();
                    toy.ToyName = dr["ToyName"].ToString();
                    toy.ToyTypeID = dr["ToyTypeID"].ToString();
                    toy.Brand = dr["Brand"].ToString();
                    toy.Description = dr["Description"].ToString();
                    toy.Image = dr["Image"].ToString();
                    toy.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 : int.Parse(dr["Price"].ToString());
                    toy.Quantity = string.IsNullOrEmpty(dr["Quantity"].ToString()) ? 0 : int.Parse(dr["Quantity"].ToString());
                    toy.OrderedQuantity = string.IsNullOrEmpty(dr["OrderedQuantity"].ToString()) ? 0 : int.Parse(dr["OrderedQuantity"].ToString());

                    lstResult.Add(toy);
                }
            }
            return lstResult;
        }

        public static int Toy_Count()
        {
            return GetAll().Count;
        }
        public static bool Toy_CreateToy(Toy toy)
        {
            object[] value = { toy.ToyID,toy.ToyName, toy.ToyTypeID, toy.Brand, toy.Description, toy.Image, toy.Price, toy.Quantity, toy.OrderedQuantity };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_CreateToy", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static bool Toy_Update(Toy toy)
        {
            object[] value = { toy.ToyID, toy.ToyName, toy.ToyTypeID, toy.Brand, toy.Description, toy.Image, toy.Price, toy.Quantity, toy.OrderedQuantity };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_Update", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
        public static bool Toy_Delete(string ID)
        {
            object[] value = { ID };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_Delete", value);

            if (connection.errorCode == 0 && connection.errorMessage == "")
            {
                return true;
            }
            return false;
        }
    }
}
