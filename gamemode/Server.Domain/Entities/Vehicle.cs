namespace Server.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public uint Hash { get; set; }
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public int Fuel { get; set; }
        public int EngineHealth { get; set; }
    }
}