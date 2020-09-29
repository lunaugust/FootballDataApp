using AutoMapper;
using Football.API.Controllers;
using Football.Client;
using Football.Client.Models;
using Football.Data;
using Football.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Football.API.test
{
    [TestFixture]
    public class ImportControllerTests
    {
        private Mock<IFootballRepository> _footballRepositoryMock;
        private Mock<IFootballClient> _footballClientMock;
        private Mock<IMapper> _mapperMock;
        private ImportController _importController;

        [SetUp]
        public void Setup()
        {
            _footballRepositoryMock = new Mock<IFootballRepository>();
            _footballClientMock = new Mock<IFootballClient>();
            _mapperMock = new Mock<IMapper>();

            _importController = new ImportController(_mapperMock.Object, _footballRepositoryMock.Object, _footballClientMock.Object);
        }

        [Test]
        public async Task ShouldReturn409()
        {
            _footballRepositoryMock.Setup(x => x.GetCompetitionAsync(It.IsAny<Expression<Func<Competition, bool>>>())).ReturnsAsync(new Competition() { Id = 2, Code = "LC" });

            var result = await _importController.Get("LC");

            _footballRepositoryMock.Verify(x => x.GetCompetitionAsync(It.IsAny<Expression<Func<Competition, bool>>>()), Times.Once);
            Assert.AreEqual((int)HttpStatusCode.Conflict, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task ShouldReturn404()
        {
            _footballRepositoryMock.Setup(x => x.GetCompetitionAsync(It.IsAny<Expression<Func<Competition, bool>>>())).ReturnsAsync((Competition)null);

            _footballClientMock.Setup(x => x.GetCompetitionByCodeAsync(It.IsAny<string>())).ReturnsAsync(new CompetitionModel());

            var result = await _importController.Get("WC");

            Assert.AreEqual((int)HttpStatusCode.NotFound, ((ObjectResult)result).StatusCode);
        }
    }
}