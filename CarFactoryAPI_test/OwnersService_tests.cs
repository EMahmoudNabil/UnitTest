using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using Moq;
using Xunit.Abstractions;

namespace CarFactoryAPI_test
{
    public class OwnersService_tests:IDisposable
    {
        private readonly ITestOutputHelper helper;
        Mock<ICarsRepository> carsRepositoryMock;
        Mock<IOwnersRepository> ownersRepositoryMock;
        Mock<ICashService> cashServiceMock;
        OwnersService ownersService;
        public OwnersService_tests(ITestOutputHelper helper)
        {
            this.helper = helper;
            helper.WriteLine("test Start");
            // create mock dependancies
            carsRepositoryMock = new ();
            cashServiceMock = new();
            ownersRepositoryMock = new();

            // assign mock to owner service
            ownersService = new OwnersService(carsRepositoryMock.Object, ownersRepositoryMock.Object, cashServiceMock.Object);
        }
        [Fact]
        public void BuyCar_carIsNotExist_NotExist()
        {
            //arrange 
            Car car = null;
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            //act
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 500
            };
            string result =  ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Car doesn't exist", result);
        }
        [Fact]
        public void BuyCar_CarHasOwner_Alreadysold()
          {
            //act
            Owner owner = new() { Id =1 , Name="hossam" };
            Car car = new() { Id = 1, OwnerId = 1 , Type=CarType.Audi , Velocity=100 ,Price=1000, VIN="5050" , Owner = owner} ;
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
           

            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 500
            };
            string result = ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Already sold", result);
          }
        [Fact]
        public void Buycar_HasNoOwner_NotExist()
        {
            Owner owner = null;
            Car car = new() { Id = 1, OwnerId = 1, Type = CarType.Audi, Velocity = 100, Price = 1000, VIN = "5050", Owner = owner };
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            ownersRepositoryMock.Setup(o=>o.GetOwnerById(1)).Returns(owner);
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 500
            };
            string result = ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Owner doesn't exist", result);
        }
        [Fact]
        public void Buycar_OwnerHasCar_HasCar()
        {
            Car car = new() { Id = 1, OwnerId = 1, Type = CarType.Audi, Velocity = 100, Price = 1000, VIN = "5050"};
            Owner owner = new() { Id = 1, Name = "hossam" , Car=car};
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            ownersRepositoryMock.Setup(o => o.GetOwnerById(1)).Returns(owner);
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 500
            };
            string result = ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Already have car", result);
        }
        [Fact] 
        public void BuyCar_PriceOfCarGreaterThanOwnerCarInputAmount_InsufficientFunds()
        {
            Car car = new() { Id = 1, OwnerId = 1, Type = CarType.Audi, Velocity = 100, Price = 1000, VIN = "5050" };
            Owner owner = new() { Id = 1, Name = "hossam" };
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            ownersRepositoryMock.Setup(o => o.GetOwnerById(1)).Returns(owner);
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 100
            };
            string result = ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Insufficient funds", result);
        }
        [Fact]
        public void BuyCar_NotApprovedPayment_SomethingWentWrong()
        {
            Car car = new() { Id = 1, OwnerId = 1, Type = CarType.Audi, Velocity = 100, Price = 1000, VIN = "5050" };
            Owner owner = new() { Id = 1, Name = "hossam" };
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            ownersRepositoryMock.Setup(o => o.GetOwnerById(1)).Returns(owner);
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 1500
            };
            CashService cash = new();
            cashServiceMock.Setup(c=>c.Pay(carInput.Amount)).Returns(cash.Pay(carInput.Amount));
            carsRepositoryMock.Setup(car => car.AssignToOwner(1, 1)).Returns(false);
            //act
            var result = ownersService.BuyCar(carInput);
            //assert
            Assert.Equal("Something went wrong", result);
        }
        [Fact]  
        public void BuyCar_ApporvedBuyCar_ApprovalMessage()
        {
            Car car = new() { Id = 1, OwnerId = 1, Type = CarType.Audi, Velocity = 100, Price = 1000, VIN = "5050" };
            Owner owner = new() { Id = 1, Name = "hossam" };
            carsRepositoryMock.Setup(c => c.GetCarById(1)).Returns(car);
            ownersRepositoryMock.Setup(o => o.GetOwnerById(1)).Returns(owner);
            BuyCarInput carInput = new BuyCarInput()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 1500
            };
            CashService cash = new();
            cashServiceMock.Setup(c => c.Pay(carInput.Amount)).Returns(cash.Pay(carInput.Amount));
            carsRepositoryMock.Setup(car => car.AssignToOwner(1, 1)).Returns(true);
            //act
            var result = ownersService.BuyCar(carInput);
            //assert
            Assert.Contains("Successfull", result);
        }
        public void Dispose()
        {
            helper.WriteLine("Test Cleanup");
        }
    }
}
