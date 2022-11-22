
namespace PM_DAL.Data.Entities
{
    public class Clean
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;

        public Clean (int id, string status)
        {
            Id = id;
            Status = status;
        }
    }
}
