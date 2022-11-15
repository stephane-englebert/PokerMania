
using PM_BLL.Data.DTO.Entities;
using PM_DAL.Data.Entities;

namespace PM_BLL.Data.Mappers
{
    internal static class PlayerMappers
    {
        public static PlayerDTO PlayerDalToDTO(this Player player)
        {
            //PlayerDTO py = new PlayerDTO();
            //py.Id = player.Id;
            //py.Pseudo = player.Pseudo;
            //py.Eliminated = player.EliminatedAt.Year > 2000;
            //py.SittingAtTable = player.TableNr;
            //py.Stack = player.Stack;
            //py.BonusTime = player.BonusTime;
            //return py;
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
    }
}
