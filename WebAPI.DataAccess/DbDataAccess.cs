using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DataAccess
{
    public class DbDataAccess : IDataAccess
    {
        public string ConnectionString;
        
        public DbDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<Player> FindPlayerByName(string playerName)
        {
            string query = @"SELECT player_id, name FROM [player] WHERE name = @NAME";
            Player player = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@NAME", SqlDbType.NVarChar);
                            command.Parameters["@NAME"].Value = playerName;

                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                player = new Player();
                                player.Id = (int)rdr["player_id"];
                                player.Name = rdr["name"].ToString();
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return player;
        }

        public async Task<Player> FindPlayerById(int id)
        {
            string query = @"SELECT player_id, name FROM [player] WHERE player_id = @ID";
            Player player = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@ID", SqlDbType.Int);
                            command.Parameters["@ID"].Value = id;

                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                player = new Player();
                                player.Id = (int)rdr["player_id"];
                                player.Name = rdr["name"].ToString();
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return player;
        }
        public async Task<int> SavePlayer(string playerName)
        {
            string query = @"INSERT INTO [player](name) VALUES(@NAME); SELECT SCOPE_IDENTITY();";
            int newId = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@NAME", SqlDbType.NVarChar);
                            command.Parameters["@NAME"].Value = playerName;

                            newId = Decimal.ToInt32((decimal)await command.ExecuteScalarAsync());  
                            if(newId > 0)
                                tran.Commit();
                        }
                        catch (Exception Ex)
                        {
                            connection.Close();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return newId;
        }

        public async Task<bool> SavePlayerScore(PlayerScore playerScore)
        {
            string query = @"INSERT INTO [player_stats](player_id, score, session_start, session_end) 
                                VALUES(@ID, @SCORE, @SESSION_START, @SESSION_END)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@ID", SqlDbType.Int);
                            command.Parameters.Add("@SCORE", SqlDbType.Int);
                            command.Parameters.Add("@SESSION_START", SqlDbType.DateTime);
                            command.Parameters.Add("@SESSION_END", SqlDbType.DateTime);
                            
                            command.Parameters["@ID"].Value = playerScore.PlayerId;
                            command.Parameters["@SCORE"].Value = playerScore.Scores.FirstOrDefault().Score;
                            command.Parameters["@SESSION_START"].Value = playerScore.Scores.FirstOrDefault().SessionStart;
                            command.Parameters["@SESSION_END"].Value = playerScore.Scores.FirstOrDefault().SessionEnd;

                            int rowsAffected = command.ExecuteNonQuery();  
                            if(rowsAffected != 0)
                                tran.Commit();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return true;
        }

        public async Task<IList<GameScore>> GetPlayerScores(string playerName)
        {
            string query = @"SELECT ps.player_id, ps.score, ps.session_start, ps.session_end 
                                FROM player_stats ps 
                                    INNER JOIN player 
                                        ON ps.player_id = player.player_id 
                                WHERE player.name = @NAME";
            List<GameScore> playerScores = new List<GameScore>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();   
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@NAME", SqlDbType.NVarChar);
                            command.Parameters["@NAME"].Value = playerName;

                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                GameScore gameScore = new GameScore
                                {
                                    Score = (int)rdr["score"],
                                    SessionStart = DateTime.Parse(rdr["session_start"].ToString()),
                                    SessionEnd = DateTime.Parse(rdr["session_end"].ToString())
                                };
                                playerScores.Add(gameScore);
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return playerScores;
        }
        
        public async Task<IList<GameScore>> GetPlayerScores(int playerId)
        {
            string query = @"SELECT ps.player_id, ps.score, ps.session_start, ps.session_end 
                                FROM player_stats ps 
                                    INNER JOIN player 
                                        ON ps.player_id = player.player_id 
                                WHERE player.player_id = @ID";
            List<GameScore> playerScores = new List<GameScore>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();   
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@ID", SqlDbType.NVarChar);
                            command.Parameters["@ID"].Value = playerId;

                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                GameScore gameScore = new GameScore
                                {
                                    Score = (int)rdr["score"],
                                    SessionStart = DateTime.Parse(rdr["session_start"].ToString()),
                                    SessionEnd = DateTime.Parse(rdr["session_end"].ToString())
                                };
                                playerScores.Add(gameScore);
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return playerScores;
        }
        
        public async Task<IList<PlayerSummrayResult>> GetTop10PlayersByScoreAndDuration(DateTime dateStart, DateTime dateEnd)
        {
            string query = @"SELECT TOP(10) player_id, name, (total_score + play_duration_seconds) ranking_score, total_score, play_duration_seconds
                                FROM (
                                    SELECT pl.player_id, pl.name, SUM(ps.score) total_score, SUM(DATEDIFF(s, ps.session_start, ps.session_end)) play_duration_seconds
                                    FROM [player_stats] ps 
                                    LEFT JOIN [player] pl 
                                    ON ps.player_id = pl.player_id 
                                    WHERE ps.session_start > @DATE_START AND ps.session_start < @DATE_END
                                    GROUP BY pl.player_id, pl.name 
                                ) AS [h_table] 
                                ORDER BY ranking_score DESC";
            List<PlayerSummrayResult> playerResults = new List<PlayerSummrayResult>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();   
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            command.Parameters.Add("@DATE_START", SqlDbType.DateTime);
                            command.Parameters.Add("@DATE_END", SqlDbType.DateTime);
                            command.Parameters["@DATE_START"].Value = dateStart;
                            command.Parameters["@DATE_END"].Value = dateEnd;

                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                PlayerSummrayResult gameScore = new PlayerSummrayResult
                                {
                                    Id = (int)rdr["player_id"],
                                    Name = rdr["name"].ToString(),
                                    RankingScore = (int)rdr["ranking_score"],
                                    TotalScore = (int)rdr["total_score"],
                                    TotalPlayDurationSeconds = (int)rdr["play_duration_seconds"],
                                    
                                };
                                playerResults.Add(gameScore);
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return playerResults;
        }
        public async Task<IList<PlayerImpactReportResult>> GetImpactReport()
        {
            string query = @"WITH [first_scores] AS (
                                SELECT ps.player_id, score AS first_score, first_playthrough 
                                FROM player_stats ps
                                JOIN (
                                    SELECT player_id, MIN(session_start) first_playthrough 
                                    FROM player_stats 
                                    GROUP BY player_id
                                ) AS a
                                ON a.player_id = ps.player_id
                                WHERE ps.session_start = first_playthrough)
                            SELECT 
                                pl.player_id, 
                                pl.name, 
                                MAX(first_score) first_score,
                                MAX(ps.score) best_score,
                                COUNT(ps.player_id) playthroughs, 
                                SUM(DATEDIFF(s, ps.session_start, ps.session_end)) play_duration_seconds 
                            FROM first_scores, [player_stats] ps
                            LEFT JOIN player pl 
                            ON ps.player_id = pl.player_id
                            where ps.player_id = first_scores.player_id
                            GROUP BY pl.player_id, pl.name
                            ORDER BY pl.player_id;";
            List<PlayerImpactReportResult> playerResults = new List<PlayerImpactReportResult>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();   
                using (var tran = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(query, connection, tran))
                    {
                        try
                        {
                            SqlDataReader rdr = await command.ExecuteReaderAsync(); 

                            while (rdr.Read())
                            {
                                PlayerImpactReportResult gameScore = new PlayerImpactReportResult
                                {
                                    Id = (int)rdr["player_id"],
                                    Name = rdr["name"].ToString(),
                                    FirstScore = (int)rdr["first_score"],
                                    BestScore = (int)rdr["best_score"],
                                    Playthroughs = (int)rdr["playthroughs"],
                                    TotalPlayDurationSeconds = (int)rdr["play_duration_seconds"],
                                    
                                };
                                playerResults.Add(gameScore);
                            }
                            await rdr.CloseAsync();
                        }
                        catch (Exception Ex)
                        {
                            await connection.CloseAsync();
                            string msg = Ex.Message.ToString();
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            return playerResults;
        }
    }
}