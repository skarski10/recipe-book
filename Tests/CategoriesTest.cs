using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cookbook
{
    public class CategoryTest : IDisposable
    {
        public static Category firstCategory = new Category("Dinner");
        public static Category secondCategory = new Category("Dinner");
        public static Category thirdCategory = new Category("lunch");
        public static Recipe firstRecipe = new Recipe("Ham", "A Delicious Ham Sandwich", "Butter Bread. Add Ham. Add Cheese. Eat.");


        public CategoryTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void CategoryDatabaseEmpty()
        {
            //Arrange, act
            int result = Category.GetAllCategories().Count;

            //Assert
            Assert.Equal(0,result);
        }

        [Fact]
        public void Test_EqualOverrideTrueForSameCategoryName()
        {
            //Arrange, Act, Assert
            Assert.Equal(firstCategory, secondCategory);

        }
        [Fact]
        public void Test_SaveToDatabase()
        {
            // Arrange
            firstCategory.Save();

            // Act
            List<Category> result = Category.GetAllCategories();
            List<Category> testList = new List<Category>{firstCategory};

            // Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            firstCategory.Save();

            //Act
            Category testCategory = Category.GetAllCategories()[0];

            int result = firstCategory.GetCategoryId();
            int testId = testCategory.GetCategoryId();

            //Assert
            Assert.Equal(testId, result);
        }


        [Fact]
        public void Test_Find_FindsCategoryinDatablase()
        {
            //Arrange
            firstCategory.Save();
            //Act
            Category foundCategory = Category.Find(firstCategory.GetCategoryId());

            //Asswert
            Assert.Equal(firstCategory, foundCategory);
        }

        public void Test_GetCategories_ReturnsAllCategoriesForARecipe()
       {
           //Arrange
           firstRecipe.Save();
           firstCategory.Save();

           firstRecipe.AddCategory(firstCategory);
           List<Category> savedCategories = firstRecipe.GetCategories();
           List<Category> firstList = new List<Category> {firstCategory};

           //Assert
           Assert.Equal(firstList, savedCategories);
       }







        public void Dispose()
        {
            Category.DeleteAll();
            Recipe.DeleteAll();
        }
    }
}
