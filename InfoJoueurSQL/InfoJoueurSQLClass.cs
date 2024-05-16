﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoJoueurSQL
{
    public class InfoJoueurSQLClass
    {
        public List<InfoJoueur> ChargementInfoJoueurs()
        {
            List<InfoJoueur> joueurs = new List<InfoJoueur>();
            string ConnectionString = $"server=10.1.139.236;user=d4;pwd=mdp;Database=based4";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT nom_joueur, nbVies_joueur, score_joueur, id_monde FROM Joueur";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string NomJoueur = reader.GetString("nom_joueur");
                        int ViesJoueur = reader.GetInt32("nbVies_joueur");
                        int ScoreJoueur = reader.GetInt32("score_joueur");
                        int IdMonde = reader.GetInt32("id_monde");

                        joueurs.Add(new InfoJoueur
                        {
                            Nom = NomJoueur,
                            IdMonde = IdMonde,
                            Vies = ViesJoueur,
                            Score = ScoreJoueur
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

            return joueurs;
        }
    }

    public class InfoJoueur
    {
        public string Nom { get; set; }
        public int IdMonde { get; set; }
        public int Vies { get; set; }
        public int Score { get; set; }
    }
}