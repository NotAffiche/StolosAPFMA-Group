using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exceptions;

namespace BusinessLayer.Model;

public class Vehicle
{
    private string _vinNumber;
    private string _licensePlate;
    private string _brandModel;

    public Vehicle(string vinNumber, string brandModel, string licensePlate, VehicleType category, FuelType fuel, string? color = null, int? doors = null, int? driverID = null)
    {
        VinNumber = vinNumber;
        LicensePlate = licensePlate;
        BrandModel = brandModel;
        Category = category;
        Fuel = fuel;
        Color = color;
        Doors = doors;
        DriverID = driverID;
    }

    public string VinNumber 
    { 
        get { return _vinNumber; } 
        set 
        {
            if (value == null) throw new DomainException("Vehicle: set-VinNumber: NULL value");
            _vinNumber = value; 
        } 
    }
    public string LicensePlate 
    { 
        get { return _licensePlate; } 
        set 
        { 
            if (value == null) throw new DomainException("Vehicle: set-LicensePlate: NULL value"); 
            _licensePlate = value; 
        } 
    }
    public string BrandModel 
    {
        get { return _brandModel; }
        set { if (value == null) throw new DomainException("Vehicle: set-BrandModel: NULL value");
            _brandModel = value;
        } 
    }
    public VehicleType Category { get; set; }
    public FuelType Fuel { get; set; }
    public string? Color { get; set; }
    public int? Doors { get; set; }

    //driver ref
    public int? DriverID { get; set; }
}
