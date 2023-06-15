using BusinessLayer.DTO;
using BusinessLayer.Exceptions;
using BusinessLayer.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer;

public class DomainFactory
{
    public static Driver CreateDriver(int? id, string lname, string fname, string natRegNum, List<DriversLicense> licenses, DateTime birthDate, string? address = null, string? vin = null, string? gcNum = null)//CreateDriver(DriverInfo di)
    {
        try
        {
            return new Driver(id, lname, fname, natRegNum, licenses, birthDate, address, vin, gcNum);        }
        catch (Exception ex)
        {
            throw new DomainException("DomainFactory - NewDriver", ex);
        }
    }

    public static Vehicle CreateVehicle(string vin, string brandModel, string plate, VehicleType vehicleType, FuelType fuelType, string? color, int? doors, int? driverId)
    {
        try
        {
            return new Vehicle(vin, brandModel, plate, vehicleType, fuelType, color, doors, driverId);
        }
        catch (Exception ex)
        {
            throw new DomainException("DomainFactory - NewVehicle", ex);
        }
    }

    public static GasCard CreateGasCard(string cardNum, DateTime expiringDate, int? pin, List<FuelType> fuelTypes, bool blocked, int? driverId)
    {
        try
        {
            return new GasCard(cardNum, expiringDate, pin, blocked, fuelTypes, driverId);
        }
        catch (Exception ex)
        {
            throw new DomainException("DomainFactory - NewGasCard", ex);
        }
    }
}
