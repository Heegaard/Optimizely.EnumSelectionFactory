//using EnumSelectionFactoryTests;

//namespace EnumSelectionFactory.Enums;

//public static partial class TestExtension
//{

//    public static string ToStringFast(this EnumSelectionFactoryTests.EnumTest value)
//    {
//        return value switch
//        {
//            EnumSelectionFactoryTests.EnumTest.None => nameof(EnumSelectionFactoryTests.EnumTest.None),
//            EnumSelectionFactoryTests.EnumTest.Test => nameof(EnumSelectionFactoryTests.EnumTest.Test),
//            _ => value.ToString()

//        };
//    }

//    public static Enum[] ToStringFast()
//    {
//        return new Enum[0];
//    }
//}