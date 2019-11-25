namespace ProductShop
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    using ProductShop.Data;
    using ProductShop.Models;
    using ProductShop.Dtos.Import;

    using AutoMapper;
    using ProductShop.Dtos.Export;
    using System.Text;
    using System.Xml;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

            var db = new ProductShopContext();


            Console.WriteLine(GetUsersWithProducts(db));
        }

        public static void SeedDatabase(ProductShopContext context)
        {
            var usersFile = File.ReadAllText("../../../Datasets/users.xml");
            var productFile = File.ReadAllText("../../../Datasets/products.xml");
            var categoriesFile = File.ReadAllText("../../../Datasets/categories.xml");
            var categoryProductsFile = File.ReadAllText("../../../Datasets/categories-products.xml");

            ImportUsers(context, usersFile);
            ImportProducts(context, productFile);
            ImportCategories(context, categoriesFile);
            ImportCategoryProducts(context, categoryProductsFile);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportUsersDTO[]), new XmlRootAttribute("Users"));

            var usersData = (ImportUsersDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<User> usersList = new List<User>();

            foreach (var userItem in usersData)
            {
                var user = Mapper.Map<User>(userItem);
                usersList.Add(user);
            }
            context.Users.AddRange(usersList);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportProductsDTO[]), new XmlRootAttribute("Products"));

            var productData = (ImportProductsDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Product> products = new List<Product>();

            foreach (var productItem in productData)
            {
                var product = Mapper.Map<Product>(productItem);
                products.Add(product);
            }

            context.Products.AddRange(products);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoriesDTO[]), new XmlRootAttribute("Categories"));

            var categoriesData = (ImportCategoriesDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Category> categories = new List<Category>();

            foreach (var categoryItem in categoriesData)
            {
                var category = Mapper.Map<Category>(categoryItem);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductsDTO[]), new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDto = ((ImportCategoryProductsDTO[])xmlSerializer.Deserialize(new StringReader(inputXml))).ToList();

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                var targetProduct = context.Products.Find(categoryProductDto.ProductId);
                var targetCategory = context.Categories.Find(categoryProductDto.CategoryId);

                if (targetProduct != null && targetCategory != null)
                {
                    var category = Mapper.Map<CategoryProduct>(categoryProductDto);
                    categoryProducts.Add(category);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ExportProductDTO[]), new XmlRootAttribute("Products"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            // var products = context.Users
            //     .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
            //     .Select(u => new ExportSoldProductsDTO
            //     {
            //         FirstName = u.FirstName,
            //         LastName = u.LastName,
            //         SoldProducts = u.ProductsSold.Select(p => new ExportProductWithPriceDTO
            //         {
            //             Name = p.Name,
            //             Price = p.Price,
            //         })
            //         .ToArray()
            //     })
            //     .OrderBy(u => u.LastName)
            //     .ThenBy(u => u.FirstName)
            //     .Take(5)
            //     .ToArray();
            //
            // StringBuilder sb = new StringBuilder();
            //
            // var serializer = new XmlSerializer(typeof(ExportSoldProductsDTO[]), new XmlRootAttribute("Users"));
            // var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            // serializer.Serialize(new StringWriter(sb), products, namespaces);
            //
            // return sb.ToString().TrimEnd();
            return "";
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new ExportCategoriesByProductCountDTO
                {
                    Name = c.Name,
                    ProductCount = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(c => c.ProductCount)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ExportCategoriesByProductCountDTO[]), new XmlRootAttribute("Categories"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
               .Where(u => u.ProductsSold.Any())
               .OrderByDescending(p => p.ProductsSold.Count())
               .Select(u => new ExportUsersWithSoldProductsDto
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   Age = u.Age,
                   SoldProducts = new ExportSoldProductsCountDto
                   {
                       Count = u.ProductsSold.Count(),
                       Products = u.ProductsSold
                       .Select(p => new ExportSoldProductsDto
                       {
                           Name = p.Name,
                           Price = p.Price
                       })
                       .OrderByDescending(p => p.Price)
                       .ToArray()
                   }
               })
               .Take(10)
               .ToArray();

            var result = new UsersAndProductsDto
            {
                Count = context.Users.Count(p => p.ProductsSold.Any()),
                Users = users
            };

            var xmlSerializer = new XmlSerializer(typeof(UsersAndProductsDto), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}