namespace Project.Identity.Models
{
    public class RoleAssignListModel
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; }
    }

    public class RoleAssignSendModel
    {
        public List<RoleAssignListModel> Roles { get; set; }
        public string UserId { get; set; }
    }
}
