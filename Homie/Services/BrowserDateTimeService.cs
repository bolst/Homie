using Microsoft.JSInterop;

namespace Homie.Services;

public interface IBrowserDateTimeService
{
    Task<TimeZoneInfo> GetTimeZoneInfoAsync();
    Task<DateTime> GetDateTimeAsync(DateTime dateTime);
    Task<DateTime> GetNowAsync();
}

public class BrowserDateTimeService(IJSRuntime _jsRuntime) : IBrowserDateTimeService
{
    private TimeZoneInfo? _browserTimeZoneInfo;


    public async Task<TimeZoneInfo> GetTimeZoneInfoAsync()
    {
        if (_browserTimeZoneInfo is null)
        {
            var timeZoneId = await _jsRuntime.InvokeAsync<string>("getBrowserTimeZoneId");
            TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out _browserTimeZoneInfo);
            return _browserTimeZoneInfo ?? TimeZoneInfo.Utc;
        }
        
        return _browserTimeZoneInfo;
    }


    public async Task<DateTime> GetDateTimeAsync(DateTime dateTime)
    {
        var timeZone = await GetTimeZoneInfoAsync();
        return TimeZoneInfo.ConvertTime(dateTime, timeZone);
    }


    public async Task<DateTime> GetNowAsync()
    {
        try
        {
            return await GetDateTimeAsync(DateTime.UtcNow);
        }
        catch (Exception e)
        {
            return DateTime.UtcNow;
        }
    }
}
