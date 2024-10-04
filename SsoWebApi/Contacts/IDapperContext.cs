using System.Data;

namespace SsoWebApi.Contacts
{
    public interface IDapperContext
    {
        IDbConnection CreateDbConnection();
    }
}