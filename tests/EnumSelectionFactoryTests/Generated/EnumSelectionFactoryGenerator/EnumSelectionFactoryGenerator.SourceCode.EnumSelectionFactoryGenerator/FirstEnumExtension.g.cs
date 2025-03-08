
namespace EnumSelectionFactory.Enums
{
    public static partial class FirstEnumExtension
    {
        
        public static string ToStringFast(this EnumSelectionFactoryTests.FirstEnum value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.FirstEnum.None => nameof(EnumSelectionFactoryTests.FirstEnum.None),
                EnumSelectionFactoryTests.FirstEnum.First => nameof(EnumSelectionFactoryTests.FirstEnum.First),
                _ => value.ToString()
            };
        }
        
        public static EnumSelectionFactoryTests.FirstEnum[] GetValues()
        {
            return new[]
            {
                EnumSelectionFactoryTests.FirstEnum.None,
                EnumSelectionFactoryTests.FirstEnum.First,
            };
        }
        
        public static string ToDisplayName(this EnumSelectionFactoryTests.FirstEnum value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.FirstEnum.None => "",
                EnumSelectionFactoryTests.FirstEnum.First => "first",
                _ => value.ToString()
            };
        }
    }
}
