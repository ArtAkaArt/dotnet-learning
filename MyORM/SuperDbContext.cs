using Npgsql;


namespace MyORM
{
    public abstract class SuperDbContext
    {
        public NpgsqlConnection connection { get; set; }
        public SuperDbContext (string conn) => connection = new NpgsqlConnection(conn);
    }
}
