using AspireURLShortener.ApiService.Endpoints;

namespace AspireURLShortener.ApiService.Features.Urls;

public sealed class ShortenUrl
{
    public sealed record Request(string Url);
    public sealed record Response(Guid Id, string OriginalUrl, string ShortenedUrl, string Code, DateTime CreatedAt);
    
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("shorten", Handler).WithTags("Urls");
        }
    }
    
    public static async Task<IResult> Handler(Request request, HttpContext context)
    {
        // Simulate URL shortening logic
        var referenceDate = new DateTime(2025, 06, 17);
        var millisecondsSinceReferenceDate = (long)(DateTime.UtcNow - referenceDate).TotalMilliseconds;
        var httpRequest = context.Request;
        var code = ToBase62(millisecondsSinceReferenceDate);
        var shortenedUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{code}";
        var response = new Response(Guid.NewGuid(), request.Url, shortenedUrl, code, DateTime.UtcNow);

        return TypedResults.Ok(response);
    }
    
    private static readonly string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    
    private static string ToBase62(long value)
    {
        if (value == 0) return Base62Chars[0].ToString();
        
        var result = new Stack<char>();
        while (value > 0)
        {
            result.Push(Base62Chars[(int)(value % 62)]);
            value /= 62;
        }
        
        return new string(result.ToArray());
    }
}