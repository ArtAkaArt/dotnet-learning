using Npgsql;


namespace MyORM
{
    public abstract class SuperDbContext : IDisposable
    {
        public NpgsqlConnection connection { get; set; }
        public SuperDbContext (string conn) => connection = new NpgsqlConnection(conn);
        public async Task OpenAsync()
        {
            await connection.OpenAsync();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            connection.Close();
        }
    }
}
