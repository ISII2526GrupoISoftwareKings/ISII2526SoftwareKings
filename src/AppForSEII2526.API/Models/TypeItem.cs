namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class TypeItem
    {
        public TypeItem()
        {
        }
        public TypeItem(int id, string name, IList<Item> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Item> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TypeItem item &&
                   Id == item.Id &&
                   Name == item.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
