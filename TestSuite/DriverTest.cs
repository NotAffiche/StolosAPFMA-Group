using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Model;

namespace TestSuite;
public class DriverTest {

    [Fact]
    public void TestDriver_ValidData_ValidCtor() {
        // Arrange
        string firstName = "Morgan";
        string lastName = "Freeman";
        string rrn = "69.10.11-848.50";
        List<DriversLicense> licenses = new() {
            DriversLicense.B
        };
        Driver d = DomainFactory.CreateDriver(1, lastName, firstName, rrn, licenses, new DateTime(1985, 10, 23), null);

        Assert.Equal(1, d.Id);
        Assert.Equal(rrn, d.NatRegNumber);
        Assert.Equal(firstName, d.FirstName);
        Assert.Equal(lastName, d.LastName);
        Assert.Equal(licenses, d.Licenses);
    }

    [Fact]
    public void TestDriver_ValidData_BirthdatefromRRN() {
        // Arrange
        string firstName = "Bobby";
        string lastName = "Tables";
        string rrn = "69.10.11-848.50";
        List<DriversLicense> licenses = new() {
            DriversLicense.B
        };
        DateTime geboortedatum = new DateTime(1985, 10, 23);
        Driver d = DomainFactory.CreateDriver(1, lastName, firstName, rrn, licenses, new DateTime(1985, 10, 23), null);

        Assert.Equal(geboortedatum, d.BirthDate);
    }

    [Fact]
    public void TestDriver_NullValuesForFields_ThrowExcepion() {
        Assert.Throws<DomainException>(() => DomainFactory.CreateDriver(1, null, null, null, null, default(DateTime), null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void TestDriver_InvalidId_ThrowException(int id) {
        List<DriversLicense> licenses = new List<DriversLicense>();
        licenses.Add(DriversLicense.B);
        Assert.Throws<DomainException>(() => DomainFactory.CreateDriver(id, "voornaam", "achternaam", "69.10.11-848.50", licenses, new DateTime(1985, 10, 23), null));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("    ", "Bobby")]
    [InlineData("Mallory", "   ")]
    [InlineData("Bob", "\t")]
    [InlineData("Alice", "\n")]
    [InlineData("Alice", null)]
    public void TestDriver_InvalidNames_ThrowException(string firstname, string lastname) {
        List<DriversLicense> licenses = new List<DriversLicense>();
        licenses.Add(DriversLicense.B);
        Assert.Throws<DomainException>(() => DomainFactory.CreateDriver(1, firstname, lastname, "69.10.11-848.50", licenses, new DateTime(1985, 10, 23), null));
    }

    [Fact]
    public void TestDriver_InvalidBirthDateToRRN_ThrowException() {
        DateTime geboortedatum = new DateTime(1976, 10, 23);
        List<DriversLicense> licenses = new List<DriversLicense>();
        licenses.Add(DriversLicense.B);
        Assert.Throws<DomainException>(() => DomainFactory.CreateDriver(1, "voornaam", "achternaam", "75.10.23-059.39", licenses, geboortedatum, null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("0.0.0-000.00")]
    public void TestDriver_InvalidRRN_ThrowException(string rrn) {
        List<DriversLicense> licenses = new List<DriversLicense>();
        licenses.Add(DriversLicense.B);
        Assert.Throws<DomainException>(() => DomainFactory.CreateDriver(1, "voornaam", "achternaam", rrn, licenses, new DateTime(1985, 10, 23), null));
    }
}
