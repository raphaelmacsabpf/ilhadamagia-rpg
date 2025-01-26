namespace GF.CrossCutting.Dto
{
    public class OrgMembershipDto
    {
        public string Username { get; set; }
        public string OrgId { get; set; }
        public int Role { get; set; }

        public override string ToString()
        {
            return $"Username: {Username}, OrgId: {OrgId}, Role: {Role}";
        }
    }
}
