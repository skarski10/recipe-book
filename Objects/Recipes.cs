using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
    public class Recipe
    {
        private int _id;
        private string _name;
        private string _description;
        private string _instructions;

        public Recipe(string name, string description, string instructions, int id = 0)
        {
            _id = id;
            _name = name;
            _description = description;
            _instructions = instructions;
        }

        public static List<Recipe> GetAllRecipes()
        {
            List<Recipe> allRecipes = new List<Recipe>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int recipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                string recipeDescription = rdr. GetString(2);
                string recipeInstructions = rdr. GetString(3);
                Recipe newRecipe = new Recipe(recipeName, recipeDescription, recipeInstructions, recipeId);
                allRecipes.Add(newRecipe);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allRecipes;
        }










        public int GetRecipeId()
        {
            return _id;
        }
        public string GetRecipeName()
        {
            return _name;
        }
        public string GetDescription()
        {
            return _description;
        }
        public string GetInstructions()
        {
            return _instructions;
        }
        public static void DeleteAll()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM recipes;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }
    }
}
