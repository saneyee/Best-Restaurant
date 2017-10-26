using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;

namespace BestRestaurants.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Restaurant> allRestaurants = Restaurant.GetAll();
      List<Cuisine> allCuisines = Cuisine.GetAll();

      model.Add("restaurant",allRestaurants);
      model.Add("cuisines",allCuisines);

      return View("Index",model);
    }

    [HttpGet("/cuisine/add")]
    public ActionResult AddCuisine()
    {
      return View();
    }

    [HttpPost("/cuisine/list")]
    public ActionResult WriteCuisineList()
    {
      Cuisine newCuisine = new Cuisine(Request.Form["cuisine-name"]);
      newCuisine.Save();
      List<Cuisine> allCuisines = Cuisine.GetAll();

      return View("ViewCuisines", allCuisines);
    }

    [HttpGet("/cuisine/list")]
    public ActionResult ReadCuisineList()
    {
      List<Cuisine> allCuisines = Cuisine.GetAll();

      return View("ViewCuisines", allCuisines);
    }

    [HttpGet("/{name}/{id}/restaurantlist")]
    public ActionResult ViewRestaurantList(int id)
    {
      // Console.WriteLine("hello we are in Get");

      Dictionary<string, object> model = new Dictionary<string, object>();
      Cuisine selectedCuisine = Cuisine.Find(id); //Cuisine is selected as an object
      List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants(); //Restaurants are displayed in a list

      model.Add("cuisine", selectedCuisine);
      model.Add("restaurants", cuisineRestaurants);

      //return the restaurant list for selected cuisine
      return View("CuisineOutlets", model);
    }

    [HttpGet("/{name}/{id}/restaurant/add")]
    public ActionResult AddRestaurant(int id)
    {
      Cuisine selectedCuisine = Cuisine.Find(id); //Cuisine is selected as an object

      return View(selectedCuisine);

    }

    [HttpPost("/{name}/{id}/restaurantlist")]
    public ActionResult AddRestaurantViewRestaurantList(int id)
    {
      Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], id);
      newRestaurant.Save();
      Dictionary<string, object> model = new Dictionary<string, object>();
      Cuisine selectedCuisine = Cuisine.Find(id); //Cuisine is selected as an object
      List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants(); //Restaurants are displayed in a list
      // Console.WriteLine(id);

      model.Add("cuisine", selectedCuisine);
      model.Add("restaurants", cuisineRestaurants);
      // Console.WriteLine(cuisineRestaurants[0].GetDescription());

      //return the restaurant list for selected cuisine
      return View("CuisineOutlets", model);
    }

    [HttpGet("/restaurant/{name}/{id}/review")]
    public ActionResult ViewRestaurantReviews(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant selectedRestaurant = Restaurant.Find(id); //Restaurant is selected as an object
      List<Review> restaurantReviews = selectedRestaurant.GetReview(); //Reviews of the selected restaurant are displayed in a list

      model.Add("restaurant", selectedRestaurant);
      model.Add("reviews", restaurantReviews);

      //return the restaurant list for selected cuisine
      return View("RestaurantReviews", model);
    }

    //id is restaurant id
    [HttpPost("/restaurant/{name}/{id}/review")]
    public ActionResult AddReviewViewRestaurantReviews(int id)
    {
      Review newReview = new Review(Request.Form["review-description"],id,Request.Form["reviewer"]);
      newReview.Save();
      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant selectedRestaurant = Restaurant.Find(id); //Restaurant is selected as an object
      List<Review> restaurantReviews = selectedRestaurant.GetReview(); //Reviews of the selected restaurant are displayed in a list

      model.Add("restaurant", selectedRestaurant);
      model.Add("reviews", restaurantReviews);

      //return the restaurant list for selected cuisine
      return View("RestaurantReviews", model);
    }

    //id is restaurant id
    [HttpGet("/restaurants/{id}/review/add")]
    public ActionResult AddReview(int id)
    {
      Restaurant selectedRestaurant = Restaurant.Find(id);
      return View(selectedRestaurant);
    }

    [HttpGet("/restaurants/{id}/edit")]
    public ActionResult RestaurantEdit(int id)
    {
      Restaurant thisRestaurant = Restaurant.Find(id);
      return View(thisRestaurant);
    }

    [HttpPost("/restaurants/{id}/edit")]
    public ActionResult RestaurantEditConfirm(int id)
    {
      Restaurant thisRestaurant = Restaurant.Find(id);
      thisRestaurant.UpdateRestaurantName(Request.Form["new-name"]);
      return RedirectToAction("Index");
    }

    [HttpGet("/restaurants/{id}/delete")]
    public ActionResult RestaurantDeleteConfirm(int id)
    {
      Restaurant thisRestaurant = Restaurant.Find(id);
      thisRestaurant.DeleteRestaurant();
      return RedirectToAction("Index");
    }

    //Id is review id
    [HttpGet("/{restaurant_id}/review/{id}/edit")]
    public ActionResult EditRestaurantReview(int restaurant_id, int id)
    {
      Review thisReview = Review.Find(id);

      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant selectedRestaurant = Restaurant.Find(restaurant_id); //Restaurant is selected as an object

      model.Add("restaurant", selectedRestaurant);
      model.Add("review", thisReview);

      return View("ReviewEdit", model);
    }

    //Id is review id
    [HttpPost("/{restaurant_id}/review/{id}/edit")]
    public ActionResult ReviewEditConfirm(int restaurant_id, int id)
    {
      Review thisReview = Review.Find(id);
      thisReview.UpdateReview(Request.Form["new-review"]);

      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant selectedRestaurant = Restaurant.Find(restaurant_id); //Restaurant is selected as an object
      List<Review> restaurantReviews = selectedRestaurant.GetReview(); //Reviews of the selected restaurant are displayed in a list

      model.Add("restaurant", selectedRestaurant);
      model.Add("reviews", restaurantReviews);

      return View("RestaurantReviews", model);
    }

    [HttpGet("/{restaurant_id}/review/{id}/delete")]
    public ActionResult ReviewDeleteConfirm(int restaurant_id, int id)
    {
      Review thisReview = Review.Find(id);
      thisReview.DeleteReview();

      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant selectedRestaurant = Restaurant.Find(restaurant_id); //Restaurant is selected as an object
      List<Review> restaurantReviews = selectedRestaurant.GetReview(); //Reviews of the selected restaurant are displayed in a list

      model.Add("restaurant", selectedRestaurant);
      model.Add("reviews", restaurantReviews);

      return View("RestaurantReviews", model);
    }
  }
}
