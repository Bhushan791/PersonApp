namespace PersonApp.core.models

{
    public class Person : BaseEntity
    {
        // Properties with basic encapsulation
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Optional: override ToString for easy display
        public override string ToString()
        {
            return $"{FullName} - {Email} - {Phone}";

        }

        
    }
}
