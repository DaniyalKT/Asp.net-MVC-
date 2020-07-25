using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ProductCommentMetaData
    {
        public int CommentID { get; set; }
        public Nullable<int> PersonID { get; set; }

        public string Name { get; set; }

        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public System.DateTime CreateDate { get; set; }
    }

    [MetadataType(typeof(ProductCommentMetaData))]
    public partial class Product_Comments { }
}
