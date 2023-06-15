using BusinessLayer;
using BusinessLayer.DTO;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using DataLayer.Exceptions;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories;

/*
 * Adrian B on 17/03
 */
public class VehicleRepository : IVehicleRepository
{
    #region attrib
    private string _connectionString;
    #endregion

    #region ctor
    public VehicleRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    #endregion

    #region get
    public List<Vehicle> GetAllVehicles()
    {
        List<Vehicle> vehicles = new();
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT * FROM Vehicle v LEFT JOIN Driver d ON v.DriverID=d.DriverID WHERE v.Deleted=0;", conn);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string vinDB = (string)reader["VIN"];
                        string brandModel = (string)reader["BrandModel"];
                        string plate = (string)reader["LicensePlate"];
                        FuelType fuelType = (FuelType)Enum.Parse(typeof(FuelType), (string)reader["FuelType"]);
                        VehicleType vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), (string)reader["VehicleType"]);
                        string? color = (string?)((reader["Color"] is DBNull) ? "" : reader["Color"]);
                        int? doors = (int?)((reader["Doors"] is DBNull) ? null : reader["Doors"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        Vehicle v = DomainFactory.CreateVehicle(vinDB, brandModel, plate, vehicleType, fuelType, color, doors, dId);

                        vehicles.Add(v);
                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-GetAllVehicles", ex);
        }
        return vehicles;
    }

    public Vehicle GetVehicleByVIN(string vin)
    {
        Vehicle? v = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT * FROM Vehicle v LEFT JOIN Driver d ON v.DriverID=d.DriverID WHERE VIN = @vin AND v.Deleted=0;", conn);
                cmd.Parameters.AddWithValue("@vin", vin);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string vinDB = (string)reader["VIN"];
                        string brandModel = (string)reader["BrandModel"];
                        string plate = (string)reader["LicensePlate"];
                        FuelType fuelType = (FuelType)Enum.Parse(typeof(FuelType), (string)reader["FuelType"]);
                        VehicleType vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), (string)reader["VehicleType"]);
                        string? color = (string?)((reader["Color"] is DBNull) ? "" : reader["Color"]);
                        int? doors = (int?)((reader["Doors"] is DBNull) ? null : reader["Doors"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        v = DomainFactory.CreateVehicle(vinDB, brandModel, plate, vehicleType, fuelType, color, doors, dId);

                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-GetVehicle", ex);
        }
        return v;
    }

    private Vehicle GetDeletedVehicleByExistingParam(Vehicle v)
    {
        Vehicle? veh = null;
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("SELECT * FROM Vehicle v LEFT JOIN Driver d ON v.DriverID=d.DriverID WHERE VIN = @vin AND v.Deleted=1;", conn);
                cmd.Parameters.AddWithValue("@vin", v.VinNumber);

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string vinDB = (string)reader["VIN"];
                        string brandModel = (string)reader["BrandModel"];
                        string plate = (string)reader["LicensePlate"];
                        FuelType fuelType = (FuelType)Enum.Parse(typeof(FuelType), (string)reader["FuelType"]);
                        VehicleType vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), (string)reader["VehicleType"]);
                        string? color = (string?)((reader["Color"] is DBNull) ? "" : reader["Color"]);
                        int? doors = (int?)((reader["Doors"] is DBNull) ? null : reader["Doors"]);

                        int? dId = (reader["DriverID"] is not DBNull) ? (int?)reader["DriverID"] : null;

                        veh = DomainFactory.CreateVehicle(vinDB, brandModel, plate, vehicleType, fuelType, color, doors, dId);

                    }
                    reader.Close();
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-GetVehicle", ex);
        }
        return veh;
    }
    #endregion

    #region post
    public void AddVehicle(Vehicle vehicle)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                bool existingButDeleted = ((GetDeletedVehicleByExistingParam(vehicle) != null) ? true : false);

                string sql = (existingButDeleted ? "UPDATE Vehicle SET VIN=@vin, BrandModel=@bm, LicensePlate=@lp, FuelType=@ft, VehicleType = @vt, Color=@clr, Doors=@drs, DriverID=@did, Deleted=@del WHERE VIN=@vin;"
                    : "INSERT INTO Vehicle (VIN, BrandModel, LicensePlate, FuelType, VehicleType, Color, Doors, DriverID, Deleted) VALUES (@vin, @bm, @lp, @ft, @vt, @clr, @drs, @did, @del);");

                cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@vin", vehicle.VinNumber);
                cmd.Parameters.AddWithValue("@bm", vehicle.BrandModel);
                cmd.Parameters.AddWithValue("@lp", vehicle.LicensePlate);
                cmd.Parameters.AddWithValue("@ft", vehicle.Fuel.ToString());
                cmd.Parameters.AddWithValue("@vt", vehicle.Category.ToString());
                cmd.Parameters.AddWithValue("@clr", vehicle.Color);
                cmd.Parameters.AddWithValue("@drs", vehicle.Doors);
                cmd.Parameters.AddWithValue("@did", vehicle.DriverID);
                cmd.Parameters.AddWithValue("@del", 0);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-AddVehicle", ex);
        }
    }
    #endregion

    #region put
    public void UpdateVehicle(Vehicle v)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE Vehicle SET BrandModel=@bm, LicensePlate=@lp, VehicleType=@vt, FuelType=@ft, Color=@clr, Doors=@drs, DriverID=@did WHERE VIN=@vin;", conn);

                cmd.Parameters.AddWithValue("@vin", v.VinNumber);
                cmd.Parameters.AddWithValue("@bm", v.BrandModel);
                cmd.Parameters.AddWithValue("@lp", v.LicensePlate);
                cmd.Parameters.AddWithValue("@ft", v.Fuel.ToString());
                cmd.Parameters.AddWithValue("@vt", v.Category.ToString());
                cmd.Parameters.AddWithValue("@clr", v.Color);
                cmd.Parameters.AddWithValue("@drs", v.Doors);
                cmd.Parameters.AddWithValue("@did", v.DriverID);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-AddVehicle", ex);
        }
    }
    #endregion

    #region delete (soft)
    public void DeleteVehicle(string vin)
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        try
        {
            using (conn = new(_connectionString))
            {
                conn.Open();

                cmd = new("UPDATE Vehicle SET Deleted=1, DriverID=NULL WHERE VIN=@vin;", conn);
                cmd.Parameters.AddWithValue("@vin", vin);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            throw new DataException("VehicleRepo-DeleteVehicle", ex);
        }
    }
    #endregion
}
