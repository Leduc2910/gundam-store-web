using CAIT.SQLHelper;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangDoChoiOnline.Models;
using QuanLyCuaHangDoChoiOnline.Const;

namespace QuanLyCuaHangDoChoiOnline.Repositories
{
    public class ToyTypeRes
    {
        public static List<ToyType> GetAllType()
        {
            object[] value = { };
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_GetAllType", value);
            List<ToyType> lstResult = new List<ToyType>();
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    ToyType toyType = new ToyType();
                    toyType.ToyTypeID = dr["ToyTypeID"].ToString();
                    toyType.ToyTypeName = dr["ToyTypeName"].ToString();

                    lstResult.Add(toyType);
                }
            }
            return lstResult;
        }

        public static ToyType ToyTypeWithID(string ID)
        {
            object[] value = { ID};
            SQLCommand connection = new SQLCommand(ConstValue.ConnectString);
            DataTable result = connection.Select("Toy_ToyTypeWithID", value);
            if (connection.errorCode == 0 && result.Rows.Count > 0)
            {
                foreach (DataRow dr in result.Rows)
                {
                    ToyType toyType = new ToyType();
                    toyType.ToyTypeID = dr["ToyTypeID"].ToString();
                    toyType.ToyTypeName = dr["ToyTypeName"].ToString();
                    return toyType;
                }
            }
            return null;
        }

    }
}
