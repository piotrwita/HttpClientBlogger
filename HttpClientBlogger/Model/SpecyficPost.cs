namespace HttpClientBlogger.Model
{
    public class SpecyficPost
    {
        public Post Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
