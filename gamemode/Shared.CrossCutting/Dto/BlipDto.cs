namespace GF.CrossCutting.Dto
{
    public class BlipDto
    {
        public BlipDto(string category, int spriteId, int colour, float x, float y, float z)
        {
            this.Category = category;
            SpriteId = spriteId;
            Colour = colour;
            X = x;
            Y = y;
            Z = z;
        }

        public string Category { get; set; }
        public int SpriteId { get; set; }
        public int Colour { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}