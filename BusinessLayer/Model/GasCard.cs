using BusinessLayer.Exceptions;
using System.Security.Cryptography;
using static BusinessLayer.Model.Vehicle;

namespace BusinessLayer.Model;

/*
 * Adrian B on 10/03
 */

public class GasCard
{
    #region attrib
    //attrib
    private string _cardNumber;
    private DateTime _expiringDate;
    private int? _pincode;
    #endregion

    #region ctor
    //ctor
    internal GasCard(string cardNumber, DateTime expiringDate, int? pincode, bool blocked, List<FuelType> fuel, int? driverID = null)
    {
        CardNumber = cardNumber;
        ExpiringDate = expiringDate;
        Pincode = pincode;
        Blocked = blocked;
        Fuel = fuel;
        DriverID = driverID;
    }
    #endregion

    #region prop
    //prop
    public string CardNumber
    {
        /*
         * must be unique: unique constraint in DB, no need to keep local list to make sure for unique val
         * DB will throw unique constraint error if double data entry, caught my Repo/RepoException
         */
        get { return _cardNumber; }
        set
        {
            if (value == null) throw new DomainException("GasCard: Set-CardNumber: NULL val");
            _cardNumber = value;
        }
    }
    public DateTime ExpiringDate
    {
        get { return _expiringDate; }
        set
        {
            //DateTime cannot be null => default val for DateTime means data entry was NULL
            if (value == default(DateTime)) throw new DomainException("GasCard: Set-DateTime: NULL val");
            _expiringDate = value;
        }
    }
    public int? Pincode
    {
        get { return _pincode; }
        set
        {
            if (value != null)
            {
                if (0 > value || 9999 < value) throw new DomainException("GasCard: Set-Pincode: Pin lower than 0 or higher than 9999");
                _pincode = value;
            }
            _pincode = value;
        }
    }
    public bool Blocked { get; set; }
    public List<FuelType> Fuel { get; set; }
    public int? DriverID { get; set; }
    #endregion
}
