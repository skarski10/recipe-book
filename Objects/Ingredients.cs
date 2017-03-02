using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
    public class Ingredient
    {
        private int _id;
        private string _name;

        public Ingredient(string name, int id = 0)
        {
            _id = id;
            _name = name;
        }

        public static List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> allIngredients = new List<Ingredient>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int ingredientId = rdr.GetInt32(0);
                string ingredientName = rdr.GetString(1);
                Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
                allIngredients.Add(newIngredient);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allIngredients;
        }

        public override bool Equals(System.Object otherIngredient)
        {
            if (!(otherIngredient is Ingredient))
            {
                return false;
            }
            else
            {
                Ingredient newIngredient = (Ingredient) otherIngredient;
                bool nameEquality = this.GetIngredientName()  == newIngredient.GetIngredientName();
                return (nameEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO ingredients(name) OUTPUT INSERTED.id VALUES (@IngredientName);", conn);

            SqlParameter nameParameter = new SqlParameter("@IngredientName", this.GetIngredientName());

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



        public static Ingredient Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId;", conn);
            SqlParameter IngredientIdParameter = new SqlParameter("@IngredientId", id.ToString());
            cmd.Parameters.Add(IngredientIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundIngredientId = 0;
            string foundIngredientName = null;

            while(rdr.Read())
            {
                foundIngredientId = rdr.GetInt32(0);
                foundIngredientName = rdr.GetString(1);
            }
            Ingredient foundIngredient = new Ingredient(foundIngredientName, foundIngredientId);

            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundIngredient;
        }

        public List<Recipe> GetRecipes()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM ingredients JOIN ingredients_recipes ON (ingredients.id = ingredients_recipes.ingredients_id) JOIN recipes ON (ingredients_recipes.recipes_id = recipes.id) WHERE ingredients.id = @IngredientId;", conn);

            SqlParameter IngredientIdParameter = new SqlParameter("@IngredientId", this.GetIngredientId().ToString());

            cmd.Parameters.Add(IngredientIdParameter);

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










        public int GetIngredientId()
        {
            return _id;
        }
        public string GetIngredientName()
        {
            return _name;
        }
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
