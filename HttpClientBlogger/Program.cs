using HttpClientBlogger;
using HttpClientBlogger.Model;
using System.Net.Http.Headers;

RunAsync().GetAwaiter().GetResult();

static async Task RunAsync()
{
    var service = new Service();
    try
    { 
        //Login
        LoginModel login = new LoginModel
        {
            UserName = "piotruser1",
            Password = "Pa$$w0rd123!"
        };

        var authSuccess = await service.LoginAsync(login);

        //Authrization
        service.Authorization(authSuccess);
        Console.WriteLine($"You are logged as '{login.UserName}'");

        //Get all notes
        var pagedPosts = await service.GetAllPostsAsync();
        service.ShowPosts(pagedPosts.Data);

        //Create a new note
        Post post = new Post
        {
            Title = "nowy tytul1",
            Content = "nowa tresc1"
        };

        var url = await service.CreatePostAsync(post);
        Console.WriteLine($"Created at {url.OriginalString}");

        //Get the note
        var specyficPost = await service.GetPostAsync(url.OriginalString);
        post = specyficPost.Data;
        service.ShowPost(post);

        //Update the note
        Console.WriteLine("Updating content...");
        post.Content = "zaktualizowana tres";
        await service.UpdatePostAsync(post);

        //Get update note
        var updatedPost = await service.GetPostAsync(url.OriginalString);
        post = updatedPost.Data;
        service.ShowPost(post);

        //Delete the note
        var statusCode = await service.DeletePostAsync(post.Id);
        Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
    }

    Console.ReadLine();
}