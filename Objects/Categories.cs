using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
    public class Category
    {
        private int _id;
        private string _name;

        public Category(string name, int id = 0)
        {
            _id = id;
            _name = name;
        }

        public static List<Category> GetAllCategories()
        {
            List<Category> allCategorys = new List<Category>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int recipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                Category newCategory = new Category(recipeName, recipeId);
                allCategorys.Add(newCategory);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allCategorys;
        }

        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool nameEquality = this.GetCategoryName()  == newCategory.GetCategoryName();
                return (nameEquality);
            }
        }











        public int GetCategoryId()
        {
            return _id;
        }
        public string GetCategoryName()
        {
            return _name;
        }
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
