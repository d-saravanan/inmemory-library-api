using LibMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LibMS.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        static Random rnd = new Random();
        public HttpResponseMessage GetLoggedInUser()
        {
            var user = UsersRepository.Users.Values.ElementAt(rnd.Next(0, UsersRepository.Users.Keys.Count));
            List<Roles> _userRoles = new List<Roles>();
            _userRoles.AddRange(UserRolesRepository.GetUserRoles(user.Email).SelectMany(x => x.Value));

            List<RolePermissions> _userRolePermissions = new List<RolePermissions>();
            foreach (var userrole in _userRoles)
            {
                _userRolePermissions.Add(RolePermissionsRepository.Get(userrole.Name));
            }

            List<Permissions> _userPermissions = new List<Permissions>();

            foreach (var rp in _userRolePermissions)
            {
                _userPermissions.AddRange(PermissionsRepository.Permissions.Values.Where(p => rp.PermissionIds.Contains(p.Id)));
            }

            var content = new ObjectContent<UserProfile>(new UserProfile
            {
                User = user,
                Roles = _userRoles,
                Permissions = _userPermissions
            }, new JsonMediaTypeFormatter());

            var response = Request.CreateResponse();
            response.Content = content;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }

    public class UserProfile
    {
        public Users User { get; set; }
        public List<Roles> Roles { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
}
