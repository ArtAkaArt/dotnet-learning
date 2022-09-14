using MyORM;
namespace Sol3
{
    public class CatDbContext : SuperDbContext
    {
        public CatDbContext(string conn) : base(conn)
        {
        }
        public async Task<IReadOnlyCollection<TItem>> Test<TItem>(string sqlSttring)
        {
            return await connection.QueryMultipleItems<TItem>(sqlSttring);
        }
    }
}
