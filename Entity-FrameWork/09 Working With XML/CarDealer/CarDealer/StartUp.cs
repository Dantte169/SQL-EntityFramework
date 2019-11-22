using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            var db = new CarDealerContext();

            Console.WriteLine(GetSalesWithAppliedDiscount(db));
        }

        public static void SeedDatabase(CarDealerContext context)
        {
            var carsFile = File.ReadAllText("../../../Datasets/cars.xml");
            var customersFile = File.ReadAllText("../../../Datasets/customers.xml");
            var partsFile = File.ReadAllText("../../../Datasets/parts.xml");
            var salesFile = File.ReadAllText("../../../Datasets/sales.xml");
            var SuppliersFile = File.ReadAllText("../../../Datasets/suppliers.xml");

            ImportSuppliers(context, SuppliersFile);
            ImportParts(context, partsFile);
            ImportCars(context, carsFile);
            ImportCustomers(context, customersFile);
            ImportSales(context, salesFile);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var suppliersData = (ImportSupplierDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var dto in suppliersData)
            {
                var supplier = Mapper.Map<Supplier>(dto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDTO[]), new XmlRootAttribute("Parts"));

            var partsData = (ImportPartDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Part> partsList = new List<Part>();

            foreach (var partItem in partsData)
            {
                var supplier = context.Suppliers.Find(partItem.SupplierId);

                if (supplier != null)
                {
                    var part = Mapper.Map<Part>(partItem);
                    partsList.Add(part);
                }
            }

            context.Parts.AddRange(partsList);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));
            var carsData = (ImportCarDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Car> carsList = new List<Car>();

            foreach (var carDto in carsData)
            {
                var car = Mapper.Map<Car>(carDto);

                context.Cars.Add(car);

                foreach (var part in carDto.Parts.PartsId)
                {
                    if (car.PartCars
                        .FirstOrDefault(p => p.PartId == part.PartId) == null &&
                        context.Parts.Find(part.PartId) != null)
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = part.PartId
                        };

                        car.PartCars.Add(partCar);
                    }
                }

                carsList.Add(car);
            }

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));
            var customersData = (ImportCustomerDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Customer> customersList = new List<Customer>();

            foreach (var customerItem in customersData)
            {
                var customer = Mapper.Map<Customer>(customerItem);
                customersList.Add(customer);
            }

            context.AddRange(customersList);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));
            var salesData = (ImportSaleDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Sale> salesList = new List<Sale>();

            foreach (var saleItem in salesData)
            {
                if (context.Cars.Find(saleItem.CarId) != null)
                {
                    var sale = Mapper.Map<Sale>(saleItem);
                    salesList.Add(sale);
                }
            }

            context.AddRange(salesList);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ExportLocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), suppliers, namespaces);


            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarsWithDistanceDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDTO[]), new XmlRootAttribute("cars"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportBMWCarsDTO
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ExportBMWCarsDTO[]), new XmlRootAttribute("cars"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new ExportCarWithPartsDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(p => new ExportListOfPartsDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ExportCarWithPartsDTO[]), new XmlRootAttribute("cars"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() >= 1)
                .Select(c => new ExportTotalSalesByCustomerDTO
                {
                    FullName = c.Name,
                    BoughCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(p => p.Car.PartCars.Sum(x => x.Part.Price))
                })
               .OrderByDescending(c => c.SpentMoney)
               .ToArray();

            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ExportTotalSalesByCustomerDTO[]), new XmlRootAttribute("customers"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new ExportSaleWithDiscountDTO
                {
                    Car = new ExportCarDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(p => p.Part.Price),
                    DiscountPrice = s.Car.PartCars.Sum(p => p.Part.Price) -
                        s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100
                })
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportSaleWithDiscountDTO[]), new XmlRootAttribute("sales"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}