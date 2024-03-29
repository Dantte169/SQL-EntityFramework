﻿namespace MusicHub.DataProcessor.ExportDtos
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ExportSongsAboveDurationDto
    {
        [XmlElement("SongName")]
        public string SongName { get; set; }
        [XmlElement("Writer")]
        public string Writer { get; set; }
        [XmlElement("Performer")]
        public string Performer { get; set; }
        [XmlElement("AlbumProducer")]
        public string AlbumProducer { get; set; }
        [XmlElement("Duration")]
        public string Duration { get; set; }
    }
}
