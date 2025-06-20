using AspireURLShortener.ApiService.Data;
using AspireURLShortener.ApiService.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspireURLShortener.ApiService.Features.Urls;

public sealed class GetPaginatedShortenedUrls
{
    public sealed record Request(int Page, int PageSize);
    public sealed record Response(List<ShortenedUrlResponse> ShortenedUrls, int Page, int TotalCount);
    
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("urls", Handler).WithTags("Urls");
        }
    }
    
    public static async Task<IResult> Handler(ApplicationDbContext dbContext, [FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var totalCount = await dbContext.ShortenedUrls.CountAsync();
        var shortenedUrls = await dbContext.ShortenedUrls
            .OrderByDescending(u => u.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new ShortenedUrlResponse(u.Id, u.LongUrl, u.ShortUrl, u.Code, u.CreatedOnUtc))
            .ToListAsync();

        var response = new Response(shortenedUrls, page, totalCount);
        
        return TypedResults.Ok(response);
    }
}