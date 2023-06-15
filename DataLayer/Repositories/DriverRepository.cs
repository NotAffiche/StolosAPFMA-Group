using BusinessLayer;
using BusinessLayer.DTO;
using BusinessLayer.Interfaces;
using BusinessLayer.Mappers;
using BusinessLayer.Model;
using DataLayer.Exceptions;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories;

public class DriverRepository : IDriverRepository
{
    #region priv attrib
    private string _connectionString;
    #endregion

    #region ctor
    public DriverRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    #endregion

    #region get
    public List<Driver> GetAllDrivers()
    {
        List<Driver> drivers = new();
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))

            {
                conn.Open();

                cmd = new("SELECT d.DriverID, d.FirstName, d.LastName, d.Address, d.BirthDate, d.NationalRegistrationNumber, d.DriversLicenses, v.VIN, v.LicensePlate, gc.CardNumber " +
                    "FROM GasCard gc RIGHT JOIN Driver d ON gc.DriverID=d.DriverID LEFT JOIN Vehicle v ON v.DriverID = d.DriverID WHERE d.Deleted=0 ORDER BY d.FirstName ASC;", conn);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //driver
                        int id = (int)reader["DriverID"];
                        string fName = (string)reader["FirstName"];
                        string lName = (string)reader["LastName"];
                        string? address = (string?)((reader["Address"] is DBNull) ? null : reader["Address"]);
                        DateTime birthDate = (DateTime)reader["BirthDate"];
                        string natRegNum = (string)reader["NationalRegistrationNumber"];

                        List<DriversLicense> licenseList = new List<DriversLicense>(reader["DriversLicenses"].ToString().Split(",").Select(dl => (DriversLicense)Enum.Parse(typeof(DriversLicense), dl)));
                        string? vin = null, gcNum = null;

                        vin = ((reader["VIN"] is not DBNull) ? (string?)reader["VIN"] : null);
                        gcNum = ((reader["CardNumber"] is not DBNull) ? (string?)reader["CardNumber"] : null);
                        Driver d = DomainFactory.CreateDriver(id, lName, fName, natRegNum, licenseList, birthDate, address, vin, gcNum);
                        drivers.Add(d);
                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-GetAllDrivers", ex);
        }
        return drivers;
    }

    public Driver GetDriverById(int driverId)
    {
        Driver? d = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT d.DriverID, d.FirstName, d.LastName, d.Address, d.BirthDate, d.NationalRegistrationNumber, d.DriversLicenses, v.VIN, v.LicensePlate, gc.CardNumber " +
                    "FROM GasCard gc RIGHT JOIN Driver d ON gc.DriverID=d.DriverID LEFT JOIN Vehicle v ON v.DriverID = d.DriverID WHERE d.Deleted=0 AND d.DriverID = @id;", conn);
                cmd.Parameters.AddWithValue("@id", driverId);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //driver
                        int id = (int)reader["DriverID"];
                        string fName = (string)reader["FirstName"];
                        string lName = (string)reader["LastName"];
                        string? address = (string?)((reader["Address"] is DBNull) ? null : reader["Address"]);
                        DateTime birthDate = (DateTime)reader["BirthDate"];
                        string natRegNum = (string)reader["NationalRegistrationNumber"];
                        List<DriversLicense> licenseList = new List<DriversLicense>(reader["DriversLicenses"].ToString().Split(",").Select(dl => (DriversLicense)Enum.Parse(typeof(DriversLicense), dl)));
                        string? vin = null, cardNum = null;

                        vin = ((reader["VIN"] is not DBNull) ? (string?)reader["VIN"] : null);
                        cardNum = ((reader["CardNumber"] is not DBNull) ? (string?)reader["CardNumber"] : null);
                        d = DomainFactory.CreateDriver(id, lName, fName, natRegNum, licenseList, birthDate, address, vin, cardNum);
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-GetDriver", ex);
        }
        return d;
    }

    private Driver GetDriverByExistingDriverParam(Driver d)
    {
        Driver? driver = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT d.DriverID, d.FirstName, d.LastName, d.Address, d.BirthDate, d.NationalRegistrationNumber, d.DriversLicenses, v.VIN, v.LicensePlate, gc.CardNumber " +
                    "FROM GasCard gc RIGHT JOIN Driver d ON gc.DriverID=d.DriverID LEFT JOIN Vehicle v ON v.DriverID = d.DriverID WHERE d.NationalRegistrationNumber = @rrn;", conn);
                cmd.Parameters.AddWithValue("@rrn", d.NatRegNumber);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //driver
                        int id = (int)reader["DriverID"];
                        string fName = (string)reader["FirstName"];
                        string lName = (string)reader["LastName"];
                        string? address = (string?)((reader["Address"] is DBNull) ? null : reader["Address"]);
                        DateTime birthDate = (DateTime)reader["BirthDate"];
                        string natRegNum = (string)reader["NationalRegistrationNumber"];
                        List<DriversLicense> licenseList = new List<DriversLicense>(reader["DriversLicenses"].ToString().Split(",").Select(dl => (DriversLicense)Enum.Parse(typeof(DriversLicense), dl)));

                        string? vin = null, cardNum = null;

                        vin = ((reader["VIN"] is not DBNull) ? (string?)reader["VIN"] : null);
                        cardNum = ((reader["CardNumber"] is not DBNull) ? (string?)reader["CardNumber"] : null);

                        driver = DomainFactory.CreateDriver(id, lName, fName, natRegNum, licenseList, birthDate, address, vin, cardNum);
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-GetDriver", ex);
        }
        return driver;
    }
    #endregion

    #region post
    public void AddDriver(Driver d)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                bool existingButDeleted = ((GetDriverByExistingDriverParam(d) != null) ? true : false);//atm still gives 200 when you try to add an already added but not deleted driver

                string sql = (existingButDeleted ? "UPDATE Driver SET FirstName=@fn, LastName=@ln, Address=@ad, BirthDate=@bd, NationalRegistrationNumber=@rrn, DriversLicenses=@dls, Deleted=@del WHERE NationalRegistrationNumber=@rrn;" 
                    : "INSERT INTO Driver(FirstName, LastName, Address, BirthDate, NationalRegistrationNumber, DriversLicenses, Deleted) VALUES (@fn, @ln, @ad, @bd, @rrn, @dls, @del);");
                cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@fn", d.FirstName);
                cmd.Parameters.AddWithValue("@ln", d.LastName);
                cmd.Parameters.AddWithValue("@ad", d.Address);
                cmd.Parameters.AddWithValue("@bd", d.BirthDate);
                cmd.Parameters.AddWithValue("@rrn", d.NatRegNumber);
                cmd.Parameters.AddWithValue("@dls", string.Join(",", d.Licenses.ConvertAll(dl => dl.ToString())));
                cmd.Parameters.AddWithValue("@del", 0);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Driver createdDriver = GetDriverByExistingDriverParam(d);

                if (d.VIN != null)
                {
                    cmd = new("UPDATE Vehicle SET DriverID=@did WHERE VIN=@vin;", conn);
                    cmd.Parameters.AddWithValue("@vin", d.VIN);
                }
                else
                {
                    cmd = new("UPDATE Vehicle SET DriverID=NULL WHERE DriverID=@did;", conn);
                }
                cmd.Parameters.AddWithValue("@did", createdDriver.Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (d.GasCardNum != null)
                {
                    cmd = new("UPDATE GasCard SET DriverID=@did WHERE CardNumber=@gc;", conn);
                    cmd.Parameters.AddWithValue("@gc", d.GasCardNum);
                }
                else
                {
                    cmd = new("UPDATE GasCard SET DriverID=NULL WHERE DriverID=@did;", conn);
                }
                cmd.Parameters.AddWithValue("@did", createdDriver.Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-AddDriver", ex);
        }
    }
    #endregion

    #region put
    public void UpdateDriver(Driver d)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE Driver SET FirstName=@fn,LastName=@ln,Address=@ad,BirthDate=@bd,NationalRegistrationNumber=@rrn,DriversLicenses=@dls WHERE DriverID=@did;", conn);

                cmd.Parameters.AddWithValue("@did", d.Id);

                cmd.Parameters.AddWithValue("@fn", d.FirstName);
                cmd.Parameters.AddWithValue("@ln", d.LastName);
                cmd.Parameters.AddWithValue("@ad", d.Address);
                cmd.Parameters.AddWithValue("@bd", d.BirthDate);
                cmd.Parameters.AddWithValue("@rrn", d.NatRegNumber);
                cmd.Parameters.AddWithValue("@dls", string.Join(",", d.Licenses.ConvertAll(dl => dl.ToString())));

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (d.VIN!=null)
                {
                    cmd = new("UPDATE Vehicle SET DriverID=NULL WHERE DriverID=@did;", conn);
                    cmd.Parameters.AddWithValue("@did", d.Id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd = new("UPDATE Vehicle SET DriverID=@did WHERE VIN=@vin;", conn);
                    cmd.Parameters.AddWithValue("@vin", d.VIN);
                }
                else
                {
                    cmd = new("UPDATE Vehicle SET DriverID=NULL WHERE DriverID=@did;", conn);
                }
                cmd.Parameters.AddWithValue("@did", d.Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (d.GasCardNum!=null)
                {
                    cmd = new("UPDATE GasCard SET DriverID=NULL WHERE DriverID=@did;", conn);
                    cmd.Parameters.AddWithValue("@did", d.Id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd = new("UPDATE GasCard SET DriverID=@did WHERE CardNumber=@gc;", conn);
                    cmd.Parameters.AddWithValue("@gc", d.GasCardNum);
                }
                else
                {
                    cmd = new("UPDATE GasCard SET DriverID=NULL WHERE DriverID=@did;", conn);
                }
                cmd.Parameters.AddWithValue("@did", d.Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-UpdateDriver", ex);
        }
    }
    #endregion

    #region delete (soft)
    public void DeleteDriver(int id)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE Driver SET Deleted=1 WHERE DriverID=@did;", conn);
                cmd.Parameters.AddWithValue("@did", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = new("UPDATE Vehicle SET DriverID=NULL WHERE DriverID=@did;", conn);
                cmd.Parameters.AddWithValue("@did", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = new("UPDATE GasCard SET DriverID=NULL WHERE DriverID=@did;", conn);
                cmd.Parameters.AddWithValue("@did", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("DriverRepo-DeleteDriver", ex);
        }
    }
    #endregion
}
