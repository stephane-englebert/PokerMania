using PM_BLL.Data.DTO.Forms;
using PM_BLL.Interfaces;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.ComponentModel.DataAnnotations;
using PM_DAL.Services;

namespace PM_BLL.Services
{
    public class MembersService: IMembersService
    {
        private readonly IMemberRepository _memberRepository = new MemberRepository();

        public MembersService()
        {
        }
        public void AddMember(MemberRegisterFormDTO member)
        {
            Member memb = new Member();
            memb.Id = 0;
            memb.Role = "player";
            memb.Pseudo = member.Pseudo;
            memb.Email = member.Email;
            memb.Bankroll = 15001;
            memb.IsPlaying = false;
            memb.IsDisconnected = true;
            memb.CurrentTournament = 0;
            if (_memberRepository.ExistEmail(memb.Email))
            {
                throw new ValidationException("REGISTER.BLL.EXISTS_EMAIL");
            }
            if (_memberRepository.ExistPseudo(memb.Pseudo))
            {
                throw new ValidationException("REGISTER.BLL.EXISTS_PSEUDO");
            }
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: member.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
            _memberRepository.AddMember(memb, hashed, salt);
        }

        public void SetAllRegisteredMembersCurrentTournId(int trId)
        {
                _memberRepository.SetAllRegisteredMembersCurrentTournId(trId);
        }
        public void SetPlayerIsConnected(int playerId)
        {
            _memberRepository.SetPlayerIsConnected(playerId);
        }
        public void SetPlayerIsDisconnected(int playerId)
        {
            _memberRepository.SetPlayerIsDisconnected(playerId);
        }
        public IEnumerable<int> GetIdOfDisconnectedPlayers(int trId)
        {
            return _memberRepository.GetIdOfDisconnectedPlayers(trId);
        }
    }
}
