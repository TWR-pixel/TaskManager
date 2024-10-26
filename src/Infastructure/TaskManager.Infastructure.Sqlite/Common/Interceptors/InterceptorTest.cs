using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TaskManager.Infastructure.Sqlite.Common.Interceptors;

public class InterceptorTest : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                     int result,
                                                     CancellationToken cancellationToken = default)
    {
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
