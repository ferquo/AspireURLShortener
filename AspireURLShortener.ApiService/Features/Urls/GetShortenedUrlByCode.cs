using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspireURLShortener.ApiService.Features.Urls;

public sealed class GetShortenedUrlByCode
{
    public sealed record Request(string Code);
    public sealed record Response(ShortenedUrlResponse ShortenedUrl);
    
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("urls/{code}", Handler).WithTags("Urls");
        }
    }
    
    public static async Task<IResult> Handler(ApplicationDbContext dbContext, [FromRoute]string code)
    {
        var shortenedUrl = await dbContext.ShortenedUrls
            .Where(u => u.Code == code)
            .Select(u => new ShortenedUrlResponse(u.Id, u.LongUrl, u.ShortUrl, u.Code, u.CreatedOnUtc))
            .FirstOrDefaultAsync();

        if (shortenedUrl == null)
        {
            return TypedResults.NotFound();
        }

        var response = new Response(shortenedUrl);
        
        return TypedResults.Ok(response);
    }
}