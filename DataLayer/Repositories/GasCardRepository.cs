using BusinessLayer;
using BusinessLayer.DTO;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories;

public class GasCardRepository : IGasCardRepository
{
    #region priv attrib
    public string _connectionString;
    #endregion

    #region ctor
    public GasCardRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    #endregion

    #region get
    public GasCard GetGasCard(string cardNum)
    {
        GasCard? gc = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;

        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT * FROM GasCard gc LEFT JOIN Driver d ON gc.DriverID=d.DriverID WHERE CardNumber = @cn AND gc.Deleted=0;", conn);
                cmd.Parameters.AddWithValue("@cn", cardNum);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cardnumber = (string)reader["CardNumber"];
                        DateTime expiringdate = (DateTime)reader["ExpiringDate"];
                        int? pincode = (int?)((reader["Pincode"] is DBNull) ? null : reader["Pincode"]);
                        List<FuelType> fuels = new List<FuelType>(reader["FuelTypes"].ToString().Split(",").Select(ft => (FuelType)Enum.Parse(typeof(FuelType), ft)));
                        bool blocked = Convert.ToBoolean(reader["Blocked"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        gc = DomainFactory.CreateGasCard(cardnumber, expiringdate, pincode, fuels, blocked, dId);
                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-GetGasCard", ex);
        }

        return gc;
    }

    public List<GasCard> GetAllGasCards()
    {
        List<GasCard> gasCards = new List<GasCard>();
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))

            {
                conn.Open();

                cmd = new("SELECT * FROM GasCard gc LEFT JOIN Driver d ON gc.DriverID=d.DriverID WHERE gc.Deleted=0;", conn);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cardnumber = (string)reader["CardNumber"];
                        DateTime expiringdate = (DateTime)reader["ExpiringDate"];
                        int? pincode = (int?)((reader["Pincode"] is DBNull) ? null : reader["Pincode"]);
                        List<FuelType> fuels = new List<FuelType>(reader["FuelTypes"].ToString().Split(",").Select(ft => (FuelType)Enum.Parse(typeof(FuelType), ft)));
                        bool blocked = Convert.ToBoolean(reader["Blocked"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        GasCard gc = DomainFactory.CreateGasCard(cardnumber, expiringdate, pincode, fuels, blocked, dId);

                        gasCards.Add(gc);
                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-GetAllGasCards", ex);
        }
        return gasCards;
    }

    //
    private GasCard GetDeletedGasCardByExistingParam(GasCard gc)
    {
        GasCard? card = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();
                cmd = new("SELECT * FROM GasCard gc LEFT JOIN Driver d ON gc.DriverID=d.DriverID WHERE CardNumber = @cn AND gc.Deleted=1;", conn);
                cmd.Parameters.AddWithValue("@cn", gc.CardNumber);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cardnumber = (string)reader["CardNumber"];
                        DateTime expiringdate = (DateTime)reader["ExpiringDate"];
                        int? pincode = (int?)((reader["Pincode"] is DBNull) ? null : reader["Pincode"]);
                        List<FuelType> fuels = new List<FuelType>(reader["FuelTypes"].ToString().Split(",").Select(ft => (FuelType)Enum.Parse(typeof(FuelType), ft)));
                        bool blocked = Convert.ToBoolean(reader["Blocked"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        card = DomainFactory.CreateGasCard(cardnumber, expiringdate, pincode, fuels, blocked, dId);
                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-GetGCbyParam", ex);
        }
        return card;
    }
    #endregion

    #region post
    public void AddGasCard(GasCard gc)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                bool existingButDeleted = (GetDeletedGasCardByExistingParam(gc) != null ? true : false);

                string sql = (existingButDeleted ? "UPDATE GasCard SET CardNumber = @cn, ExpiringDate = @ed, Pincode = @pc, FuelTypes = @ft, DriverID = @did, Blocked=@bl, Deleted=@del WHERE CardNumber = @cn" 
                    : "INSERT INTO GasCard (CardNumber, ExpiringDate, Pincode, FuelTypes, DriverID, Blocked, Deleted) VALUES (@cn, @ed, @pc, @ft, @did, @bl, @del);");

                cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("@cn", gc.CardNumber);
                cmd.Parameters.AddWithValue("@ed", gc.ExpiringDate);
                cmd.Parameters.AddWithValue("@pc", gc.Pincode);
                cmd.Parameters.AddWithValue("@ft", string.Join(",", gc.Fuel.ConvertAll(f => f.ToString())));
                cmd.Parameters.AddWithValue("@did", gc.DriverID);
                cmd.Parameters.AddWithValue("@bl", gc.Blocked);
                cmd.Parameters.AddWithValue("@del", 0);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-AddGasCard", ex);
        }
    }
    #endregion

    #region put
    public void UpdateGasCard(GasCard gc)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE GasCard SET CardNumber = @cn, ExpiringDate = @ed, Pincode = @pc, FuelTypes = @ft, DriverID = @did, Blocked=@bl, Deleted=@del WHERE CardNumber = @cn", conn);

                cmd.Parameters.AddWithValue("@cn", gc.CardNumber);
                cmd.Parameters.AddWithValue("@ed", gc.ExpiringDate);
                cmd.Parameters.AddWithValue("@pc", gc.Pincode);
                cmd.Parameters.AddWithValue("@ft", string.Join(",", gc.Fuel.ConvertAll(f => f.ToString())));
                cmd.Parameters.AddWithValue("@did", gc.DriverID);
                cmd.Parameters.AddWithValue("@bl", gc.Blocked);
                cmd.Parameters.AddWithValue("@del", 0);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-UpdateGasCard", ex);
        }
    }
    #endregion

    #region delete (soft)
    public void DeleteGasCard(string cn)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE GasCard SET Deleted=1, DriverID=NULL WHERE CardNumber=@cn;", conn);
                cmd.Parameters.AddWithValue("@cn", cn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("GasCardRepo-DeleteGasCard", ex);
        }
    }
    #endregion
}
