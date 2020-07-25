using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class SocialNetworkMetaData
    {
        [Key]
        public int NetwordID { get; set; }

        [Display(Name = "لوگو")]
        public string Logo { get; set; }

        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Url(ErrorMessage = "لینک وارد شده معتبر نمیباشد")]
        public string Url { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
    }

    [MetadataType(typeof(SocialNetworkMetaData))]
    public partial class SocialNetwork { }
}
