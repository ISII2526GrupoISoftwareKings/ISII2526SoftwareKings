
namespace AppForSEII2526.API.DTOs.TypeItemDTOs
{
    public class TypeItemForClassDTO
    {
        public TypeItemForClassDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TypeItemForClassDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
