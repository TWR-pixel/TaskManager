using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskManager.Infrastructure.Sqlite.Common.Interceptors;

public class InterceptorTest : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                     int result,
                                                     CancellationToken cancellationToken = default)
    {
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
