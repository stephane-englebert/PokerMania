
using PM_BLL.Data.DTO.Entities;
using PM_DAL.Data.Entities;

namespace PM_BLL.Data.Mappers
{
    internal static class PlayerMappers
    {
        public static PlayerDTO PlayerDalToDTO(this Player player)
        {
            return new PlayerDTO
            {
                Id = player.Id,
                Pseudo = player.Pseudo,
                Eliminated = player.EliminatedAt.Year > 2000,
                SittingAtTable = player.TableNr,
                Stack = player.Stack,
                BonusTime = player.BonusTime
            };
        }
        public static RankedPlayerDTO PlayerDTOToRankedPlayerDTO(this PlayerDTO player)
        {
            return new RankedPlayerDTO
            {
                Rank = player.GeneralRanking,
                TableNr = player.SittingAtTable,
                PlayerId = player.Id,
                Pseudo = player.Pseudo,
                Stack = player.Stack,
                Eliminated = player.Eliminated
            };
        }
    }
}
