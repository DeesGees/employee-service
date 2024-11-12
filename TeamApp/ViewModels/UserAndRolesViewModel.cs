using TeamApp.Models;

namespace TeamApp.ViewModels
{
    public class UserAndRolesViewModel
    {
        public List<Roles> Role { get; set; }
        public List<Users> User { get; set; }
        public Users SingleUser { get; set; }
        public Roles SingleRoles { get; set; }
        public string SelectedRole { get; set; }
        public string SelectedUserId { get; set; }

    }
}
