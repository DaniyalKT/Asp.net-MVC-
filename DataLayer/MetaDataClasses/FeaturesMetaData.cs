using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class FeaturesMetaData
    {
        [Key]
        public int FeatureID { get; set; }

        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureTitle { get; set; }

    }

    [MetadataType(typeof(FeaturesMetaData))]
    public partial class Features { }
}
