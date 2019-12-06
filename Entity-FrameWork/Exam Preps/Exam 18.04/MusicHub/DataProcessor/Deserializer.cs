namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using AutoMapper;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
    using System.Linq;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using MusicHub.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writerData = JsonConvert.DeserializeObject<WriterImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            List<Writer> writers = new List<Writer>();

            foreach (var dto in writerData)
            {
                if (IsValid(dto))
                {
                    var writer = Mapper.Map<Writer>(dto);

                    sb.AppendLine(String.Format(SuccessfullyImportedWriter, writer.Name));
                    writers.Add(writer);
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Writers.AddRange(writers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producerData = JsonConvert.DeserializeObject<ProducerAlbumsImportDto[]>(jsonString);

            List<Producer> producers = new List<Producer>();

            StringBuilder sb = new StringBuilder();

            foreach (var dto in producerData)
            {
                if (IsValid(dto) && dto.Albums.All(IsValid))
                {
                    var producer = new Producer
                    {
                        Name = dto.Name,
                        Pseudonym = dto.Pseudonym,
                        PhoneNumber = dto.PhoneNumber,
                    };

                    foreach (var albumDto in dto.Albums)
                    {
                        var album = new Album
                        {
                            Name = albumDto.Name,
                            ReleaseDate = DateTime.ParseExact(albumDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        };

                        producer.Albums.Add(album);
                    }

                    producers.Add(producer);

                    if (producer.PhoneNumber != null)
                    {
                        sb.AppendLine(String.Format(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber, producer.Albums.Count));
                    }
                    else
                    {
                        sb.AppendLine(String.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count));
                    }
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Producers.AddRange(producers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(SongImportDto[]), new XmlRootAttribute("Songs"));
            var songData = (SongImportDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Song> songs = new List<Song>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in songData)
            {
                bool isValidEnum = Enum.IsDefined(typeof(Genre), dto.Genre);
                var isAlbumValid = context.Albums.Find(dto.AlbumId);
                var isWriterValid = context.Writers.Find(dto.WriterId);
                if (IsValid(dto) && isValidEnum && isWriterValid != null && isAlbumValid != null)
                {
                    var enumType = Enum.Parse(typeof(Genre), dto.Genre);
                    var song = new Song
                    {
                        Name = dto.Name,
                        Duration = TimeSpan.ParseExact(dto.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture),
                        CreatedOn = DateTime.ParseExact(dto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Genre = (Genre)enumType,
                        AlbumId = dto.AlbumId,
                        WriterId = dto.WriterId,
                        Price = dto.Price,
                    };

                    sb.AppendLine(String.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
                    songs.Add(song);
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(SongPerformerImportDto[]), new XmlRootAttribute("Performers"));
            var performersData = (SongPerformerImportDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Performer> performers = new List<Performer>();

            foreach (var dto in performersData)
            {
                var songs = context.Songs.Select(s => s.Id).ToList();
                bool isSongsValid = dto.PerformersSongs.Select(s => s.Id).All(s => songs.Contains(s));

                if (IsValid(dto) && isSongsValid)
                {
                    var performer = new Performer
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        Networth = dto.NetWorth
                    };

                    performer.SongPerformers = dto.PerformersSongs
                        .Select(s => new SongPerformer
                        {
                            SongId = s.Id,
                        })
                        .ToList();

                    performers.Add(performer);

                    sb.AppendLine(string.Format(SuccessfullyImportedPerformer,
                        performer.FirstName,
                        performer.SongPerformers.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Performers.AddRange(performers);
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