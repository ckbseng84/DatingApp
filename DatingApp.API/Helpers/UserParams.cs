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
        
    }
}