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

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories(name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);

            SqlParameter nameParameter = new SqlParameter("@CategoryName", this.GetCategoryName());

            cmd.Parameters.Add(nameParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }



        public static Category Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);
            SqlParameter CategoryIdParameter = new SqlParameter("@CategoryId", id.ToString());
            cmd.Parameters.Add(CategoryIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCategoryId = 0;
            string foundCategoryName = null;

            while(rdr.Read())
            {
                foundCategoryId = rdr.GetInt32(0);
                foundCategoryName = rdr.GetString(1);
            }
            Category foundCategory = new Category(foundCategoryName, foundCategoryId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundCategory;
        }

        public void AddRecipe(Recipe newRecipe)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (categories_id, recipes_id) VALUES (@CategoryId, @RecipeId);", conn);

            SqlParameter recipeIdParameter = new SqlParameter ("@RecipeId", newRecipe.GetRecipeId());
            cmd.Parameters.Add(recipeIdParameter);

            SqlParameter categoryIdParameter = new SqlParameter("@CategoryId", this.GetCategoryId());
            cmd.Parameters.Add(categoryIdParameter);

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public List<Recipe> GetRecipes()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN categories_recipes ON (categories.id = categories_recipes.categories_id) JOIN recipes ON (categories_recipes.recipes_id = recipes.id) WHERE categories.id = @CategoryId;", conn);

            SqlParameter CategoryIdParameter = new SqlParameter("@CategoryId", this.GetCategoryId().ToString());

            cmd.Parameters.Add(CategoryIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Recipe> newList = new List<Recipe>{};

            while(rdr.Read())
            {
                int recipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                string recipeDescription = rdr.GetString(2);
                string recipeInstructions = rdr.GetString(3);

                Recipe newRecipe = new Recipe(recipeName, recipeDescription, recipeInstructions, recipeId);
                newList.Add(newRecipe);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return newList;

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
