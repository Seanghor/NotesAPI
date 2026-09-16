using System.Data;

namespace NotesApi.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
