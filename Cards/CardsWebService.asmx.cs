using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Cards
{
    /// <summary>
    /// Summary description for CardsWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CardsWebService : System.Web.Services.WebService
    {
        private const string ConnectionString = "your_database_connection_string";
        [WebMethod]
        public string GetCards()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Cards", connection);
                SqlDataReader reader = command.ExecuteReader();

                // TODO: Process the result and return the list of cards
            }

            return "List of cards";
        }

        [WebMethod]
        public string GetCard(int cardId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Cards WHERE Id = {cardId}", connection);
                SqlDataReader reader = command.ExecuteReader();

                // TODO: Process the result and return card details
            }

            return "Card details";
        }

        [WebMethod]
        public string CreateCard(string name, string description, string color)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO Cards (Name, Description, Color) VALUES ('{name}', '{description}', '{color}')", connection);
                command.ExecuteNonQuery();
            }

            return "Card created";
        }

        [WebMethod]
        public string UpdateCard(int cardId, string name, string description, string color, string status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE Cards SET Name = '{name}', Description = '{description}', Color = '{color}', Status = '{status}' WHERE Id = {cardId}", connection);
                command.ExecuteNonQuery();
            }

            return "Card updated";
        }

        [WebMethod]
        public string DeleteCard(int cardId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"DELETE FROM Cards WHERE Id = {cardId}", connection);
                command.ExecuteNonQuery();
            }

            return "Card deleted";
        }
    }
}
