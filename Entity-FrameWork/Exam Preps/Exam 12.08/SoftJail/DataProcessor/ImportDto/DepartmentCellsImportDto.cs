namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    public class DepartmentCellsImportDto
    {
        [Required, MinLength(3), MaxLength(25)]
        public string Name { get; set; }

        public CellImportDto[] Cells { get; set; }
    }
}
