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
                List<Recipe> allRecipes = Recipe.GetAllRecipes();
                return View["recipes.cshtml", allRecipes];

            };
            Post["/recipes"] = _ => {
                Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-instructions"]);
                newRecipe.Save();
                List<Recipe> allRecipes = Recipe.GetAllRecipes();
                return View["recipes.cshtml", allRecipes];
            };

        }
    }
}
