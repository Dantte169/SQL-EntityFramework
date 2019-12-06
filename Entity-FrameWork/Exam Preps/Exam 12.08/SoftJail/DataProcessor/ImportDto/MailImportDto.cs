namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    public class MailImportDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required, RegularExpression(@"^[a-zA-Z 0-9]+ str.$")]
        public string Address { get; set; }
    }
}
