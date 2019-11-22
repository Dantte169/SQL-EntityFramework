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

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

            var db = new ProductShopContext();

            SeedDatabase(db);
        }

        public static void SeedDatabase(ProductShopContext context)
        {
            var usersFile = File.ReadAllText("../../../Datasets/users.xml");
            var productFile = File.ReadAllText("../../../Datasets/products.xml");
            var categoriesFile = File.ReadAllText("../../../Datasets/categories.xml");
            var categoryProductsFile = File.ReadAllText("../../../Datasets/categories-products.xml");

            Console.WriteLine(ImportCategoryProducts(context, categoryProductsFile));
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
            var categoryProductsDto =((ImportCategoryProductsDTO[])xmlSerializer.Deserialize(new StringReader(inputXml))).ToList();

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
    }
}