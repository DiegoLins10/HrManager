using AutoFixture;
using HrManager.Application.Models.InputModels;
using HrManager.Application.Services;
using HrManager.Core.Entities;
using HrManager.Core.Exceptions;
using HrManager.Core.Repositories;
using Moq;
using Shouldly;
using System;
using Xunit;

namespace HrManager.UnitTests.Application.Services.EmployeeServiceTests
{
    public class EmployeeServiceAddTests
    {
        [Fact]
        public void ValidEmployee_AddIsCalled_ReturnValidEmployeeViewModel()
        {
            // Arrange
            // cria com dados aleatorio o input model
            var addEmploymentInputModel = new Fixture().Create<AddEmployeeInputModel>();

            // criando o mock
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            // adicionando o mock ao nosso service
            var employeeService = new EmployeeService(employeeRepositoryMock.Object);


            //  o Fixture gera uma data aleatoria entao pode gerar uma data maior que hoje e gerar excecao, tratando isso com esse trecho
            addEmploymentInputModel.BirthDate = DateTime.Today.AddYears(-23);

            // Act
            // adicionando objeto gerado pelo fixture
            var result = employeeService.Add(addEmploymentInputModel);

            // Assert

            // utilizando a asert do proprio XUnit para validar as propriedades
            Assert.Equal(addEmploymentInputModel.Role, result.Role);
            Assert.Equal(addEmploymentInputModel.FullName, result.FullName);
            Assert.Equal(addEmploymentInputModel.Document, result.Document);
            Assert.Equal(addEmploymentInputModel.BirthDate, result.BirthDate);
            Assert.Equal(addEmploymentInputModel.RoleLevel, result.RoleLevel);
            Assert.Equal(addEmploymentInputModel.Role, result.Role);

            // utilizando a lib shouldly para validar as propriedades
            result.FullName.ShouldBe(addEmploymentInputModel.FullName);
            result.Document.ShouldBe(addEmploymentInputModel.Document);
            result.BirthDate.ShouldBe(addEmploymentInputModel.BirthDate);
            result.RoleLevel.ShouldBe(addEmploymentInputModel.RoleLevel);
            result.Role.ShouldBe(addEmploymentInputModel.Role);

            employeeRepositoryMock.Verify(er => er.Add(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public void InvalidBirthDateForEmployee_AddIsCalled_ThrowAnInvalidBirthDateException()
        {
            // Arrange
            // cria com dados aleatorio o input model
            var addEmploymentInputModel = new Fixture().Create<AddEmployeeInputModel>();

            // mudo a data de aniversio para provocar a exceção
            addEmploymentInputModel.BirthDate = DateTime.Today.AddDays(1);

            // criando o mock
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            // adicionando o mock ao nosso service
            var employeeService = new EmployeeService(employeeRepositoryMock.Object);

            // Act + Assert

            // utilizando XUnit
            var exception = Assert.Throws<BirthDateCannotBeInTheFutureException>(() =>
            employeeService.Add(addEmploymentInputModel));

            Assert.Equal("Birth date cannot be in the future.", exception.Message);

            // utilizando Shouldly
            Should.Throw<BirthDateCannotBeInTheFutureException>(() =>
                employeeService.Add(addEmploymentInputModel))
                .Message.ShouldBe("Birth date cannot be in the future.");
        }
    }
}