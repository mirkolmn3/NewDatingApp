using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewDatingApp.API.Models;

namespace NewDatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        #region PROPERTIES
        private readonly DataContext context;
        #endregion PROPERTIES

        #region CONSTRUCTOR
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }
        #endregion CONSTRUCTOR

        #region PUBLIC METHODS
        public async Task<User> Login(string userName, string password)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await this.context.Users.AnyAsync(u => u.UserName == userName))
                return true;
            else
                return false;

        }
        #endregion PUBLIC METHODS

        #region PRIVATE METHODS
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHass = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHass.Length; i++)
                {
                    if (computedHass[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        #endregion
    }
}