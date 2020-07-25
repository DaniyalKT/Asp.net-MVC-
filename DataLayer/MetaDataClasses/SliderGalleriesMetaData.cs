using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class SliderGalleriesMetaData
    {
        [Key]
        public int GalleryID { get; set; }

        [Display(Name = "اسلایدر")]
        public int SliderID { get; set; }

        [Display(Name = "عنوان تصویر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }
    }
    [MetadataType(typeof(SliderGalleriesMetaData))]
    public partial class Slider_Galleries { }
}
