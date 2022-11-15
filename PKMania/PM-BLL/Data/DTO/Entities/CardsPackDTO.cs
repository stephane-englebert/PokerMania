
namespace PM_BLL.Data.DTO.Entities
{
    public class CardsPackDTO
    {
        public int IndexCrtCard { get; set; }
        public IEnumerable<CardDTO> Pack { get; set; }

        public CardsPackDTO()
        {
            IndexCrtCard = 0;
            Pack = new List<CardDTO>();
        }
    }
}
