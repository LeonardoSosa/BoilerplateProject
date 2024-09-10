using Abp.Authorization;
using BoilerplateProject.Authorization.Roles;
using BoilerplateProject.Authorization.Users;

namespace BoilerplateProject.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
