using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ContactCommantMetaData
    {
        [Key]
        public int CommantID { get; set; }

        [Display(Name = "نام ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد!")]
        public string Email { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string Title { get; set; }

        [Display(Name = "پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800)]
        [DataType(DataType.MultilineText)]
        public string Massage { get; set; }



    }
    [MetadataType(typeof(ContactCommantMetaData))]
    public partial class ContactCommant { }
}
