
namespace EnumSelectionFactory.Enums
{
    public static partial class SharedEnumExtension
    {
        
        public static string ToStringFast(this Enum value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.EnumTest propertyEnum => propertyEnum.ToStringFast(),
                EnumSelectionFactoryTests.FirstEnum propertyEnum => propertyEnum.ToStringFast(),
                _ => value.ToString()
            };
        }
        
        public static Enum[] GetValues(this Enum value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.EnumTest propertyEnum => propertyEnum.GetValues(),
                EnumSelectionFactoryTests.FirstEnum propertyEnum => propertyEnum.GetValues(),
                _ => new Enum[0]
            };
        }
        
        public static string ToDisplayName(this Enum value)
        {
            return value switch
            {
                EnumSelectionFactoryTests.EnumTest propertyEnum => propertyEnum.ToDisplayName(),
                EnumSelectionFactoryTests.FirstEnum propertyEnum => propertyEnum.ToDisplayName(),
                _ => value.ToString()
            };
        }
    }
}
