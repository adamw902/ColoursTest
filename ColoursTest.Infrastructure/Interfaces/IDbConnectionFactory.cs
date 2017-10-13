using System.Data;

namespace ColoursTest.Infrastructure.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}