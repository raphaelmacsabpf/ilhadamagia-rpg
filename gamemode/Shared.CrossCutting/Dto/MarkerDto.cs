namespace Shared.CrossCutting.Dto
{
    public class MarkerDto
    {
        public MarkerDto(int type, float posX, float posY, float posZ, float dirX, float dirY, float dirZ, float rotX, float rotY, float rotZ, float scaleX, float scaleY, float scaleZ, MarkerColor color, bool bobUpAndDown, bool faceCamera, int p19, bool rotate, string textureDict, string textureName, bool drawOnEnts)
        {
            this.Type = type;
            this.PosX = posX;
            this.PosY = posY;
            this.PosZ = posZ;
            this.DirX = dirX;
            this.DirY = dirY;
            this.DirZ = dirZ;
            this.RotX = rotX;
            this.RotY = rotY;
            this.RotZ = rotZ;
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.ScaleZ = scaleZ;
            this.Color = color;
            this.BobUpAndDown = bobUpAndDown;
            this.FaceCamera = faceCamera;
            this.P19 = p19;
            this.Rotate = rotate;
            this.TextureDict = textureDict;
            this.TextureName = textureName;
            this.DrawOnEnts = drawOnEnts;
        }

        public int Type { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float DirX { get; set; }
        public float DirY { get; set; }
        public float DirZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public MarkerColor Color { get; set; }
        public bool BobUpAndDown { get; set; }
        public bool FaceCamera { get; set; }
        public int P19 { get; set; }
        public bool Rotate { get; set; }
        public string TextureDict { get; set; }
        public string TextureName { get; set; }
        public bool DrawOnEnts { get; set; }
    }
}