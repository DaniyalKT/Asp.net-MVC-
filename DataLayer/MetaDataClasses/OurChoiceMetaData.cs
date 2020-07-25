using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class OurChoiceMetaData
    {

        [Key]
        public int ChoiceID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage ="کاراکتر های وارد شده بیش از 200 کاراکتر میباشد ")]
        public string ShortDescription { get; set; }

        [Display(Name = "لوگو")]
        public string ImageName { get; set; }
    }

    [MetadataType(typeof(OurChoiceMetaData))]
    public partial class OurChoice { }
}
