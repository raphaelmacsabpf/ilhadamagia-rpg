namespace Server.Domain.Entities
{
    public class OrgMembership
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int OrgId { get; set; }
        public int Role { get; set; }
    }
}
