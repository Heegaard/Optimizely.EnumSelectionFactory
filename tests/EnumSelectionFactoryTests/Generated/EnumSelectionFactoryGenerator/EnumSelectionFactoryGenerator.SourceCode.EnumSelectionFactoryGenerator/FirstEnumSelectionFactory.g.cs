using EPiServer.Shell.ObjectEditing;
using EnumSelectionFactory.Enums;

namespace EnumSelectionFactory.SelectionFactories
{
    public class FirstEnumSelectionFactory : ISelectionFactory
    {
        
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var enumValues = FirstEnumExtension.GetValues();
            foreach (var item in enumValues)
            {
                yield return new SelectItem { Value = item, Text = item.ToStringFast() };
            }
        }
    }
}
