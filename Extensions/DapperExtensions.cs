using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Polly;
using Polly.Retry;
using Dapper;


namespace PollyDapperRetryPolicy.Extensions;

public static class DapperExtensions
{
    private const int RetryCount = 4;
    private static readonly Random Random = new Random();
    private static readonly AsyncRetryPolicy RetryPolicy = Policy
        .Handle<SqlException>()
        .Or<TimeoutException>()
        .OrInner<Win32Exception>()
        .WaitAndRetryAsync(
            RetryCount,
            currentRetryNumber => TimeSpan.FromSeconds(Math.Pow(1.5, currentRetryNumber - 1)) + TimeSpan.FromMilliseconds(Random.Next(0, 100)),
            (currentException, currentSleepDuration, currentRetryNumber, currentContext) =>
            {
                Console.WriteLine(currentException.Message);
                Console.WriteLine(currentSleepDuration.Seconds);
                Console.WriteLine(currentRetryNumber);
                Console.WriteLine(currentContext);
            });

    public static async Task<int> ExecuteAsyncWithRetry(this IDbConnection cnn, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null,
        CommandType? commandType = null) =>
        await RetryPolicy.ExecuteAsync(async () => await cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType));

    public static async Task<IEnumerable<T>> QueryAsyncWithRetry<T>(this IDbConnection cnn, string sql, object? param = null,
        IDbTransaction? transaction = null, int? commandTimeout = null,
        CommandType? commandType = null) =>
        await RetryPolicy.ExecuteAsync(async () => await cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType));
}