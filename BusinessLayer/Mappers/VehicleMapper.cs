using BusinessLayer.DTO;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers;

public static class VehicleMapper
{
    public static Vehicle MapDtoToEntity(VehicleInfo vi)
    {
        return DomainFactory.CreateVehicle(vi.VIN, vi.BrandModel, vi.LicensePlate, (VehicleType)Enum.Parse(typeof(VehicleType), vi.VehicleType.ToString()), (FuelType)Enum.Parse(typeof(FuelType), vi.FuelType.ToString()), vi.Color, vi.Doors, vi.DriverId);
    }
    public static VehicleInfo? MapEntityToDto(Vehicle v)
    {
        return (v != null) ? new VehicleInfo(v.VinNumber, v.BrandModel, v.LicensePlate, v.Fuel.ToString(), v.Category.ToString(), v.Color, v.Doors, v.DriverID) : null;
    }
}
