namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string result = GetBooksByPrice(db);
            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            // var age = Enum.Parse<AgeRestriction>(command, true);

            var restrictedBooks = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.AgeRestriction
                })
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(x => x.Title)
                .ToList();

            StringBuilder builder = new StringBuilder();


            foreach (var book in restrictedBooks)
            {
                builder
                    .AppendLine(book.Title);
            }

            return builder.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context
                .Books
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Copies
                })
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .ToList();


            StringBuilder builder = new StringBuilder();


            foreach (var book in goldenBooks)
            {
                builder
                    .AppendLine(book.Title);
            }

            return builder.ToString().TrimEnd();

        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            StringBuilder builder = new StringBuilder();


            foreach (var book in books)
            {
                builder
                    .AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return builder.ToString().TrimEnd();

        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var releasedBooks = context
                .Books
                .Select(b => new
                {
                    b.Title,
                    b.ReleaseDate
                })
                .Where(b => b.ReleaseDate.Value.Year != year)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in releasedBooks)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            List<string> booksTitles = new List<string>();

            foreach (var item in categories)
            {
                var books = context.Books
                    .Where(b => b.BookCategories
                    .Any(c => c.Category.Name.ToLower() == item.ToLower()))
                    .Select(b => new
                    {
                        b.Title
                    })
                    .ToList();

                foreach (var book in books)
                {
                    booksTitles.Add(book.Title);
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (var title in booksTitles.OrderBy(x => x))
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime releaseDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var releasedBooks = context
                .Books
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .Where(b => b.ReleaseDate < releaseDate)
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var book in releasedBooks)
            {
                sb
                    .AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Author.LastName,
                    AurthorName = b.Author.FirstName + " " + b.Author.LastName
                })
                .Where(b => b.LastName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .OrderBy(b => b.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AurthorName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(b => b.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorCopies = context.Authors
                .Select(a => new
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    Count = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.Count)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authorCopies)
            {
                sb.AppendLine($"{author.AuthorName} - {author.Count}");
            }

            return sb.ToString().TrimEnd();

        }
    }
}
