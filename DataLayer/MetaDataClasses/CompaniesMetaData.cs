using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class CompaniesMetaData
    {
        [Key]
        public int CompanyID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "لوگو")]
        public string ImageName { get; set; }
    }

    [MetadataType(typeof(CompaniesMetaData))]
    public partial class Companies { }

}
