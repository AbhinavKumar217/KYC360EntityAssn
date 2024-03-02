using Serilog;

namespace KYC360Assn.Utils
{
    public class RetryAndBackoff
    {
        public async Task<bool> RetryWriteOperation(Func<Task<bool>> operation, int maxAttempts = 3)
        {
            int attempt = 0;
            TimeSpan delay = TimeSpan.FromSeconds(3);
            bool success = false;

            while (!success && attempt < maxAttempts)
            {
                try
                {
                    success = await operation();
                }
                catch (Exception ex)
                {
                    Log.Error($"Retry attempt {attempt + 1} failed. Error: {ex.Message}");
                    await Task.Delay(delay);
                    delay *= 2;
                }

                attempt++;
            }

            return success;
        }
    }
}
