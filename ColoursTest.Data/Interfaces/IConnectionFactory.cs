using System.Data;

namespace ColoursTest.Infrastructure.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}