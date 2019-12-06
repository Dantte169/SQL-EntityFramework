namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;
    [XmlType("Customer")]
    public class CustomerWithTicketsImportDto
    {

        [Required, MinLength(3), MaxLength(20)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }
        [Required, MinLength(3), MaxLength(20)]
        [XmlElement("LastName")]
        public string LastName { get; set; }
        [Required, Range(12, 110)]
        [XmlElement("Age")]
        public int Age { get; set; }
        [Required, Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Balance")]
        public decimal Balance { get; set; }
        [XmlArray("Tickets")]
        public TicketImportDto[] Tickets { get; set; }
    }
}
