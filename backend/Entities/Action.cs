namespace backend.entity
{
    public partial class Action
    {
        public Action()
        {
            Tasks = new HashSet<Task>();
        }

        public int ActionId { get; set; }
        public string Action1 { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
