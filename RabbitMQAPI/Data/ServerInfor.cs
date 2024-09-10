using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RabbitMQAPI.Data
{
    [Table("ServerInfor")]
    public class ServerInfor
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public double CPU { get; set; }
        public double RAM { get; set; }
        public double D { get; set; }
        public DateTime Time { get; set; }
        public string IP { get; set; }
        public string Region { get; set; }
    }
}
