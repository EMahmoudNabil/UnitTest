using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryLibrary;

namespace CarLiberary_Tests
{
    public class CarStoreTests
    {
        [Fact]
        public void AddCar_AddBMW_ContainsBmw()
        {
            //arrange
            CarStore carStore = new CarStore();
            BMW bMW = new BMW()
            {
                drivingMode = DrivingMode.Forward,
                velocity = 50
            };
            //act
            carStore.AddCar(bMW);
            //assert
            Assert.Contains<Car>(bMW,carStore.cars);

        }
        [Fact]
        public void TimeToCoverDistance_Velocity60Distance120_Time2()
        {
            // Arrange

            BMW bMW = new BMW();

            // Act
            double actualTime = bMW.TimeToCoverDistance(120);

            //assert
            Assert.Equal(2, actualTime);

            
        }
        [Fact]  
        public void GetMyCar_IsReferTooject_SameObject()
        {
            // arrange
           BMW bmw = new() { drivingMode = DrivingMode.Forward, velocity = 10 };

            // act
            Car Refernce = bmw.GetMyCar();

            // assert
            

            Assert.Same(bmw, Refernce);

        }

    }

}
