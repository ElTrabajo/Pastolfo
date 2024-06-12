using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace InfoClassementSQL
{
    public class InfoClassementSQLClass
    {
        private readonly string connectionString = "server=10.1.139.236;user=d4;pwd=mdp;Database=based4";

        public List<InfoClassement> ChargementInfoClassement()
        {
            List<InfoClassement> rangs = new List<InfoClassement>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT rang_classement, point_classement, nom_classement FROM Classement ORDER BY point_classement DESC";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int rank = 1;
                        while (reader.Read())
                        {
                            int RangClassement = rank++;
                            int PointClassement = reader.GetInt32("point_classement");
                            string NomJoueurClassement = reader.GetString("nom_classement");

                            rangs.Add(new InfoClassement
                            {
                                Rang = RangClassement,
                                Nom = NomJoueurClassement,
                                Point = PointClassement
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion ou de récupération de données
                throw; // Relancer l'exception pour la gestion dans le code appelant
            }

            return rangs;
        }

        public bool UpdateClassementPoints(string nomJoueur, int newPoints)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Classement 
                        SET point_classement = @newPoints 
                        WHERE nom_classement = @nomJoueur";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                        command.Parameters.AddWithValue("@newPoints", newPoints);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion ou de récupération de données
                throw; // Relancer l'exception pour la gestion dans le code appelant
            }
        }
    }

    public class InfoClassement
    {
        public int Rang { get; set; }
        public string Nom { get; set; }
        public int Point { get; set; }
    }
}
