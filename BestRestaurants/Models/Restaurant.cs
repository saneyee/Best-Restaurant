using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BestRestaurants.Models
{
  public class Restaurant
  {
    private string _restaurantName;
    private int _cuisineId;
    private int _id;

    public Restaurant(string restaurantName, int cuisineId, int Id = 0)
    {
      _restaurantName = restaurantName;
      _cuisineId = cuisineId;
      _id = Id;
    }

    public override bool Equals(System.Object otherRestaurant)
    {
    if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool idEquality = (this.GetId() == newRestaurant.GetId());
        bool restaurantNameEquality = (this.GetRestaurantName() == newRestaurant.GetRestaurantName());
        bool cuisineEquality = this.GetCuisineId() == newRestaurant.GetCuisineId();

        return (idEquality && restaurantNameEquality && cuisineEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetRestaurantName().GetHashCode();
    }

    public string GetRestaurantName()
    {
      return _restaurantName;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetCuisineId()
    {
      return _cuisineId;
    }

    public List<Review> GetReview()
    {
        List<Review> allRestaurantReviews = new List<Review> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @restaurant_id;";

        MySqlParameter restaurantId = new MySqlParameter();
        restaurantId.ParameterName = "@restaurant_id";
        restaurantId.Value = this._id;
        cmd.Parameters.Add(restaurantId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int reviewId = rdr.GetInt32(0);
          string reviewDescription = rdr.GetString(1);
          string reviewerName = rdr.GetString(2);
          int reviewRestaurantId = rdr.GetInt32(3);

          Review newReview = new Review(reviewDescription, reviewRestaurantId, reviewerName, reviewId);
          allRestaurantReviews.Add(newReview);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allRestaurantReviews;
    }

    public void UpdateRestaurantName(string newRestaurantName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE restaurants SET restaurant_name = @newRestaurantName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter restaurantName = new MySqlParameter();
      restaurantName.ParameterName = "@newRestaurantName";
      restaurantName.Value = newRestaurantName;
      cmd.Parameters.Add(restaurantName);

      cmd.ExecuteNonQuery();
      _restaurantName = newRestaurantName;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void DeleteRestaurant()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = _id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO restaurants (restaurant_name, cuisine_id) VALUES (@restaurantName, @cuisine_id);";

      MySqlParameter restaurantName = new MySqlParameter();
      restaurantName.ParameterName = "@restaurantName";
      restaurantName.Value = this._restaurantName;
      cmd.Parameters.Add(restaurantName);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisine_id";
      cuisineId.Value = this._cuisineId;
      cmd.Parameters.Add(cuisineId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int restaurantCuisineId = rdr.GetInt32(2);

        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
        allRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allRestaurants;
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `restaurants` WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int restaurantId = 0;
      string restaurantName = "";
      int restaurantCuisineId = 0;


      while (rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        restaurantName = rdr.GetString(1);
        restaurantCuisineId = rdr.GetInt32(2);
      }

      Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newRestaurant;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
