using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BestRestaurants.Models
{
  public class Review
  {
    private string _description;
    private int _id;
    private int _restaurantId;

    public Review(string description, int restaurantId, int Id = 0 )
    {
      _description = description;
      _restaurantId = restaurantId;
      _id = Id;
    }

    public override bool Equals(System.Object otherReview)
    {
    if (!(otherReview is Review))
      {
        return false;
      }
      else
      {
        Review newReview = (Review) otherReview;
        bool idEquality = (this.GetId() == newReview.GetId());
        bool descriptionEquality = (this.GetDescription() == newReview.GetDescription());
        bool restaurantIdEquality = this.GetRestaurantId() == newReview.GetRestaurantId();

        return (idEquality && descriptionEquality && restaurantIdEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }

    public string GetDescription()
    {
      return _description;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetRestaurantId()
    {
      return _restaurantId;
    }

    public void UpdateReview(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE reviews SET description = @newDescription WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);

      cmd.ExecuteNonQuery();
      _description = newDescription;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void DeleteReview()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM reviews WHERE id = @thisId;";

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
      cmd.CommandText = @"INSERT INTO reviews (description, restaurant_id) VALUES (@description, @restaurant_id);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@restaurant_id";
      restaurantId.Value = this._restaurantId;
      cmd.Parameters.Add(restaurantId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM reviews;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int reviewId = rdr.GetInt32(0);
        string reviewDescription = rdr.GetString(1);
        int reviewRestaurantId = rdr.GetInt32(2);

        Review newReview = new Review(reviewDescription, reviewRestaurantId, reviewId);
        allReviews.Add(newReview);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allReviews;
    }

    public static Review Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `reviews` WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int reviewId = 0;
      string reviewDescription = "";
      int reviewRestaurantId = 0;


      while (rdr.Read())
      {
        reviewId = rdr.GetInt32(0);
        reviewDescription = rdr.GetString(1);
        reviewRestaurantId = rdr.GetInt32(2);
      }

      Review newReview = new Review(reviewDescription, reviewRestaurantId, reviewId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newReview;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM reviews;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
