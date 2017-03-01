using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cookbook
{
    public class RecipeTest
    {
        public void StudentTest()
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
    }
}
