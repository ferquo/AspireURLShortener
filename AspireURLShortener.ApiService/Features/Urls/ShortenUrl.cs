using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;

namespace AspireURLShortener.ApiService.Features.Urls;

public sealed class ShortenUrl
{
    public sealed record Request(string Url);
    
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("shorten", Handler).WithTags("Urls");
        }
    }
    
    public static async Task<IResult> Handler(Request request, HttpContext context, ApplicationDbContext dbContext)
    {
        // Simulate URL shortening logic
        var referenceDate = new DateTime(2025, 06, 17);
        var millisecondsSinceReferenceDate = (long)(DateTime.UtcNow - referenceDate).TotalMilliseconds;
        var httpRequest = context.Request;
        var code = ToBase62(millisecondsSinceReferenceDate);
        var shortenedUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{code}";
        
        var shortenedUrlEntity = new Data.Entities.ShortenedUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = request.Url,
            ShortUrl = shortenedUrl,
            Code = code,
            CreatedOnUtc = DateTime.UtcNow
        };
        
        dbContext.ShortenedUrls.Add(shortenedUrlEntity);
        await dbContext.SaveChangesAsync();

        var response = new ShortenedUrlResponse(shortenedUrlEntity.Id, request.Url, shortenedUrl, code,
            shortenedUrlEntity.CreatedOnUtc);

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