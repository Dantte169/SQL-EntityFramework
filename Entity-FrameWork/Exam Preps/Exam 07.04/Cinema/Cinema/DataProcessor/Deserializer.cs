namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movieDtos = JsonConvert.DeserializeObject<MovieImportDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Movie> movies = new List<Movie>();

            foreach (var Dto in movieDtos)
            {
                bool isEnumValid = Enum.IsDefined(typeof(Genre), Dto.Genre);
                bool doesMovieExist = context.Movies.Any(t => t.Title == Dto.Title);

                if (!IsValid(Dto) || !isEnumValid || doesMovieExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var enumType = Enum.Parse(typeof(Genre), Dto.Genre);

                var movie = new Movie
                {
                    Title = Dto.Title,
                    Genre = (Genre)enumType,
                    Duration = TimeSpan.ParseExact(Dto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = Dto.Rating,
                    Director = Dto.Director,
                };

                movies.Add(movie);

                sb.AppendLine(String.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("F2")));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDtos = JsonConvert.DeserializeObject<HallWithSeatsImportDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Hall> halls = new List<Hall>();

            foreach (var dto in hallsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall
                {
                    Name = dto.Name,
                    Is4Dx = dto.Is4Dx,
                    Is3D = dto.Is3D,
                };

                for (int i = 0; i < dto.Seats; i++)
                {
                    var seat = new Seat
                    {
                        HallId = hall.Id
                    };
                    hall.Seats.Add(seat);
                }

                halls.Add(hall);

                string projectionType = string.Empty;
                if (hall.Is4Dx)
                {
                    if (hall.Is3D)
                    {
                        projectionType = "4Dx/3D";
                    }
                    else
                    {
                        projectionType = "4Dx";
                    }
                }
                else if (hall.Is3D)
                {
                    projectionType = "3D";
                }
                else
                {
                    projectionType = "Normal";
                }

                sb.AppendLine(String.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count()));

            }
            context.Halls.AddRange(halls);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }


        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectionImportDto[]), new XmlRootAttribute("Projections"));
            var projectionDtos = (ProjectionImportDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();
            List<Projection> projections = new List<Projection>();

            foreach (var dto in projectionDtos)
            {
                var isHallValid = context.Halls.Find(dto.HallId);
                var isMovieValid = context.Movies.Find(dto.MovieId);
                if (!IsValid(dto) || isHallValid == null || isMovieValid == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection
                {
                    HallId = dto.HallId,
                    Hall = isHallValid,
                    MovieId = dto.MovieId,
                    Movie = isMovieValid,
                    DateTime = DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                projections.Add(projection);
                sb.AppendLine(String.Format(SuccessfulImportProjection, isMovieValid.Title, projection.DateTime.ToString("MM/dd/yyyy")));
            }
            context.Projections.AddRange(projections);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CustomerWithTicketsImportDto[]), new XmlRootAttribute("Customers"));
            var customerDtos = (CustomerWithTicketsImportDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Customer> customers = new List<Customer>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in customerDtos)
            {
                var projections = context.Projections.Select(s => s.Id).ToArray();
                var isProjectionValid = projections.Any(p => dto.Tickets.Any(t => t.ProjectionId != p));
                if (!IsValid(dto) && dto.Tickets.All(IsValid) && isProjectionValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Balance = dto.Balance
                };

                foreach (var ticket in dto.Tickets)
                {
                    customer.Tickets.Add(new Ticket
                    {
                        ProjectionId = ticket.ProjectionId,
                        Price = ticket.Price
                    });
                }
                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;

        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}