using BusinessLayer.DTO;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers;

public static class DriverMapper
{
    public static Driver MapDtoToEntity(DriverInfo di)
    {
        return DomainFactory.CreateDriver(di.DriverID, di.LastName, di.FirstName, di.NatRegNum, di.Licenses.ConvertAll(dl => (DriversLicense)Enum.Parse(typeof(DriversLicense), dl)), di.BirthDate, di.Address, di.VehicleVin, di.GasCardNum);
    }

    public static DriverInfo? MapEntityToDto(Driver d)
    {
        return (d != null) ? new DriverInfo(d.Id, d.FirstName, d.LastName, d.BirthDate, d.NatRegNumber, d.Licenses.ConvertAll(dl => dl.ToString()), d.Address, d.VIN, d.GasCardNum) : null;
    }
}
