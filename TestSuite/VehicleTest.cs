using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite
{
    public class VehicleTest
    {
        [Fact]
        public void TestVehicle_ValidData_ValidCtor()
        {
            string vin = "G2NO5C7LMNA7FHOWL";
            string licenseplate = "1-ZCN-585";
            string brandmodel = "Audi A4";
            VehicleType vtype = VehicleType.Car;
            FuelType ftype = FuelType.Petrol;
            string color = "red";
            int doors = 3;

            Vehicle v = DomainFactory.CreateVehicle(vin, brandmodel, licenseplate, vtype, ftype, color, doors, null);

            Assert.Equal(vin, v.VinNumber);
            Assert.Equal(brandmodel, v.BrandModel);
            Assert.Equal(licenseplate, v.LicensePlate);
            Assert.Equal(vtype, v.Category);
            Assert.Equal(ftype, v.Fuel);
            Assert.Equal(color, v.Color);
            Assert.Equal(doors, v.Doors);
        }

        [Fact]
        public void TestVehicle_Doors_null()
        {
            string vin = "G2NO5C7LMNA7FHOWL";
            string licenseplate = "1-ZCN-585";
            string brandmodel = "Audi A4";
            VehicleType vtype = VehicleType.Car;
            FuelType ftype = FuelType.Petrol;
            string color = "red";

            Vehicle v = DomainFactory.CreateVehicle(vin, brandmodel, licenseplate, vtype, ftype, color, null, null);

            Assert.Null(v.Doors);
        }

        [Fact]
        public void TestVehicle_Color_null()
        {
            string vin = "G2NO5C7LMNA7FHOWL";
            string licenseplate = "1-ZCN-585";
            string brandmodel = "Audi A4";
            VehicleType vtype = VehicleType.Car;
            FuelType ftype = FuelType.Petrol;
            int doors = 3;

            Vehicle v = DomainFactory.CreateVehicle(vin, brandmodel, licenseplate, vtype, ftype, null, doors, null);

            Assert.Null(v.Color);
        }
    }
}
