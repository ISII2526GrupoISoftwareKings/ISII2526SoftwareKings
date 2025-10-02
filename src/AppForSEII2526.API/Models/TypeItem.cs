namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class TypeItem
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public Class? Class { get; set; }
    }
}
