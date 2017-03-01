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
        public static Recipe secondRecipe = new Recipe("Ham", "A Delicious Ham Sandwich", "Butter Bread. Add Ham. Add Cheese. Eat.");


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








        public void Dispose()
        {
            Category.DeleteAll();
        }
    }
}
