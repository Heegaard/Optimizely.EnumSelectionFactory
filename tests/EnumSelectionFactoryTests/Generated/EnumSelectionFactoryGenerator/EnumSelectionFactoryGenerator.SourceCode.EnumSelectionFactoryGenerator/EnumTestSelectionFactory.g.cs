using EPiServer.Shell.ObjectEditing;
using EnumSelectionFactory.Enums;

namespace EnumSelectionFactory.SelectionFactories
{
    public class EnumTestSelectionFactory : ISelectionFactory
    {
        
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var enumValues = EnumTestExtension.GetValues();
            foreach (var item in enumValues)
            {
                yield return new SelectItem { Value = item, Text = item.ToStringFast() };
            }
        }
    }
}
