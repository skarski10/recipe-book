using System.Collections.Generic;
using Nancy;
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
              Recipe selectedRecipe = Recipe.Find(parameters.id);
              Dictionary<string, object> model = new Dictionary<string, object>();
              return View["recipe.cshtml", selectedRecipe];

            };
        }
    }
}
