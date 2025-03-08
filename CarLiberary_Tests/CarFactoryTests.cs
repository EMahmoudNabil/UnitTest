using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryLibrary;

namespace CarLiberary_Tests
{
    public class CarFactoryTests
    {
        public void NewCar_AskForHonda_ThrowNotImplementedEx()
        {
           
            Assert.Throws<NotImplementedException>(() => {
                // act
                Car? car = CarFactory.NewCar(CarTypes.Honda);
            });
        }
        [Fact]
        public void NewCar_AskForBMW_ObjectOfBMW()
        {
         

            // act
            Car? Car = CarFactory.NewCar(CarTypes.BMW);

            // assert
           
            Assert.IsType<BMW>(Car);
        }
    }
}
