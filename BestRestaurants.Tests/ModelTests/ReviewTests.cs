using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestRestaurants.Models;
using System.Collections.Generic;
using System;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class ReviewTests : IDisposable
  {
    public ReviewTests()
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
      int result = Review.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfReviewsAreTheSame_Review()
    {
      // Arrange, Act
      Review firstReview = new Review("Good",1);
      Review secondReview = new Review("Good",1);

      // Assert
      Assert.AreEqual(firstReview, secondReview);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ReviewList()
    {
      //Arrange
      Review testReview = new Review("Good",1);

      //Act
      testReview.Save();
      List<Review> result = Review.GetAll();
      List<Review> testList = new List<Review>{testReview};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Review testReview = new Review("Good",1);

      //Act
      testReview.Save();
      Review savedReview = Review.GetAll()[0];

      int result = savedReview.GetId();
      int testId = testReview.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsReviewInDatabase_Review()
    {
      //Arrange
      Review testReview = new Review("Good",1);
      testReview.Save();

      //Act
      Review foundReview = Review.Find(testReview.GetId());

      //Assert
      Assert.AreEqual(testReview, foundReview);
    }
    [TestMethod]
    public void Update_UpdatesReviewInDatabase_String()
    {
      //Arrange
      string reviewName = "Good";
      Review testReview = new Review(reviewName, 1);
      testReview.Save();
      string newReviewName = "Very bad";

      //Act
      testReview.UpdateReview(newReviewName);

      string result = Review.Find(testReview.GetId()).GetDescription();

      //Assert
      Assert.AreEqual(newReviewName, result);
    }
    [TestMethod]
    public void DeleteReview_DeleteReviewInDatabase_Null()
    {
      //Arrange
      string reviewName = "Very bad";
      Review testReview = new Review(reviewName, 1);
      testReview.Save();
      // string deletedReview = "";

      //Act
      testReview.DeleteReview();
      int result = Review.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);

    }
  }
}
