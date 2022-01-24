namespace EFCore_Tutorial.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public  List<Character> Character { get; set; }
    }
}
