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
                string recipeDescription = rdr.GetString(2);
                string recipeInstructions = rdr.GetString(3);
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

        public override bool Equals(System.Object otherRecipe)
        {
            if (!(otherRecipe is Recipe))
            {
                return false;
            }
            else
            {
                Recipe newRecipe = (Recipe) otherRecipe;
                bool nameEquality = this.GetRecipeName()  == newRecipe.GetRecipeName();
                return (nameEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO recipes(name, description, instructions) OUTPUT INSERTED.id VALUES (@RecipeName, @RecipeDescription, @RecipeInstructions);", conn);

            SqlParameter nameParameter = new SqlParameter("@RecipeName", this.GetRecipeName());
            SqlParameter descriptionParameter = new SqlParameter("@RecipeDescription", this.GetDescription());
            SqlParameter instructionsParameter = new SqlParameter("@RecipeInstructions", this.GetInstructions());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(descriptionParameter);
            cmd.Parameters.Add(instructionsParameter);

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



        public static Recipe Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);
            SqlParameter RecipeIdParameter = new SqlParameter("@RecipeId", id.ToString());
            cmd.Parameters.Add(RecipeIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundRecipeId = 0;
            string foundRecipeName = null;
            string foundRecipeDescription = null;
            string foundRecipeInstructions = null;

            while(rdr.Read())
            {
                foundRecipeId = rdr.GetInt32(0);
                foundRecipeName = rdr.GetString(1);
                foundRecipeDescription = rdr.GetString(2);
                foundRecipeInstructions = rdr.GetString(3);
            }
            Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeDescription, foundRecipeInstructions, foundRecipeId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundRecipe;
        }

        public void AddCategory(Category newCategory)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (categories_id, recipes_id) VALUES (@CategoryId, @RecipeId);", conn);

            SqlParameter categoryIdParameter = new SqlParameter ("@CategoryId", newCategory.GetCategoryId());
            cmd.Parameters.Add(categoryIdParameter);

            SqlParameter recipeIdParameter = new SqlParameter("@RecipeId", this.GetRecipeId());
            cmd.Parameters.Add(recipeIdParameter);

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public List<Category> GetCategories()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT categories.* FROM recipes JOIN recipes_categories ON (recipes.id = recipes_categories.recipes_id) JOIN categories ON (recipes_categories.categories_id = categories.id) WHERE recipes.id = @RecipeId;", conn);

            SqlParameter RecipeIdParameter = new SqlParameter("@RecipeId", this.GetRecipeId().ToString());

            cmd.Parameters.Add(RecipeIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Category> newList = new List<Category>{};

            while(rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);

                Category newCategory = new Category(categoryName, categoryId);
                newList.Add(newCategory);
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

        public List<string> SplitInstructions()
        {
            string[] delimiters = { "!","@","#","$","%","^","&","*","_","+","{","}","[","]","|","?","/","<",">", ".", ". "};
            string[] instructionsArray = this.GetInstructions().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            List<string> instructionList = new List<string>{};

            foreach (var index in instructionsArray)
            {
                instructionList.Add(index);
            }

            return instructionList;
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
