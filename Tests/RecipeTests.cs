using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cookbook
{
    public class RecipeTest : IDisposable
    {

        public static Recipe firstRecipe = new Recipe("Ham", "A Delicious Ham Sandwich", "Butter Bread. Add Ham. Add Cheese. Eat.");
        public static Recipe secondRecipe = new Recipe("Ham","A Delicious Ham Sandwich", "Butter Bread. Add Ham. Add Cheese. Eat.");
        public static Category firstCategory = new Category("Dinner");
        public static Ingredient firstIngredient = new Ingredient("Garlic Powder");


        public RecipeTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void RecipeDatabaseEmpty()
        {
            //Arrange, act
            int result = Recipe.GetAllRecipes().Count;

            //Assert
            Assert.Equal(0,result);
        }

        [Fact]
        public void Test_EqualOverrideTrueForSameDescription()
        {
            //Arrange, Act

            var thirdRecipe = secondRecipe;

            Assert.Equal(firstRecipe, thirdRecipe);
            //Assert
        }

        [Fact]
        public void Test_SaveToDatabase()
        {
            // Arrange
            Recipe firstRecipe = new Recipe("Ham", "A Delicious Ham Sandwich","Butter Bread. Add Ham. Add Cheese. Eat.");
            firstRecipe.Save();

            // Act
            List<Recipe> result = Recipe.GetAllRecipes();
            List<Recipe> testList = new List<Recipe>{firstRecipe};

            // Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            firstRecipe.Save();

            //Act
            Recipe testRecipe = Recipe.GetAllRecipes()[0];

            int result = firstRecipe.GetRecipeId();
            int testId = testRecipe.GetRecipeId();

            //Assert
            Assert.Equal(testId, result);
        }


        [Fact]
        public void Test_Find_FindsRecipeinDatablase()
        {
            //Arrange
            firstRecipe.Save();
            //Act
            Recipe foundRecipe = Recipe.Find(firstRecipe.GetRecipeId());

            //Asswert
            Assert.Equal(firstRecipe, foundRecipe);
        }

        [Fact]
        public void Test_GetRecipes_ReturnsAllRecipesInACategory()
        {
            //Arrange
            firstCategory.Save();
            firstRecipe.Save();

            firstCategory.AddRecipe(firstRecipe);
            List<Recipe> savedRecipes = firstCategory.GetRecipes();
            List<Recipe> firstList = new List<Recipe> {firstRecipe};

            //Assert
            Assert.Equal(firstList, savedRecipes);
        }

        [Fact]
        public void Test_InstructionsSplitIntoList()
        {
            // Arragne
            firstRecipe.Save();
            List<string> testInstructions = new List<string> {"Butter Bread", "Add Ham", "Add Cheese", "Eat"};

            // Act
            List<string> splitList = firstRecipe.SplitInstructions();

            // Assert
            Assert.Equal(testInstructions, splitList);
        }

        [Fact]
        public void Test_GetIngredients_ReturnsAllIngredientsInARecipe()
        {
            //Arrange
            firstRecipe.Save();
            firstIngredient.Save();

            firstRecipe.AddIngredient(firstIngredient);
            List<Ingredient> savedIngredients = firstRecipe.GetIngredients();
            List<Ingredient> firstList = new List<Ingredient> {firstIngredient};

            //Assert
            Assert.Equal(firstList, savedIngredients);
        }





        public void Dispose()
        {
            Category.DeleteAll();
            Recipe.DeleteAll();
            Ingredient.DeleteAll();
        }
    }
}
