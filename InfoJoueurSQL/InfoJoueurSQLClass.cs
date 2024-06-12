using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace InfoJoueurSQL
{
    public class InfoJoueurSQLClass
    {
        private readonly string connectionString = "server=10.1.139.236;user=d4;pwd=mdp;Database=based4";

        public List<InfoJoueur> ChargementInfoJoueurs()
        {
            List<InfoJoueur> joueurs = new List<InfoJoueur>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT nom_joueur, nbVies_joueur, score_joueur, id_monde, etat_joueur FROM Joueur";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            joueurs.Add(new InfoJoueur
                            {
                                Nom = reader.GetString("nom_joueur"),
                                IdMonde = reader.GetInt32("id_monde"),
                                Vies = reader.GetInt32("nbVies_joueur"),
                                Score = reader.GetInt32("score_joueur"),
                                Etat = reader.GetString("etat_joueur")
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

            return joueurs;
        }

        public bool VerificationNomJoueur(string nomJoueur)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT 1 FROM Joueur WHERE nom_joueur = @nomJoueur";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion ou de récupération de données
                return false; // Supposer que la valeur n'existe pas si une erreur se produit
            }
        }

        public int CreateJoueur(string nomJoueur, int scoreJoueur, int nbViesJoueur, string etatJoueur, int idMonde)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string queryClassement = "INSERT INTO Classement (point_classement, nom_classement) VALUES (@scoreJoueur, @nomJoueur)";
                            using (MySqlCommand commandClassement = new MySqlCommand(queryClassement, connection, transaction))
                            {
                                commandClassement.Parameters.AddWithValue("@scoreJoueur", scoreJoueur);
                                commandClassement.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                                commandClassement.ExecuteNonQuery();
                            }

                            string queryGetRang = "SELECT rang_classement FROM Classement WHERE nom_classement = @nomJoueur";
                            int newRangJoueur;
                            using (MySqlCommand commandGetRang = new MySqlCommand(queryGetRang, connection, transaction))
                            {
                                commandGetRang.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                                object rangResult = commandGetRang.ExecuteScalar();
                                newRangJoueur = rangResult != null ? Convert.ToInt32(rangResult) : throw new InvalidOperationException("Failed to retrieve rang_classement.");
                            }

                            string queryJoueur = "INSERT INTO Joueur (nom_joueur, score_joueur, nbVies_joueur, etat_joueur, id_monde, rang_joueur) VALUES (@nomJoueur, @scoreJoueur, @nbViesJoueur, @etatJoueur, @idMonde, @rangJoueur)";
                            using (MySqlCommand commandJoueur = new MySqlCommand(queryJoueur, connection, transaction))
                            {
                                commandJoueur.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                                commandJoueur.Parameters.AddWithValue("@scoreJoueur", scoreJoueur);
                                commandJoueur.Parameters.AddWithValue("@nbViesJoueur", nbViesJoueur);
                                commandJoueur.Parameters.AddWithValue("@etatJoueur", etatJoueur);
                                commandJoueur.Parameters.AddWithValue("@idMonde", idMonde);
                                commandJoueur.Parameters.AddWithValue("@rangJoueur", newRangJoueur);
                                commandJoueur.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return 1; // Indique une insertion réussie
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw; // Relancer l'exception pour la gestion dans le code appelant
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion ou de récupération de données
                throw; // Relancer l'exception pour la gestion dans le code appelant
            }
        }

        public bool UpdateJoueur(string nomJoueur, int scoreJoueur, int nbViesJoueur, string etatJoueur, int idMonde)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Joueur 
                        SET score_joueur = @scoreJoueur, 
                            nbVies_joueur = @nbViesJoueur, 
                            etat_joueur = @etatJoueur, 
                            id_monde = @idMonde 
                        WHERE nom_joueur = @nomJoueur";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nomJoueur", nomJoueur);
                        command.Parameters.AddWithValue("@scoreJoueur", scoreJoueur);
                        command.Parameters.AddWithValue("@nbViesJoueur", nbViesJoueur);
                        command.Parameters.AddWithValue("@etatJoueur", etatJoueur);
                        command.Parameters.AddWithValue("@idMonde", idMonde);

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

    public class InfoJoueur
    {
        public string Nom { get; set; }
        public int IdMonde { get; set; }
        public int Vies { get; set; }
        public int Score { get; set; }
        public string Etat { get; set; }
    }
}
