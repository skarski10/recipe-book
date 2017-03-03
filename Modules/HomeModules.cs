using System.Collections.Generic;
using Nancy;
using System;
using Nancy.ViewEngines.Razor;

namespace Cookbook
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                return View["index.cshtml"];
            };
            Get["/categories"] = _ => {
                return View["categories.cshtml"];
            };

            Get["/recipes"] = _ => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                List<Recipe> allRecipes = Recipe.GetAllRecipes();
                List<Category> allCategories = Category.GetAllCategories();
                model.Add("recipe", allRecipes);
                model.Add("categories", allCategories);
                return View["recipes.cshtml", model];
            };
            Post["/recipes"] = _ => {

                Console.WriteLine("Ckeck returns: " + Request.Form["boxtest"]);

                Dictionary<string, object> model = new Dictionary<string, object>();
                Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-instructions"]);
                Category newCategory = Category.Find(Request.Form["assign-category"]);
                newRecipe.Save();
                newRecipe.AddCategory(newCategory);
                List<Category> allCategories = Category.GetAllCategories();
                List<Recipe> allRecipes = Recipe.GetAllRecipes();

                model.Add("recipe", allRecipes);
                model.Add("categories", allCategories);
                return View["recipes.cshtml", model];
            };

            Get["/categories"] = _ => {
                List<Category> allCategories = Category.GetAllCategories();
                return View["categories.cshtml", allCategories];
            };
            Post["/categories"] = _ => {
                Category newCategory = new Category(Request.Form["category-name"]);
                newCategory.Save();
                List<Category> allCategories = Category.GetAllCategories();
                return View["categories.cshtml", allCategories];
            };
            Get["/category/{id}/{name}"] = parameters => {

                Category selectedCategory = Category.Find(parameters.id);
                List<Recipe> allRecipes = selectedCategory.GetRecipes();

                Dictionary<string, object> model = new Dictionary<string, object>();
                model.Add("category", selectedCategory);
                model.Add("recipes", allRecipes);

                return View["category.cshtml", model];
            };

            Get["/recipe/{id}/{name}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Recipe selectedRecipe = Recipe.Find(parameters.id);
                List<Ingredient> ingredientList = Ingredient.GetAllIngredients();
                model.Add("recipe", selectedRecipe);
                model.Add("instructions", selectedRecipe.SplitInstructions());
                model.Add("ingredients", ingredientList);
                return View["recipe.cshtml", model];
            };

            Post["/recipe/{id}/{name}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                // Ingredient newIngredient = new Ingredient();
                Recipe selectedRecipe = Recipe.Find(parameters.id);
                List<Ingredient> recipeIngredients = new List<Ingredient>{};
                if (Request.Form["ingredient"] != "")
                {
                    var newIngredient = Request.Form["ingredient"];
                    foreach(var ingredient in Ingredient.Find(newIngredient))
                    {
                        selectedRecipe.AddIngredient(ingredient);
                    }
                }
                model.Add("recipe", selectedRecipe);
                model.Add("instructions", selectedRecipe.SplitInstructions());
                model.Add("ingredients", recipeIngredients);
                return View["recipe.cshtml", model];
            };
        }
    }
}
