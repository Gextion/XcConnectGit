using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XcConnect.Models.Security
{
    public class IdentityManager
    {
        // Swap ApplicationRole for IdentityRole:
        private readonly SecurityDbContext _db = new SecurityDbContext();

        private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new SecurityDbContext()));

        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()));

        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }

        public ApplicationRole GetRole(string RoleId)
        {
            return _roleManager.FindById(RoleId);
        }

        public IdentityResult CreateRole(string name, string description = "")
        {
            // Swap ApplicationRole for IdentityRole:
            return _roleManager.Create(new ApplicationRole(name, description));
        }

        public void ClearGroups(ApplicationUser user)
        {
            ClearUserGroups(user.Id);
        }

        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            var AppUser = _userManager.FindByName(user.UserName);
            if (AppUser == null)
            {
                return _userManager.Create(user, password);
            }

            return null;
        }

        public ApplicationUser GetUser(string UserName)
        {
            return _userManager.FindByName(UserName);
        }

        public IdentityResult AddUserToRole(string userId, string roleName)
        {
            return _userManager.AddToRole(userId, roleName);
        }


        public void ClearUserRoles(string userId)
        {
            ApplicationUser user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                _userManager.RemoveFromRole(userId, role.RoleId);
            }
        }

        public void RemoveFromRole(string userId, string roleName)
        {
            _userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(string roleId)
        {
            IQueryable<ApplicationUser> roleUsers = _db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = _db.Roles.Find(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                RemoveFromRole(user.Id, role.Name);
            }
            _db.Roles.Remove(role);
            _db.SaveChanges();
        }

        public void CreateGroup(string groupName)
        {
            if (!GroupNameExists(groupName))
            {
                var newGroup = new Group(groupName);
                _db.Groups.Add(newGroup);
                _db.SaveChanges();
            }
        }

        public bool GroupNameExists(string groupName)
        {
            return _db.Groups.Any(gr => gr.Name == groupName);
        }


        public void ClearUserGroups(string userId)
        {
            ClearUserRoles(userId);
            ApplicationUser user = _db.Users.Find(userId);
            user.Groups.Clear();
            _db.SaveChanges();
        }

        public void AddUserToGroup(string userId, int groupId)
        {
            Group group = _db.Groups.Find(groupId);
            ApplicationUser user = _db.Users.Find(userId);

            var userGroup = new ApplicationUserGroup
            {
                Group = group,
                GroupId = group.Id,
                User = user,
                UserId = user.Id
            };

            var Roles = GetRolesForUser(userId);

            foreach (ApplicationRoleGroup role in group.Roles)
            {
                bool Exist = false;

                if (Roles != null)
                {
                    Exist = (Roles.Where(r => r.Name == role.Role.Name).Count() > 0);
                }

                if (!Exist)
                   _userManager.AddToRole(userId, role.Role.Name);
            }

            if (user.Groups != null)
            {
                bool Exist = false;

                foreach (var groupCol in user.Groups)
                {
                    if (groupCol.GroupId.Equals(userGroup.GroupId) && groupCol.UserId.Equals(userGroup.UserId))
                    {
                        Exist = true;
                        break;
                    }
                }

                if (!Exist)
                {
                    user.Groups.Add(userGroup);
                    _db.SaveChanges();
                }
            }
        }

        public void ClearGroupRoles(int groupId)
        {
            Group group = _db.Groups.Include(g => g.Roles).Where(g => g.Id == groupId).FirstOrDefault();
            if (group != null && group.Roles != null && group.Roles.Count > 0)
            {
                List<ApplicationRoleGroup> Roles = group.Roles.ToList();
                List<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id)).ToList();

                foreach (ApplicationRoleGroup role in Roles)
                {
                    foreach (ApplicationUser user in groupUsers)
                    {
                        int groupsWithRole = user.Groups.Count(g => g.Group.Roles.Any(r => r.RoleId == role.RoleId));

                        // This will be 1 if the current group is the only one:
                        if (groupsWithRole == 1)
                        {
                            //ClearUserGroups(user.Id);
                            RemoveFromRole(user.Id, role.Role.Name);
                        }
                    }
                }
                group.Roles.Clear();
                _db.SaveChanges();
            }
        }

        public void AddRoleToGroup(int groupId, string roleName)
        {
            Group group = _db.Groups.Find(groupId);
            ApplicationRole role = _db.Roles.First(r => r.Name == roleName);
            
            var newgroupRole = new ApplicationRoleGroup
            {
                GroupId = group.Id,
                Group = group,
                RoleId = role.Id,
                Role = role
            };

            bool Exist = false;

            foreach (var roleCol in group.Roles)
            {
                if (roleCol.GroupId.Equals(newgroupRole.GroupId) && roleCol.RoleId.Equals(newgroupRole.RoleId))
                {
                    Exist = true;
                    break;
                }
            }

            if (!Exist)
            {
                group.Roles.Add(newgroupRole);
                _db.SaveChanges();
            }

            // Add all of the users in this group to the new role:
            IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            foreach (ApplicationUser user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    AddUserToRole(user.Id, role.Name);
                }
            }
        }

        public void DeleteGroup(int groupId)
        {
            Group group = _db.Groups.Find(groupId);

            // Clear the roles from the group:
            ClearGroupRoles(groupId);
            _db.Groups.Remove(group);
            _db.SaveChanges();
        }

        public List<ApplicationRole> GetRolesForUser(string UserId)
        {
            List<ApplicationRole> Roles = new List<ApplicationRole>();
            var RolesIds = _userManager.GetRoles(UserId);
            RolesIds = RolesIds.OrderBy(x => x).ToList();
            for (int i = 0; i < RolesIds.Count; i++)
            {
                var Role = _roleManager.FindByName(RolesIds[i]);
                if (Role != null && !Roles.Contains(Role))
                {
                    Roles.Add(Role);
                }
            }

            return Roles;
        }
    }

    [Serializable]
    public class GroupExistsException : Exception
    {
        public GroupExistsException()
        {
        }

        public GroupExistsException(string message) : base(message)
        {
        }

        public GroupExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GroupExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}