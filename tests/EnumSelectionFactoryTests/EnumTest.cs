﻿using EnumSelectionFactory.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EnumSelectionFactoryTests
{
    [EnumExtension]
    public enum EnumTest
    {
        [Display(Name = "None", ShortName = "")]
        None = 0,

        [Display(Name = "Test", ShortName = "test")]
        Test = 1
    }
}
