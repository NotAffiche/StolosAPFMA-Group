using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.Exceptions;

namespace BusinessLayer.Model;

public class Driver
{
    #region prop/model attrib
    private int? _id;
    private string _lastName;
    private string _firstName;
    private DateTime _birthDate;
    private string _natRegNumber;
    #endregion

    #region prop
    public int? Id 
    {
        get { return _id; } 
        set 
        {
            if (value != null && value < 1) throw new DomainException("Driver: set-Id: Invalid value (val < 1)");
            _id = value;
        }
    }
    public string LastName 
    {
        get { return _lastName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Driver: set-LastName: NULL value;");
            _lastName = value;
        } 
    }
    public string FirstName 
    {
        get { return _firstName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Driver: set-FirstName: NULL value");
            _firstName = value;
        }
    }
    public string? Address { get; set; }
    public DateTime BirthDate 
    {
        get { return _birthDate; }
        set
        {
            if (value == default(DateTime) || value > DateTime.Now) throw new DomainException("Driver: set-BirthDate: Incorrect value");
            _birthDate = value;
        }
    }
    public string NatRegNumber 
    {
        get { return _natRegNumber; }
        set
        {
            #region info NatRegNum
            /*
            RRN controle ->
            yy.MM.dd-iii.cc
            y=year, M=month, d=day, i=id, c=control

            M may be incremented by 20 or 40 in case of BIS nr (person not in RR but data must be saved for social security, ex: foreign workers < 3 months in BE / border workers)
            +40 in case gender known at date of application, +20 if gender unkown

            M=d -> 00 if person = refugee and birth date not known

            i = even for women, uneven for men (daycounter of births) => men (001-997), women (002-998)

            c = control num based op 9 previous digits, gets calculated by concatenating all individual date parts + daycounter, that modulating by 97, subtracting modulated res from 97 = control num
            (97 - ({birth date + day counter} % 97))

            for people born in 2000 or after, must add a 2 before modulating (+ 2000000000)
            Example: A man is born op 1 feb 1990, possible RRN/NatRegNum 90.02.01-997-04. 900201997 % 97 = 93. 97-93 = 04. RRN is correct.

            !!! IMPORTANT !!!
            For the scope of this project: only validation is required. A more elaborated version was created but was dropped due to time restrains and for simplicity's sake.
            Old_NatRegNumCheck
             */
            #endregion
            try
            {
                if (!SimpleNatRegNumCheck(value)) { throw new DomainException("Driver: Set-NatRegNumber: Not Valid Control Num"); }
                _natRegNumber = value;
            }
            catch (Exception ex)
            {
                throw new DomainException("Driver: Set-NatRegNumber: Incorrect value", ex);
            }
        }
    }
    public List<DriversLicense> Licenses { get; set; }
    public string? VIN { get; set; }
    public string? GasCardNum { get; set; }
    #endregion

    #region ctor
    internal Driver(int? id, string lastName, string firstName, string natRegNumber, List<DriversLicense> licenses, DateTime? birthDate = null, string? address = null, string? vin = null, string? gasCardNum = null)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        Licenses = licenses;

        NatRegNumber = natRegNumber;
        BirthDate = (birthDate is null) ? GetDateFromRRN(NatRegNumber) : (DateTime)birthDate;
        if (!IsMatchingBirthDateAndRRN(NatRegNumber)) throw new DomainException("BirthDate and RRN do not match!");
        Address = address;
        VIN = vin;
        GasCardNum = gasCardNum;
    }
    #endregion

    #region methods
    private DateTime GetDateFromRRN(string natRegNum)
    {
        #region declarations
        string fullDate = natRegNum.Split("-")[0];
        string idAndControl = natRegNum.Split("-")[1];
        string year = fullDate.Split(".")[0];
        string month = fullDate.Split(".")[1];
        string day = fullDate.Split(".")[2];
        string id = idAndControl.Split(".")[0];
        string control = idAndControl.Split('.')[1];
        int yearNum = int.Parse(year);
        int monthNum = int.Parse(month);
        int dayNum = int.Parse(day);
        int idNum = int.Parse(id);
        int controlNum = int.Parse(control);
        #endregion
        if (monthNum < 1 || monthNum > 12) throw new DomainException("Driver: GetDateFromRRN: Invalid month");
        if (dayNum < 1 || dayNum > 31) throw new DomainException("Driver: GetDateFromRRN: Invalid day");
        bool valid = false;
        string allButControl = year + month + day + id;
        uint res = (uint)(97 - (int.Parse(allButControl) % 97));
        if (controlNum == (int)res) valid = true;
        string yearBeginning = "19";
        if (!valid)
        {
            const uint magicNum = 2000000000; // gebruikt voor mensen die in/na 2000 zijn geboren
            res = 97 - ((magicNum + uint.Parse(allButControl)) % 97);
            if (controlNum == (int)res) valid = true;
            yearBeginning = "20";
        }
        return new DateTime(int.Parse(yearBeginning + year), monthNum, dayNum);
    }

    private bool SimpleNatRegNumCheck(string natRegNum)
    {
        #region declarations
        bool valid = false;
        string fullDate = natRegNum.Split("-")[0];
        string idAndControl = natRegNum.Split("-")[1];
        string year = fullDate.Split(".")[0];
        string month = fullDate.Split(".")[1];
        string day = fullDate.Split(".")[2];
        string id = idAndControl.Split(".")[0];
        string control = idAndControl.Split('.')[1];
        int yearNum = int.Parse(year);
        int monthNum = int.Parse(month);
        int dayNum = int.Parse(day);
        int idNum = int.Parse(id);
        int controlNum = int.Parse(control);
        string allButControl = year + month + day + id;
        #endregion
        uint res = (uint)(97 - (int.Parse(allButControl) % 97));
        if (controlNum == (int)res) valid = true;
        if (!valid)
        {
            const uint magicNum = 2000000000; // gebruikt voor mensen die in/na 2000 zijn geboren
            res = 97 - ((magicNum + uint.Parse(allButControl)) % 97);
            if (controlNum == (int)res) valid = true;
        }
        return valid;
    }

    private bool IsMatchingBirthDateAndRRN(string natRegNum)
    {
        DateTime dtFromRRN = GetDateFromRRN(natRegNum);

        DateOnly doFromRRN = new DateOnly(dtFromRRN.Year, dtFromRRN.Month, dtFromRRN.Day);
        DateOnly doFromBD = new DateOnly(BirthDate.Year, BirthDate.Month, BirthDate.Day);
        return doFromRRN.Equals(doFromBD);
    }
    #endregion

    #region OLD
    //private bool Old_NatRegNumCheck(string natRegNum, out bool valid, out bool pre, out string sexe, out DateTime correctDate)
    //{
    //    /*
    //     * Adrian B on 19/03
    //     */

    //    if (string.IsNullOrWhiteSpace(natRegNum)) throw new DomainException("Driver: NatRegNumber: NULL value");
    //    if (!Regex.Match(natRegNum, @"(\d{2})\.(\d{2})\.(\d{2})-(\d{3})\.(\d{2})").Success) throw new DomainException("Driver: NatRegNum: Incorrect format");

    //    string fullDate = natRegNum.Split("-")[0];
    //    string idAndControl = natRegNum.Split("-")[1];

    //    string year = fullDate.Split(".")[0];
    //    string month = fullDate.Split(".")[1];
    //    string day = fullDate.Split(".")[2];

    //    string id = idAndControl.Split(".")[0];
    //    string control = idAndControl.Split('.')[1];

    //    int yearNum = int.Parse(year);
    //    int monthNum = int.Parse(month);
    //    int dayNum = int.Parse(day);

    //    int idNum = int.Parse(id);
    //    int controlNum = int.Parse(control);

    //    int monthRes = 0; // 1 = refugee, 2 = BIS gender unkown, 3 = standard, 4 = BIS gender known

    //    //month check => refugee, in RR, BIS (gender (un)known) of incorrect data entry
    //    switch (monthNum)
    //    {
    //        case 0:
    //            if (dayNum == 0)
    //            {
    //                // refugee with unknown birth date
    //                monthRes = 1;

    //                break;
    //            }
    //            throw new DomainException("Driver: NatRegNumber: Refugee error");
    //        case int M when M >= 1 && M <= 12:
    //            // standard date = person in RR
    //            monthRes = 3;
    //            break;
    //        case int M when M >= 21 && M <= 32:
    //            // non standard date -> +20 = gender unknown
    //            monthRes = 2;
    //            break;
    //        case int M when M >= 41 && M <= 52:
    //            // non standard date -> +40 = gender known
    //            monthRes = 4;
    //            break;
    //        //incorrect entry
    //        default: throw new DomainException("Driver: NatRegNumber: Incorrect month");

    //    }
    //    switch (monthRes)
    //    {
    //        case 1:
    //        case 2:
    //            // gender unkown (refugee/BIS gender unknown)
    //            sexe = "onbekend";
    //            break;
    //        case 3:
    //        case 4:
    //            // gender known
    //            sexe = dayNum % 2 == 0 ? "vrouw" : "man";
    //            break;
    //        //incorrect data entry
    //        default: throw new DomainException("Driver: NatRegNumber: Incorrect ID");
    //    }

    //    valid = false;
    //    pre = true;
    //    //control check double -> once in case pre 2000, 2nd time incase 2000 or later
    //    string allButControl = year + month + day + id;
    //    uint res = (uint)(97 - (int.Parse(allButControl) % 97));
    //    if (controlNum == (int)res) valid = true;
    //    if (!valid)
    //    {
    //        const uint magicNum = 2000000000; // gebruikt voor mensen die in/na 2000 zijn geboren
    //        res = 97 - ((magicNum + uint.Parse(allButControl)) % 97);
    //        if (controlNum == (int)res) valid = true;
    //        pre = false;
    //    }

    //    correctDate = DateTime.UnixEpoch;
    //    string yearBeginning = (pre == true ? "19" : "20");
    //    int x = 0;
    //    switch (monthRes)
    //    {
    //        case 2:
    //            x = -20;
    //            break;
    //        case 4:
    //            x = -40;
    //            break;
    //    }
    //    correctDate = new DateTime(int.Parse(yearBeginning + year), monthNum + x, dayNum);

    //    return valid;
    //}
    #endregion

}
