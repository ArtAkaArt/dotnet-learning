
namespace Sol0
{
    public class InputContainer
    {
        public int Id { get; }
        public string Name { get; }
        public int FactoryId { get; }

        public InputContainer(int id, string name, int factoryid)
        {
            Id = id;
            Name = name;
            FactoryId = factoryid;
        }
    }
}
