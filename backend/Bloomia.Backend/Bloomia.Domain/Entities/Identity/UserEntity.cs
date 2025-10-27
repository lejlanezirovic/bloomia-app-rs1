using Bloomia.Domain.Entities.Basics;
using Bloomia.Domain.Entities.Enums;
using Bloomia.Domain.Common;
using Bloomia.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Identity
{
    public class UserEntity: BaseEntity
    {
        public string Firstname {  get; set; }  
        public string Lastname { get; set; }
        public string? Username { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";
        public DateTime DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }

        public string Email { get; set; }   
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PhoneNumber { get; set; }


        public int? GenderId { get; set; }
        public GenderEntity? Gender { get; set; }
        public int? LocationId { get; set; }
        public LocationEntity? Location { get; set; }
        public int? LanguageId { get; set; }
        public LanguageEntity? Language { get; set; }

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public int TokenVersion { get; set; } = 0;// For global revocation
        public bool IsEnabled { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();

    }
}
