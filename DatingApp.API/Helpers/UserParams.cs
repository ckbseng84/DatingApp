namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50; // optional to declare
        public int PageNumber { get; set; } = 1; // default value
        private int pageSize = 10; // default value
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; } // simplify compare with instructor
        }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; }=18;
        public int MaxAge { get; set; }=99;
        public string OrderBy { get; set; }
        public bool Likees { get; set; }=false;
        public bool Likers { get; set; }=false;
    }
}