using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BestRestaurants.Models;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class CuisineTests : IDisposable
  {
        public CuisineTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889 ;database=bestrestaurants_test;";
        }

        public void Dispose()
        {
          Restaurant.DeleteAll();
          Cuisine.DeleteAll();
          Review.DeleteAll();
        }

       [TestMethod]
       public void GetAll_CategoriesEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Cuisine.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Cuisine()
      {
        //Arrange, Act
        Cuisine firstCuisine = new Cuisine("Mexican");
        Cuisine secondCuisine = new Cuisine("Mexican");

        //Assert
        Assert.AreEqual(firstCuisine, secondCuisine);
      }

      [TestMethod]
      public void Save_SavesCuisineToDatabase_CuisineList()
      {
        //Arrange
        Cuisine testCuisine = new Cuisine("Mexican");
        testCuisine.Save();

        //Act
        List<Cuisine> result = Cuisine.GetAll();
        List<Cuisine> testList = new List<Cuisine>{testCuisine};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToCuisine_Id()
     {
       //Arrange
       Cuisine testCuisine = new Cuisine("Mexican");
       testCuisine.Save();

       //Act
       Cuisine savedCuisine = Cuisine.GetAll()[0];

       int result = savedCuisine.GetId();
       int testId = testCuisine.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Find_FindsCuisineInDatabase_Cuisine()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      //Assert
      Assert.AreEqual(testCuisine, foundCuisine);
    }

    [TestMethod]
    public void GetRestaurants_RetrievesAllRestaurantsWithCuisine_RestaurantList()
    {
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();

      Restaurant firstRestaurant = new Restaurant("Mexican", testCuisine.GetId());
      firstRestaurant.Save();
      Restaurant secondRestaurant = new Restaurant("Japanese", testCuisine.GetId());
      secondRestaurant.Save();


      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

      CollectionAssert.AreEqual(testRestaurantList, resultRestaurantList);
    }

  }
}
