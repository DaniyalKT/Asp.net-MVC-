using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ContactUsMetaData
    {
        [Key]
        public int ContactID { get; set; }

        [Display(Name = "توضیح کوتاه")]
        [MaxLength()]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "تلفن تماس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معبتر نمیباشد ")]
        public string Email { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500)]
        public string Address { get; set; }
    }
    [MetadataType(typeof(ContactUsMetaData))]
    public partial class ContactUs { }
}
