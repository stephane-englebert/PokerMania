namespace PM_BLL.Data.DTO.Entities
{
    public class ShuffleCardsDTO
    {
        public Guid shuffleGuid { get; set; }
        public CardDTO Card { get; set; }

        public ShuffleCardsDTO(CardDTO aCard)
        {
            shuffleGuid = Guid.NewGuid();
            Card = new CardDTO(aCard.Value, aCard.Family, aCard.Abbreviation);
        }
    }
}
