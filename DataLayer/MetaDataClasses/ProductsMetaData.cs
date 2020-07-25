using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataLayer
{
    public class ProductsMetaData
    {

        [Key]
        public int ProductID { get; set; }

        [Display(Name = "عنوان کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(350)]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیح کوتاه ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز میباشد !")]
        [MinLength(100, ErrorMessage = "تعداد کاراکتر های وارد شده کمتر از حد مجاز میباشد !")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Text { get; set; }

        [Display(Name = "قیمت اصلی ")]

        public int OldPrice { get; set; }

        [Display(Name = "قیمت تخفیف ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }


        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DisplayFormat(DataFormatString = "{0: yyy/MM/dd}")]
        public System.DateTime CreateDate { get; set; }
    }

    [MetadataType(typeof(ProductsMetaData))]
    public partial class Products { }
}
