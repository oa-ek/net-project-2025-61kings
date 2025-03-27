using Microsoft.VisualBasic.FileIO;
using System.Drawing;

namespace Dealship.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; } 
        public decimal Price { get; set; } 
        public int Mileage { get; set; } 
        public int EngineId { get; set; } 
        public int TransmissionId { get; set; } 
        public int FuelTypeId { get; set; } 
        public int ColorId { get; set; } 
        public byte[] Image { get; set; }
        public virtual Engine Engine { get; set; }
        public virtual Transmission Transmission { get; set; }
        public virtual FuelType FuelType { get; set; }
        public virtual Color Color { get; set; }
    }
}
