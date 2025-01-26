namespace Server.Domain.Entities
{
    public class Org
    {
        public string Id { get; }
        public string Name { get; }
        public string Leader { get; private set; }
        public float SpawnX { get; }
        public float SpawnY { get; }
        public float SpawnZ { get; }

        public void SetLeader(Account account)
        {
            this.Leader = account.Username;
        }
    }
}