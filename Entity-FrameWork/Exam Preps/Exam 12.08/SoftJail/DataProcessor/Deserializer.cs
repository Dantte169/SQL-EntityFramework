namespace SoftJail.DataProcessor
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;

    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string ImportedDepartmentMessage = "Imported {0} with {1} cells";
        private const string ImportedPrisonerMessage = "Imported {0} {1} years old";
        private const string ImportedOfficersPrisonerMessage = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentDtos = JsonConvert.DeserializeObject<DepartmentCellsImportDto[]>(jsonString);
            List<Department> departments = new List<Department>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in departmentDtos)
            {
                if (!IsValid(dto) || dto.Cells.Any(c => IsValid(c) == false))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var department = new Department
                {
                    Name = dto.Name
                };

                foreach (var cellDto in dto.Cells)
                {
                    var cell = new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow,
                    };

                    department.Cells.Add(cell);
                }

                departments.Add(department);

                sb.AppendLine(String.Format(ImportedDepartmentMessage, department.Name, department.Cells.Count()));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<PrisonerMailImportDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in prisonersDto)
            {
                if (IsValid(dto) == false || dto.Mails.Any(m => IsValid(m) == false))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                var prisoner = new Prisoner
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = string.IsNullOrEmpty(dto.ReleaseDate) ? (DateTime?)null : DateTime.ParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = dto.Bail,
                    CellId = dto.CellId,
                };

                foreach (var mailDto in dto.Mails)
                {
                    var mail = new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address,
                    };
                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                sb.AppendLine(String.Format(ImportedPrisonerMessage, prisoner.FullName, prisoner.Age));
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerImportDto[]), new XmlRootAttribute("Officers"));
            var officerDtos = (OfficerImportDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();

            foreach (var dto in officerDtos)
            {
                bool isWeaponValid = Enum.IsDefined(typeof(Weapon), dto.Weapon);
                bool isPositionValid = Enum.IsDefined(typeof(Position), dto.Position);

                if (!IsValid(dto) || isWeaponValid == false || isPositionValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer
                {
                    FullName = dto.FullName,
                    Salary = dto.Salary,
                    Position = (Position)Enum.Parse(typeof(Position), dto.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), dto.Weapon),
                    DepartmentId = dto.DepartmentId,
                };

                foreach (var prisonerdto in dto.Prisoners)
                {
                    var officerPrisoner = new OfficerPrisoner
                    {
                        PrisonerId = prisonerdto.Id,
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(officer);

                sb.AppendLine(string.Format(ImportedOfficersPrisonerMessage,
                    officer.FullName,
                    officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}