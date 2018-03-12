using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models.Security
{
    public class ApplicationRoleGroup
    {
        [Required, Key]
        [Column(Order = 1)]
        public virtual string RoleId { get; set; }

        [Required, Key]
        [Column(Order = 2)]
        public virtual int GroupId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual Group Group { get; set; }
    }
}