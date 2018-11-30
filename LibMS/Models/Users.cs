using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibMS.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class UsersRepository
    {
        public static Dictionary<Guid, Users> Users = new List<Users>
            {
                new Users{Id = Guid.NewGuid(), FirstName = "john", LastName = "smith", Email = "john.smith@example.org"},
                new Users{Id = Guid.NewGuid(), FirstName = "mary", LastName = "kate", Email = "mary.kate@example.org"},
                new Users{Id = Guid.NewGuid(), FirstName = "james", LastName = "philips", Email = "james.philips@example.org"},
                new Users{Id = Guid.NewGuid(), FirstName = "joseph", LastName = "smith", Email = "joseph.smith@example.org"},
                new Users{Id = Guid.NewGuid(), FirstName = "barrack", LastName = "obama", Email = "barrack.obama@example.org"},
                new Users{Id = Guid.NewGuid(), FirstName = "donald", LastName = "trump", Email = "donald.trump@example.org"}
            }.ToDictionary(t => t.Id, t => t);
    }

    public class Roles
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public static class RolesRepository
    {
        public static Dictionary<Guid, Roles> Roles = new List<Roles>
            {
                new Roles{Id = Guid.NewGuid(), Name = "Librarian"},
                new Roles{Id = Guid.NewGuid(), Name = "Member"},
                new Roles{Id = Guid.NewGuid(), Name = "Guest"},
                new Roles{Id = Guid.NewGuid(), Name = "Admin"},
            }.ToDictionary(t => t.Id, t => t);
    }

    public class Permissions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public static class PermissionsRepository
    {
        static Dictionary<Guid, Permissions> _permissions = new Dictionary<Guid, Permissions>();
        public static Dictionary<Guid, Permissions> Permissions = new List<Permissions>
            {
                new Permissions{Id = Guid.NewGuid(), Name = "LendBooks"},
                new Permissions{Id = Guid.NewGuid(), Name = "ViewBooks"},
                new Permissions{Id = Guid.NewGuid(), Name = "ManageBooks"},
                new Permissions{Id = Guid.NewGuid(), Name = "AdministerBooks"},

                new Permissions{Id = Guid.NewGuid(), Name = "LendVideo"},
                new Permissions{Id = Guid.NewGuid(), Name = "ViewVideo"},
                new Permissions{Id = Guid.NewGuid(), Name = "ManageVideo"},
                new Permissions{Id = Guid.NewGuid(), Name = "AdministerVideo"},

                new Permissions{Id = Guid.NewGuid(), Name = "LendAudio"},
                new Permissions{Id = Guid.NewGuid(), Name = "ViewAudio"},
                new Permissions{Id = Guid.NewGuid(), Name = "ManageAudio"},
                new Permissions{Id = Guid.NewGuid(), Name = "AdministerAudio"},
            }.ToDictionary(t => t.Id, t => t);
    }

    public class RolePermissions
    {
        public Guid RoleID { get; set; }
        public Guid[] PermissionIds { get; set; }
    }

    public static class RolePermissionsRepository
    {
        static Dictionary<string, RolePermissions> _rp = new Dictionary<string, RolePermissions>();
        public static RolePermissions Get(string roleName)
        {
            var allRoles = RolesRepository.Roles;

            switch (roleName)
            {
                case "Admin":
                    if (!_rp.ContainsKey(roleName))
                        _rp.Add(roleName,
                    new RolePermissions
                    {
                        RoleID = allRoles.Values.FirstOrDefault(r => r.Name.Contains("Admin")).Id,
                        PermissionIds = PermissionsRepository.Permissions.Keys.ToArray()
                    });
                    return _rp[roleName];
                case "Librarian":
                    if (!_rp.ContainsKey(roleName))
                        _rp.Add(roleName, new RolePermissions
                        {
                            RoleID = allRoles.Values.FirstOrDefault(r => r.Name.Contains("Librarian")).Id,
                            PermissionIds = PermissionsRepository.Permissions.Values.Where(p => p.Name.StartsWith("lend",StringComparison.OrdinalIgnoreCase) || p.Name.StartsWith("view", StringComparison.OrdinalIgnoreCase) || p.Name.StartsWith("manage", StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).ToArray()
                        });
                    return _rp[roleName];
                case "Member":
                    if (!_rp.ContainsKey(roleName))
                        _rp.Add(roleName, new RolePermissions
                        {
                            RoleID = allRoles.Values.FirstOrDefault(r => r.Name.Contains("Member")).Id,
                            PermissionIds = PermissionsRepository.Permissions.Values.Where(p => p.Name.StartsWith("lend", StringComparison.OrdinalIgnoreCase) || p.Name.StartsWith("view", StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).ToArray()
                        });
                    return _rp[roleName];
                case "Guest":
                    if (!_rp.ContainsKey(roleName))
                        _rp.Add(roleName, new RolePermissions
                        {
                            RoleID = allRoles.Values.FirstOrDefault(r => r.Name.Contains("Guest")).Id,
                            PermissionIds = PermissionsRepository.Permissions.Values.Where(p => p.Name.StartsWith("view", StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).ToArray()
                        });
                    return _rp[roleName];
                default:
                    return null;
            }
        }
    }
    public static class UserRolesRepository
    {
        static Dictionary<string, Roles[]> _userRoles = new Dictionary<string, Roles[]>();

        public static Dictionary<string, Roles[]> GetUserRoles(string emailId)
        {
            if (_userRoles.ContainsKey(emailId)) return new Dictionary<string, Roles[]> { { emailId, _userRoles[emailId] } };

            IEnumerable<Users> allUsers = UsersRepository.Users.Values;

            var allRoles = RolesRepository.Roles.Values;

            for (var i = 0; i < allUsers.Count(); i++)
            {
                _userRoles.Add(allUsers.ElementAt(i).Email, new Roles[] { allRoles.ElementAt(i % allRoles.Count) });
            }
            return _userRoles;
        }
    }
}
