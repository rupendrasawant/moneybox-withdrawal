﻿using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using Moneybox.App.Features;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;

namespace Moneybox.Tests.Features
{
    public class WithdrawMoneyTests
    {
        private WithdrawMoney _withdrawMoney;
        private Mock<IAccountRepository> _iAccountRepository;
        private Mock<INotificationService> _notificationService;


        [SetUp]
        public void Setup()
        {
            _iAccountRepository = new Mock<IAccountRepository>();
            _notificationService = new Mock<INotificationService>();
            _withdrawMoney = new WithdrawMoney(_iAccountRepository.Object, _notificationService.Object);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Execute_InvalidFromAccountId_InvalidOperationException()
        {
            // Arrange           
            Guid from = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF03");
        
            _iAccountRepository.Setup(x => x.GetAccountById(from)).Returns(() => null);
           
            // Act
            Assert.Throws<InvalidOperationException>(() => _withdrawMoney.Execute(from, 500));

            // Assert
            _iAccountRepository.Verify(service => service.Update(null), Times.Never);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Execute_InvalidZeroAmount_InvalidOperationException()
        {
            // Arrange            
            Guid from = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");
           
            _iAccountRepository.Setup(x => x.GetAccountById(from)).Returns(new App.Account { Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), User = new App.User() { Id = new Guid("12223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Test1", Email = "test1@gmail.com" }, Balance = 50, Withdrawn = 50, PaidIn = 50 });
            
            // Act
            Assert.Throws<InvalidOperationException>(() => _withdrawMoney.Execute(from, 50));

            // Assert

            _iAccountRepository.Verify(service => service.Update(null), Times.Never);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Execute_InvalidAmountNegativeAmount_InvalidOperationException()
        {
            // Arrange

            Guid from = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");

            _iAccountRepository.Setup(x => x.GetAccountById(from)).Returns(new App.Account { Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), User = new App.User() { Id = new Guid("12223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Test1", Email = "test1@gmail.com" }, Balance = 49, Withdrawn = 50, PaidIn = 50 });

            // Act
            Assert.Throws<InvalidOperationException>(() => _withdrawMoney.Execute(from, 50));

            // Assert

            _iAccountRepository.Verify(service => service.Update(null), Times.Never);
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Execute_FromAccountLowBalance_NotifyFundsLow()
        {
            // Arrange         
            Guid from = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");
        
            var fromAccount = new App.Account
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                User = new App.User() { Id = new Guid("12223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Test1", Email = "test1@gmail.com" },
                Balance = 500,
                Withdrawn = 50,
                PaidIn = 50
            };

            _iAccountRepository.Setup(x => x.GetAccountById(from)).Returns(fromAccount);
            
            // Act
            Assert.DoesNotThrow(() => _withdrawMoney.Execute(from, 50));

            // Assert
            _notificationService.Verify(service => service.NotifyFundslow("test1@gmail.com"), Times.Once);            
            _iAccountRepository.Verify(service => service.Update(fromAccount), Times.Once);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Execute_ValidDetails_Success()
        {
            // Arrange        
            Guid from = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00");
            Guid to = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF01");

            var fromAccount = new App.Account
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                User = new App.User() { Id = new Guid("12223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Test1", Email = "test1@gmail.com" },
                Balance = 500,
                Withdrawn = 50,
                PaidIn = 50
            };

         
            _iAccountRepository.Setup(x => x.GetAccountById(from)).Returns(fromAccount);
            
            // Act
            Assert.DoesNotThrow(() => _withdrawMoney.Execute(from, 50));

            // Assert                       
            _iAccountRepository.Verify(service => service.Update(fromAccount), Times.Once);
        }

    }
}
