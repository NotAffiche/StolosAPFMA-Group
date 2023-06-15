using BusinessLayer.Exceptions;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO;

public class GasCardInfo
{
	public GasCardInfo(string cardNumber, DateTime expiringDate, int? pinCode, List<string> fuelTypes, bool blocked, int? driverId)
	{
		CardNumber = cardNumber;
		ExpiringDate = expiringDate;
		Pincode = pinCode;
		FuelTypes = fuelTypes.ConvertAll(l => l.ToString());
		Blocked = blocked;

		if (driverId != null) DriverId= driverId;
    }

    public string CardNumber { get; set; }
	public DateTime ExpiringDate { get; set; }
	public int? Pincode { get; set; }
	public List<string>? FuelTypes { get; set; }
	public bool Blocked { get; set; }

    public int? DriverId { get; set; }
}
