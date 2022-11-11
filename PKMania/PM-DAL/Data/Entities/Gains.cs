
namespace PM_DAL.Data.Entities
{
    public class Gains
    {
        public int GainsSharingNr { get; set; }
        public int StartPlace { get; set; }
        public int EndPlace { get; set; }
        public int NumberChips { get; set; }
        public Decimal Percentage { get; set; }
        public Gains(int gainsSharingNr, int startPlace, int endPlace, int numberChips, decimal percentage)
        {
            GainsSharingNr = gainsSharingNr;
            StartPlace = startPlace;
            EndPlace = endPlace;
            NumberChips = numberChips;
            Percentage = percentage;
        }
    }
}
