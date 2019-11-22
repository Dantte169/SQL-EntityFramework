namespace ProductShop.Dtos.Import
{
    using System.Xml.Serialization;
    [XmlType("CategoryProduct")]
    public class ImportCategoryProductsDTO
    {
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }
        [XmlElement("ProductId")]
        public int ProductId { get; set; }
    }
}
