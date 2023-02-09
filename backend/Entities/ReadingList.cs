namespace backend.entity
{
    public partial class ReadingList
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ArticleId {get; set;}
        public int UserId { get; set; } 
        public string article {get; set;} = null!;
    }
}
