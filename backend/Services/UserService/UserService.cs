namespace backend.services
{
    using backend.entity;
    using backend.model;
    using System.Security.Cryptography;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using ProfanityFilter;

    public class UserService : IUserService
    {
        private dbContext _context;
        public static string SessionIdToken = "session-id";
        private readonly IConfiguration _configuration;
        private MarketDataService _marketDataService;
        private AdminService log;
        private static JToken watchListCached;
        private Boolean watchListChange = true;

        public UserService(IConfiguration configration, MarketDataService marketDataService){
            _configuration = configration;
            _context = new dbContext(_configuration);
            _marketDataService = marketDataService;
            this.log = new AdminService(_configuration);
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
            if(isAccountLocked(input.UserName) == true) {
                return "Account locked by admin.";
            }
            AdminService log = new AdminService(_configuration);
            log.logEvent(1, input.UserName);

            return CreateToken(user);
        }

        public Boolean isAccountLocked(string username) {
            return (bool)_context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result.UserIsAccountLocked;
        }

         public async Task<string> Register(Register input) {
            var filter = new ProfanityFilter();
            if(filter.ContainsProfanity(input.UserName) == true) {
                return "Please do not use any rude words.";
            }
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
            log.logEvent(2, input.UserName);
            return CreateToken(user);
         }

        private string CreateToken(User user) {
            var role = "User";
            if(user.UserIsAdmin == true) {
                role = "Administrator";
            }

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

            token.Payload["user"] = user.UserName;
            token.Payload["role"] = role;
            token.Payload["AIpreference"] = user.UserAiSwitchedOnPreference.ToString();
            
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

        public string AddToWatchlist(string user, string ticker)
        {
            try {
                int userId = (int)_context.Users.FirstOrDefaultAsync(u => u.UserName == user).Result!.UserId!;
                if (_marketDataService.IsStockValid(ticker))
                {
                    var watchlist = _context.Watchlists.FirstOrDefaultAsync(u => u.UserId == userId).Result;
                    if (watchlist == null)
                    {
                        List<string> mylist = new List<string>(new string[] { ticker });
                        var entry = new Watchlist
                        {
                            UserId = userId,
                            WatchListName = "My watchlist",
                            Stocks = mylist.ToString()
                        };
                        _context.Watchlists.Add(entry);
                        _context.SaveChanges();
                        watchListChange = true;
                        log.logEvent(4, user);
                        return "Added to watchlist.";
                    }
                    else
                    {
                        if (!watchlist.Stocks!.ToString().Contains(ticker))
                        {
                            List<string> result =  watchlist.Stocks.Split(',').ToList();
                            result.Add(ticker);
                            watchlist.Stocks = string.Join(",", result);
                            _context.Update(watchlist);
                            _context.SaveChanges();
                            watchListChange = true;
                            log.logEvent(4, user);
                            return "Added to watchlist.";
                        }
                        return "Stock is already in watchlist.";
                    }
                } 
                else
                {
                    return "That stock does not exist, please use the ticker.";
                }
            } catch (Exception) 
            {
                return "Issue with user claim, username invalid.";
            }
        }

        public string GetWatchlist(string username) {
            int userId = (int)_context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result!.UserId!;
            var watchlist = _context.Watchlists.FirstOrDefaultAsync(u => u.UserId == userId).Result;
            if(watchlist == null)
            {
                return "There are no items in the watchlist.";
            }
            var itemToAdd = new JObject();
            itemToAdd["title"] = watchlist.WatchListName;
            if(watchListChange == true) {
                watchListCached = (JToken?)JsonConvert.DeserializeObject<Object>(_marketDataService.GetListPrices(watchlist.Stocks!.ToString()));
                watchListChange = false;
            } 
            itemToAdd["stocks"] = watchListCached;
            
            var jsonToOutput = JsonConvert.SerializeObject(itemToAdd, Formatting.Indented);
            return jsonToOutput;
        }

        public string RemoveFromWatchList(string username, string ticker) {

            int userId = (int)_context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result!.UserId!;
            var watchlist = _context.Watchlists.FirstOrDefaultAsync(u => u.UserId == userId).Result;
            List<string> result =  watchlist.Stocks.Split(',').ToList();
            try {
                if (result.Contains(ticker))
                {
                    result.Remove(ticker);
                    watchlist.Stocks = string.Join(",", result);
                    _context.Watchlists.Update(watchlist);
                    _context.SaveChanges();
                    watchListChange = true;
                    log.logEvent(5, username);
                    return "Watchlist updated.";
                }
                return "That stock is not in the watchlist.";
            } 
            catch (Exception) 
            {
                return "Watchlist does not exist.";
            }
        } 

        public string UpdateWatchListTitle(string username, string newTitle) {
            if(newTitle.Length < 3 || newTitle.Length > 16) {
                return "Watchlist length is too short or long (3-16)";
            }
            var filter = new ProfanityFilter();
            if(filter.ContainsProfanity(newTitle) == true) {
                return "Please do not use any rude words.";
            }
            int userId = (int)_context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result!.UserId!;
            var watchlist = _context.Watchlists.FirstOrDefaultAsync(u => u.UserId == userId).Result;
            if(watchlist != null)
            {
                watchlist.WatchListName = newTitle;
                _context.Watchlists.Update(watchlist);
                _context.SaveChanges();
                log.logEvent(6, username);
                return $"Watchlist title is now {newTitle}.";
            }
            return "Watchlist hasn't been created yet.";
        }
        
        public string toggleAIpreference(string username) {
            var user = _context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result!;
            user.UserAiSwitchedOnPreference = !user.UserAiSwitchedOnPreference;
            _context.Users.Update(user);
            _context.SaveChanges();
            var msg = "";
            if(user.UserAiSwitchedOnPreference == true) {
                log.logEvent(8, username);
            } else {
                log.logEvent(7, username);
            }
            return CreateToken(user);
            
        }

        public string LogSignOut(string username) {
            log.logEvent(3, username);
            return "Action Logged";
        }
    }
}