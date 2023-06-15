using BusinessLayer;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite
{
    public class GasCardTest
    {
        public GasCardTest()
        {

            //Driver d = DomainFactory.CreateDriver(1, "Doe", "John", "85.10.23-059.39", DriversLicense.B, "9000 Gent", null)
        }

        [Fact]
        [InlineData("15987", 01/01/2024,"", false, FuelType.Petrol)]
        public void Test_ctor_valid()
        {
            //GasCard g = new GasCard("15978", 02/01/2024,)
        }
    }
}
