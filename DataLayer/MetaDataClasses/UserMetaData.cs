using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class UserMetaData
    {
        public int UserID { get; set; }

        [Display(Name = "نقش کاربر")]
        public int RoleID { get; set; }

        [Display(Name = "نام کاربری ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد!")]
        public string Email { get; set; }


        [Display(Name = "تلفن همراه")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کدپستی")]
        public Nullable<int> PostalCode { get; set; }


        [Display(Name = "آدرس")]
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }


        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "کد فعال سازی")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public System.DateTime RegisterDate { get; set; }





    }

    [MetadataType(typeof(UserMetaData))]
    public partial class Users { }
}
