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
      List<Restaurant> allRestaurants = Restaurant.GetAll();

      return View(allRestaurants);
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
      Console.WriteLine("hello we are in Get");

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

    [HttpGet("/restaurants/{id}/edit")]
    public ActionResult RestaurantEdit(int id)
    {
      // Dictionary<string, object> model = new Dictionary<string, object>();
      // Cuisine selectedCuisine = Cuisine.Find(id); //Cuisine is selected as an object
      // List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants(); //Restaurants are displayed in a list
      //
      // model.Add("cuisine", selectedCuisine);
      // model.Add("restaurants", cuisineRestaurants);

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


  }
}
