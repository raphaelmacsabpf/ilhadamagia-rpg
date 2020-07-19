namespace GF.CrossCutting.Dto
{
    // TODO: Uppercase first letter in each auto-property
    public class MarkerDto
    {
        public MarkerDto(int type, float posX, float posY, float posZ, float dirX, float dirY, float dirZ, float rotX, float rotY, float rotZ, float scaleX, float scaleY, float scaleZ, MarkerColor color, bool bobUpAndDown, bool faceCamera, int p19, bool rotate, string textureDict, string textureName, bool drawOnEnts)
        {
            this.type = type;
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
            this.dirX = dirX;
            this.dirY = dirY;
            this.dirZ = dirZ;
            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.scaleZ = scaleZ;
            this.Color = color;
            this.bobUpAndDown = bobUpAndDown;
            this.faceCamera = faceCamera;
            this.p19 = p19;
            this.rotate = rotate;
            this.textureDict = textureDict;
            this.textureName = textureName;
            this.drawOnEnts = drawOnEnts;
        }

        public int type { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public float posZ { get; set; }
        public float dirX { get; set; }
        public float dirY { get; set; }
        public float dirZ { get; set; }
        public float rotX { get; set; }
        public float rotY { get; set; }
        public float rotZ { get; set; }
        public float scaleX { get; set; }
        public float scaleY { get; set; }
        public float scaleZ { get; set; }
        public MarkerColor Color { get; set; }
        public bool bobUpAndDown { get; set; }
        public bool faceCamera { get; set; }
        public int p19 { get; set; }
        public bool rotate { get; set; }
        public string textureDict { get; set; }
        public string textureName { get; set; }
        public bool drawOnEnts { get; set; }
    }
}