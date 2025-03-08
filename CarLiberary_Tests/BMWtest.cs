using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryLibrary;

namespace CarLiberary_Tests
{
    public class BMWtest
    {
        [Fact]
        public void ISstoped_Velocity10_False()
        {
            //arrange
            BMW bMW = new BMW();
            bMW.velocity = 10;
            //act
            bool ActualVelocity = bMW.IsStopped();
            // boolean Assert
            Assert.False(ActualVelocity);
        }
        [Fact]
        public void GetDirection_Dicrectionstopped_stopped()
        {
            //arrange
            BMW bMW = new BMW();
            bMW.drivingMode = DrivingMode.Stopped;
            //act
           string Direction= bMW.GetDirection();
            //assert
            Assert.Equal("stopped",Direction,ignoreCase:true);
            Assert.Contains("opp",Direction);
        }
    }
}
