﻿namespace api.Shared.Extensions;

using api.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalCount, page, pageSize);
    }
}
