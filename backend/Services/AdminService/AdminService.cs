using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using backend.entity;
using Newtonsoft.Json;
using Task = backend.entity.Task;

namespace backend.services
{
    public class ResponseMessage
    {
        public int TaskId { get; set; }
        public DateTime TaskTime { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public bool? UserIsAccountLocked { get; set; }
        public string ActionName { get; set; } = null!;

    }

    public class AdminService : IAdminService
    {
        private readonly IConfiguration Configuration;
        private static readonly dbContext _context = new dbContext();
        private static readonly dbContext _context2 = new dbContext();
        public List<ResponseMessage> Response { get; set; }
        public AdminService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string getTaskHistory(string adminName)
        {
            var role = _context.Users.FirstOrDefaultAsync(u => u.UserName == adminName).Result!.UserIsAdmin;
            if (role == true)
            {
                List<ResponseMessage> objectList = new List<ResponseMessage>();
                var tasks = _context.Tasks;
                foreach(var task in tasks) {
                    var Action = _context2.Actions.FirstOrDefault(a => a.ActionId == task.ActionId);
                    var User = _context2.Users.FirstOrDefault(u => u.UserId == task.UserId);

                    objectList.Add(new ResponseMessage 
                    {
                        UserId = task.UserId, 
                        TaskTime = task.TaskTime, 
                        ActionId = task.ActionId, 
                        TaskId = task.TaskId,
                        UserName = User.UserName,
                        UserIsAccountLocked = User.UserIsAccountLocked,
                        ActionName = Action.Action1
                    }
                    );
                }
                string json = JsonConvert.SerializeObject(objectList);
                return json;
            }
            else
            {
                return "Unauthorized";
            }
        }

        public string toggleAccountlock(string adminName, int userId) {
            var role = _context.Users.FirstOrDefaultAsync(u => u.UserName == adminName).Result!.UserIsAdmin;
            if (role == true)
            {
                var user = _context.Users.FirstOrDefaultAsync(u => u.UserId == userId).Result;

                user.UserIsAccountLocked = !user.UserIsAccountLocked;
                _context.Users.Update(user);
                _context.SaveChanges();
                var msg = "";
                if(user.UserIsAccountLocked == true) {
                    msg = "locked";
                } else {
                    msg = "unlocked";
                }
                return $"{user.UserName}'s account is now {msg}.";
            }
            else
            {
                return "Unauthorized";
            }
        }

        public bool logEvent(int eventType, string username)
        {
            try{
                var userId = _context.Users.FirstOrDefaultAsync(u => u.UserName == username).Result.UserId;
                var entry = new Task
                {
                    TaskTime = DateTime.Now,
                    ActionId = eventType,
                    UserId = (int) userId
                };
                _context.Tasks.Add(entry);
                _context.SaveChanges();
                return true;
            } catch(Exception) {
                return false;
            }
        }
    }
}