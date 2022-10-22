namespace backend.services 
{
    using backend.entity;
    using backend.model;
    using System.Text;
    using System.Security.Cryptography;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using backend.services;
    using Serilog;
    using Swashbuckle.AspNetCore.Annotations;

    public class UserService : IUserService
    {
        private static readonly dbContext _context = new dbContext();
        public static string SessionIdToken = "session-id";

         public async Task<bool> Login(Login input) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == input.UserName);
            if(user == null)
            {
                return false;
            }
            // if(user.VerifiedAt == null)
            // {
            //     return false;
            // }
            if(!VerifyPasswordHash(input.Password, user.PasswordHash, user.PasswordSalt))
            {
                return false;
            }

            return true;
         }

         public async Task<bool> Register(Register input) {
            if(_context.Users.Any(u => u.UserName == input.UserName)) 
            {
                return false;
            }

            CreatePasswordHash(input.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new User
            {
                UserName = input.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserIsAdmin = false,
                UserAiSwitchedOnPreference = true,
                UserIsAccountLocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
         }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken() {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}