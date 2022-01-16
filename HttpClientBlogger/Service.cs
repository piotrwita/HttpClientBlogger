using HttpClientBlogger.Model;
using System.Net;
using System.Net.Http.Headers;

namespace HttpClientBlogger
{
    public class Service
    {
        //statyczny obiekt ktory pozwoli na wykonywanie zadan http do api
        private readonly HttpClient client;

        public Service()
        {
            client = new HttpClient();
            SetClientSettings();
        }
        private void SetClientSettings()
        {
            //konfiguracja klienta http - podanie adresu api
            client.BaseAddress = new Uri("https://localhost:44322/");
            //domyslny naglowek contenttype typu application/json
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Post
        public void ShowPost(Post post)
        {
            Console.WriteLine($"Id: {post.Id.ToString()} | Title: {post.Title} | Content: {post.Content} | CreationDate: {post.CreationDate.ToShortDateString()}");
        }

        public void ShowPosts(IEnumerable<Post> posts)
        {
            foreach (var post in posts)
            {
                ShowPost(post);
            };
        }

        public async Task<PagedPosts> GetAllPostsAsync()
        {
            PagedPosts pagedPosts = null;
            HttpResponseMessage response = await client.GetAsync("api/posts");
            var odp = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                pagedPosts = await response.Content.ReadAsAsync<PagedPosts>();
            }
            return pagedPosts;
        }

        public async Task<SpecyficPost> GetPostAsync(string path)
        {
            SpecyficPost specyficPost = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                specyficPost = await response.Content.ReadAsAsync<SpecyficPost>();
            }
            return specyficPost;
        }

        //od uri poniewaz w przypadku dodawania nowego zasobu jako rezultat
        //bedziemy oczekiwac lokalizacji pod ktora bedzie mozna znalezc nowo dodana notatke
        //czyli tak naprawde notatke do konkretnej notatki
        public async Task<Uri> CreatePostAsync(Post post)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/posts/", post);
            //upewnienie sie czy operacja powodzeniem sie zakonczyla

            return response.Headers.Location;
        }

        public async Task UpdatePostAsync(Post post)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/posts/", post);

            await response.Content.ReadAsAsync<Post>();
        }

        //zwracamy tylko satus http
        public async Task<HttpStatusCode> DeletePostAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/posts/{id}");

            return response.StatusCode;
        }
        #endregion
        #region Login
        public async Task<AuthSuccessResponse> LoginAsync(LoginModel login)
        {
            AuthSuccessResponse authSuccess = null;

            HttpResponseMessage response = await client.PostAsJsonAsync("api/identity/login", login);

            if (response.IsSuccessStatusCode)
            {
                authSuccess = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            }

            return authSuccess;
        }
        public void Authorization(AuthSuccessResponse authSuccess)
        {
            var token = authSuccess.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        #endregion


    }
}
