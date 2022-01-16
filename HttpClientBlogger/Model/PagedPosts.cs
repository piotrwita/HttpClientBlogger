namespace HttpClientBlogger.Model
{
    public class PagedPosts
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public bool NextPage { get; set; }
        public bool PreviousPage { get; set; }
        public IEnumerable<Post> Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
