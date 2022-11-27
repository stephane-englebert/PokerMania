
namespace PM_BLL.Data.DTO.Entities
{
    public class CardDTO
    {
        public string Value { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public CardDTO(string value, string family, string abbreviation)
        {
            Value = value;
            Family = family;
            Abbreviation = abbreviation;
        }
    }
}
