using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseApi.Services;

namespace WarehouseApi.Infrastructure;

public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception ex, CancellationToken ct)
    {
        var (status, title) = ex switch
        {
            NotFoundException => (404, ex.Message),
            BusinessRuleException => (400, ex.Message),
            DbUpdateException => (409, "მონაცემი ვერ შეინახა (შესაძლოა ასეთი პროდუქტი უკვე არსებობს)"),
            _ => (0, "")
        };

        if (status == 0) return false; // დანარჩენ შეცდომებს ნაგულისხმევი დამუშავება მიხედავს

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = status, Title = title }, ct);
        return true;
    }
}