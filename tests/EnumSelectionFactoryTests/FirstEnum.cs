using EnumSelectionFactory.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EnumSelectionFactoryTests
{
    [EnumExtension]
    public enum FirstEnum
    {
        [Display(Name = "None", ShortName = "")]
        None = 0,

        [Display(Name = "First", ShortName = "first")]
        First = 1
    }
}
