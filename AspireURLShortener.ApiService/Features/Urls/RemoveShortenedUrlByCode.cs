using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspireURLShortener.ApiService.Features.Urls;

public sealed class RemoveShortenedUrlByCode
{
    public sealed record Request(string Code);
    
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("urls/{code}", Handler).WithTags("Urls");
        }
    }
    
    public static async Task<IResult> Handler(ApplicationDbContext dbContext, [FromRoute]string code)
    {
        var shortenedUrl = await dbContext.ShortenedUrls
            .FirstOrDefaultAsync(u => u.Code == code);

        if (shortenedUrl == null)
        {
            return TypedResults.NotFound();
        }

        shortenedUrl.RemovedOnUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}