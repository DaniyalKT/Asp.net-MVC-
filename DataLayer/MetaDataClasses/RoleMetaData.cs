using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DataLayer
{
   public class RoleMetaData
    {
        public int RoleID { get; set; }

        [Display(Name = "نقش کاربر")]
        public string RoleTitle { get; set; }
        public string RoleName { get; set; }
    }


    [MetadataType(typeof(RoleMetaData))]
    public partial class Roles
    {

    }
}
