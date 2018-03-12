using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XcConnect.Models.Security{
    /// <summary>
    /// Application User Group
    /// </summary>
    public class ApplicationUserGroup
    {
        [Required, Key]
        [Column(Order = 1)]
        public virtual string UserId { get; set; }
        [Required, Key]
        [Column(Order = 2)]
        public virtual int GroupId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}