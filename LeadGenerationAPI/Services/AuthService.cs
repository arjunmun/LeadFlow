using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LeadGenerationAPI.Models;
using LeadGenerationAPI.Data;

namespace LeadGenerationAPI.Services
{
    public interface IAuthService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly LeadGenerationContext _context;

        public AuthService(LeadGenerationContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {

            var us = await _context.Users.ToListAsync();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return false;

            return VerifyPassword(password, user.PasswordHash, user.Salt);
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                return false;

            var (hash, salt) = HashPassword(password);
            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private (string hash, string salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var hash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
                return (hash, salt);
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
                return computedHash == storedHash;
            }
        }
    }
} 