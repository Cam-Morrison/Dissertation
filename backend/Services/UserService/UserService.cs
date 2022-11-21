namespace backend.services 
{
    using backend.entity;
    using backend.model;
    using System.Security.Cryptography;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;

    public class UserService : IUserService
    {
        private static readonly dbContext _context = new dbContext();
        public static string SessionIdToken = "session-id";
        private readonly IConfiguration _configuration;
        private MarketDataService _marketDataService;

        public UserService(IConfiguration configration, MarketDataService marketDataService){
            _configuration = configration;
            _marketDataService = marketDataService;
        }

        public async Task<string> Login(Login input) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == input.UserName);
            if(user == null)
            {
                return "Username invalid.";
            }
            if(!VerifyPasswordHash(input.Password, user.PasswordHash, user.PasswordSalt))
            {
                return "Password invalid.";
            }
            return CreateToken(user);
        }

         public async Task<string> Register(Register input) {
            if(_context.Users.Any(u => u.UserName == input.UserName)) 
            {
                return "User already exists.";
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
            
            return CreateToken(user);
         }

        private string CreateToken(User user) {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
            (_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
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

        public string AddToWatchlist(string username, string ticker)
        {
            if (_context.Users.Any(u => u.UserName == username))
            {
                if (_marketDataService.isStockValid(ticker))
                {
                    var user = _context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result;
                    var watchlist = _context.Watchlists.FirstOrDefaultAsync(u => u.UserId == user!.UserId).Result;
                    if (watchlist == null)
                    {
                        var entry = new Watchlist
                        {
                            UserId = (int)user!.UserId!,
                            WatchListName = "My watchlist",
                            Stocks = ticker
                        };
                        _context.Watchlists.Add(entry);
                        _context.SaveChanges();
                        return "Added to watchlist.";

                    }
                    else
                    {
                        if (!watchlist.Stocks.ToString().Contains(ticker))
                        {
                            watchlist.Stocks = watchlist.Stocks.ToString() + $", {ticker}";
                            _context.Update(watchlist);
                            _context.SaveChanges();
                            return "Added to watchlist.";
                        }
                        return "Stock is already in watchlist.";
                    }
                }
                return "Invalid stock ticker.";
            }
            return "Unauthorized";
        }

        public string RemoveFromWatchList(string ticker) {
            return "TODO";
        }
    }
}