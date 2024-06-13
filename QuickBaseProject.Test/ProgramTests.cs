using Moq;
using System.Data.SqlClient;

namespace QuickBaseProject.Test
{
    [TestFixture]
    public class ProgramTests
    {
        private Mock<HttpClient> _githubClient;
        private Mock<HttpClient> _freshdeskClient;
        private Mock<SqlConnection> _connection;

        [SetUp]
        public void Setup()
        {
            _githubClient = new Mock<HttpClient>();
            _freshdeskClient = new Mock<HttpClient>();
            _connection = new Mock<SqlConnection>();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}