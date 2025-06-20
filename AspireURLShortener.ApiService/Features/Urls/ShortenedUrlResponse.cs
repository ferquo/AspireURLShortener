namespace AspireURLShortener.ApiService.Features.Urls;

public sealed record ShortenedUrlResponse(Guid Id, string OriginalUrl, string ShortenedUrl, string Code, DateTime CreatedAt);