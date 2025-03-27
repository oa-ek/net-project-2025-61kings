namespace Dealship.Models
{
    public class Engine
    {
        public int Id { get; set; }
        public string Type { get; set; } // (Бензин, Дизель, Електро)
        public double Volume { get; set; } // Об'єм двигуна
        public int Horsepower { get; set; } // Кількість к.с.
    }
}
