using BusinessLayer.DTO;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mappers;

public static class GasCardMapper
{
    public static GasCard MapDtoToEntity(GasCardInfo gci)
    {
        return DomainFactory.CreateGasCard(gci.CardNumber, gci.ExpiringDate, gci.Pincode, gci.FuelTypes.Select(f => (FuelType) Enum.Parse(typeof(FuelType), f)).ToList(), gci.Blocked, gci.DriverId);
    }
    public static GasCardInfo? MapEntityToDto(GasCard gc)
    {
        return (gc != null) ? new GasCardInfo(gc.CardNumber, gc.ExpiringDate, gc.Pincode, gc.Fuel.ConvertAll(f => f.ToString()), gc.Blocked, gc.DriverID) : null;
    }
}
