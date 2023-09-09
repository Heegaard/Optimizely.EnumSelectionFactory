
namespace EnumSelectionFactory.Enums
{
    public static partial class EnumTestExtension
    {
        
        public static string ToStringFast(this EnumSelectionFactoryTests.EnumTest value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.EnumTest.None => nameof(EnumSelectionFactoryTests.EnumTest.None),
                EnumSelectionFactoryTests.EnumTest.Test => nameof(EnumSelectionFactoryTests.EnumTest.Test),
                _ => value.ToString()
                };
        }
        
        public static EnumSelectionFactoryTests.EnumTest[] GetValues()
        {
            return new[]
            {
                EnumSelectionFactoryTests.EnumTest.None,
                EnumSelectionFactoryTests.EnumTest.Test,
            };
        }
    }
}
