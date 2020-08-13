namespace Server.Domain.Entities
{
    public class Org
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Leader { get; set; }
        public float SpawnX { get; set; }
        public float SpawnY { get; set; }
        public float SpawnZ { get; set; }
    }
}