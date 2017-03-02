using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cookbook
{
    public class IngredientTest : IDisposable
    {
        public static Ingredient firstIngredient = new Ingredient("Salt");
        public static Ingredient secondIngredient = new Ingredient("Salt");
        public static Ingredient thirdIngredient = new Ingredient("Pepper");
        public static Recipe firstRecipe = new Recipe("Ham", "A Delicious Ham Sandwich", "Butter Bread. Add Ham. Add Cheese. Eat.");


        public IngredientTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void IngredientDatabaseEmpty()
        {
            //Arrange, act
            int result = Ingredient.GetAllIngredients().Count;

            //Assert
            Assert.Equal(0,result);
        }

        [Fact]
        public void Test_EqualOverrideTrueForSameIngredientName()
        {
            //Arrange, Act, Assert
            Assert.Equal(firstIngredient, secondIngredient);

        }
        [Fact]
        public void Test_SaveToDatabase()
        {
            // Arrange
            firstIngredient.Save();

            // Act
            List<Ingredient> result = Ingredient.GetAllIngredients();
            List<Ingredient> testList = new List<Ingredient>{firstIngredient};

            // Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            firstIngredient.Save();

            //Act
            Ingredient testIngredient = Ingredient.GetAllIngredients()[0];

            int result = firstIngredient.GetIngredientId();
            int testId = testIngredient.GetIngredientId();

            //Assert
            Assert.Equal(testId, result);
        }


        [Fact]
        public void Test_Find_FindsIngredientinDatablase()
        {
            //Arrange
            firstIngredient.Save();
            //Act
            Ingredient foundIngredient = Ingredient.Find(firstIngredient.GetIngredientId());

            //Asswert
            Assert.Equal(firstIngredient, foundIngredient);
        }

    






        public void Dispose()
        {
            Ingredient.DeleteAll();
            Recipe.DeleteAll();
        }
    }
}
