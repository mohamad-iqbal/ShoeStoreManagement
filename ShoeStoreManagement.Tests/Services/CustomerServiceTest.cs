using FluentAssertions;
using Moq;
using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Exceptions;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Application.Services;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoeStoreManagement.Tests.Services
{
    public class CustomerServiceTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _customerService = new CustomerService(
                _customerRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Sales)]
        public async Task CreateCustomerAsync_WhenUserIsValidRole_ShouldCreateCustomer(Role role)
        {
            // Arrange
            var dto = new CreateCustomerDto
            {
                Name = "John Doe",
                Phone = "081234567890",
                Address = "Jakarta",
                Source = Source.Shopee
            };

            _currentUserServiceMock
                .Setup(x => x.Role)
                .Returns(role);

            // Act
            var result = await _customerService.CreateCustomerAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Address.Should().Be(dto.Address);
            result.Phone.Should().Be(dto.Phone);
            result.Source.Should().Be(dto.Source);

            _customerRepositoryMock.Verify(
                x => x.AddAsync(It.Is<Customer>(c =>
                    c.Name == dto.Name &&
                    c.Address == dto.Address &&
                    c.Phone == dto.Phone &&
                    c.Source == dto.Source)),
                Times.Once());

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenUserRoleIsInvalid_ShouldThrowForbiddenException()
        {
            // Arrange
            var dto = new CreateCustomerDto
            {
                Name = "Jhon Doe",
                Address = "Jakarta",
                Phone = "081234567890",
                Source = Source.Shopee
            };

            _currentUserServiceMock
                .Setup(x => x.Role)
                .Returns((Role)99);

            // Act
            Func<Task> act = async () => await _customerService.CreateCustomerAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ForbiddenException>();

            _customerRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Customer>()),
                Times.Never());

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never());
        }
    }
}
