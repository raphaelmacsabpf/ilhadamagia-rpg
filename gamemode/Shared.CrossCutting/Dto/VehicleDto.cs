namespace Shared.CrossCutting.Dto
{
    public class VehicleDto
    {
        public string Guid { get; set; }
        public uint Hash { get; set; }
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public int Fuel { get; set; }
        public int EngineHealth { get; set; }
    }
}