using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Auth.Commands.Register
{
    public sealed class UserRegisterCommandDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RoleName { get; set; }
        public string GenderName { get; set; }
        public string City {  get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //if sarajevo != City u locationu kao string ako ne postoji onda dodajemo 
    }
}
