using AutoMapper;
using Booking.Dto;
using Booking.Dtos;
using Booking.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Services
{
    public class AuthService
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        private readonly IConfiguration _configuration;
        public AuthService(
            BookingContext bookingContext,
            IMapper iMapper,
            IConfiguration configuration
        )
        {
            _bookingContext = bookingContext;
            _iMapper = iMapper;
            _configuration = configuration;
        }

        public bool FindMemberAccountExist(string account, out Member? member)
        {
            member = _bookingContext.Members
                        .Where(m => m.Email == account)
                        .SingleOrDefault();
            if (member == null) return false;
            return true;
        }

        public bool FindCompanyAccountExist(string account, out Company? company)
        {
            company = _bookingContext.Companies
                        .Where(c => c.Email == account)
                        .SingleOrDefault();
            if (company == null) return false;
            return true;
        }

        public void RegisterMember(AddMemberRequestDto registerRequest)
        {
            CreatePasswordHash(registerRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Member member = _iMapper.Map<Member>(registerRequest);
            member.Password = passwordHash;
            member.Salt = passwordSalt;

            _bookingContext.Members.Add(member);
            _bookingContext.SaveChanges();
        }

        public void RegisterCompany(AddCompanyRequestDto registerRequest)
        {
            CreatePasswordHash(registerRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Company newCompany = _iMapper.Map<Company>(registerRequest);
            newCompany.Salt = passwordSalt;
            newCompany.Password = passwordHash;

            _bookingContext.Companies.Add(newCompany);
            _bookingContext.SaveChanges();
        }

        public bool VerifyMemberLogin(QueryLoginRequestDto loginRequest, Member? memberFromDB)
        {
            if (loginRequest != null && memberFromDB != null)
            {
                using (var hmac = new HMACSHA512(memberFromDB.Salt))
                {
                    var loingRequestHashPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginRequest.Password));
                    return memberFromDB.Password.SequenceEqual(loingRequestHashPassword);
                }
            }
            return false;
        }

        public bool VerifyCompanyLogin(QueryLoginRequestDto loginRequest, Company? companyFromDB)
        {
            if (loginRequest != null && companyFromDB != null)
            {
                var hmac = new HMACSHA512(companyFromDB.Salt);
                var loginRequestHashPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginRequest.Password));
                return companyFromDB.Password.SequenceEqual(loginRequestHashPassword);
            }
            return false;
        }

        public string? CreateToken(Member? member, Company? company, string role)
        {
            List<Claim>? claims = null;
            switch (role)
            {
                case null:
                    return null;

                case "member":
                    if (member != null) claims = CreateMemberClaims(member);
                    else { return null; }
                    break;

                case "company":
                    if (company != null) claims = CreateCompanyClaims(company);
                    else { return null; }
                    break;

                default:
                    break;
            }


            // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;
        }
        private List<Claim> CreateMemberClaims(Member member)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, member.MemberName),
                new Claim("memberID",member.MemberId.ToString()),
                new Claim(ClaimTypes.Role, "member"),
            };
            return claims;
        }

        private List<Claim> CreateCompanyClaims(Company company)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("companyID", company.CompanyId.ToString()),
                new Claim(ClaimTypes.Role, "company")
            };
            return claims;
        }



        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            //雜湊位元組陣列
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        //public bool GetUserID(int role, out short claimValue)
        //{
        //    var claims = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();
        //    Console.WriteLine("-------------------");
        //    Console.WriteLine("-------------------");
        //    Console.WriteLine("-------------------");
        //    Console.WriteLine(claims);
        //    Console.WriteLine("-------------------");
        //    Console.WriteLine("-------------------");
        //    Console.WriteLine("-------------------");
        //    string? temp = null;
        //    switch (role)
        //    {
        //        case (int)Enums.role.MEMBER:
        //            temp = claims?.FirstOrDefault(x => x.Type.Equals("memberID"))?.Value;
        //            break;
        //        case (int)Enums.role.COMPANY:
        //            temp = claims?.FirstOrDefault(x => x.Type.Equals("companyID"))?.Value;
        //            break;
        //        default:
        //            claimValue = 0;
        //            return false;
        //    }
        //    //return Int16.TryParse(temp, out claimValue);
        //    claimValue = 1;
        //    return true;
        //}
    }
}
