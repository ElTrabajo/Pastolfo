using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoClassementSQL
{
    public class InfoClassementSQLClass
    {
        public List<InfoClassement> ChargementInfoClassement()
        {
            List<InfoClassement> rangs = new List<InfoClassement>();
            string ConnectionString = $"server=10.1.139.236;user=d4;pwd=mdp;Database=based4";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT rang_classement, point_classement, nom_classement FROM Classement ORDER BY rang_classement ASC, point_classement DESC";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int RangClassement = reader.GetInt32("rang_classement");
                        int PointClassement = reader.GetInt32("point_classement");
                        string NomJoueurClassement = reader.GetString("nom_classement");

                        rangs.Add(new InfoClassement
                        {
                            Rang = RangClassement,
                            Nom = NomJoueurClassement,
                            Point = PointClassement
                        });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle connection or data retrieval errors
                throw ex; // Re-throw the exception for handling in the calling code
            }

            return rangs;
        }
    }

    public class InfoClassement
    {
        public int Rang { get; set; }
        public string Nom { get; set; }
        public int Point { get; set; }
    }
}
