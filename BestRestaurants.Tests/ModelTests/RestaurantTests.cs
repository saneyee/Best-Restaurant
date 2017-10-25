using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestRestaurants.Models;
using System.Collections.Generic;
using System;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class RestaurantTests : IDisposable
  {
    public RestaurantTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=bestrestaurants_test;";
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
      Review.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Restaurant.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfRestaurantsAreTheSame_Restaurant()
    {
      // Arrange, Act
      Restaurant firstRestaurant = new Restaurant("Taco Bell",1);
      Restaurant secondRestaurant = new Restaurant("Taco Bell",1);

      // Assert
      Assert.AreEqual(firstRestaurant, secondRestaurant);
    }

    [TestMethod]
    public void Save_SavesToDatabase_RestaurantList()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Taco Bell",1);

      //Act
      testRestaurant.Save();
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Taco Bell",1);

      //Act
      testRestaurant.Save();
      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      int result = savedRestaurant.GetId();
      int testId = testRestaurant.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsRestaurantInDatabase_Restaurant()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Taco Bell",1);
      testRestaurant.Save();

      //Act
      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

      //Assert
      Assert.AreEqual(testRestaurant, foundRestaurant);
    }
    [TestMethod]
    public void Update_UpdatesRestaurantInDatabase_String()
    {
      //Arrange
      string restaurantName = "Japonessa";
      Restaurant testRestaurant = new Restaurant(restaurantName, 1);
      testRestaurant.Save();
      string newRestaurantName = "Taco Bell";

      //Act
      testRestaurant.UpdateRestaurantName(newRestaurantName);

      string result = Restaurant.Find(testRestaurant.GetId()).GetRestaurantName();

      //Assert
      Assert.AreEqual(newRestaurantName, result);
    }

    [TestMethod]
    public void DeleteRestaurant_DeleteRestaurantInDatabase_Null()
    {
      //Arrange
      string restaurantName = "Taco Bell";
      Restaurant testRestaurant = new Restaurant(restaurantName, 1);
      testRestaurant.Save();
      // string deletedRestaurant = "";

      //Act
      testRestaurant.DeleteRestaurant();
      int result = Restaurant.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);

    }
  }
}
