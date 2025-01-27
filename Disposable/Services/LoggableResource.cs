using System.Text.RegularExpressions;

namespace Disposable.Services
{
    public class LoggableResource : IDisposable
    {
        private List<IDisposable> _resources = new List<IDisposable>();
        private List<Exception> exceptions = new List<Exception>();

        private readonly string? _logsFilePath;
        private readonly string? _logExceptionPath;
        private bool _disposed;

        // Constructor to initialize file paths for logging resource disposal and exceptions
        public LoggableResource(string? logsFilePath, string? logExceptionPath)
        {
            _logsFilePath = logsFilePath;
            _logExceptionPath = logExceptionPath;
        }

        // Method to add resources to be managed and disposed of
        public void AddResource(IDisposable resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource), "Resource cannot be null");

            _resources.Add(resource);
        }

        // Dispose method to release resources and log the operations
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            // Loop through all resources and dispose them
            foreach (var resource in _resources)
            {
                try
                {
                    resource.Dispose();
                    LogDisposeResource(resource);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    LogExceptions(ex);
                }
            }
        }

        // Logging the disposal of a resource with the current time
        //ResourceName: Freed at <DateTime>. logging Format
        private void LogDisposeResource(IDisposable disposedResource)
        {
            if (disposedResource == null)
                throw new ArgumentNullException(nameof(disposedResource), "disposedResource cannot be null");

            if (string.IsNullOrEmpty(_logsFilePath))
                throw new InvalidOperationException("Logs file path cannot be null");

            string logFormat = $"{disposedResource.GetType().Name}: Freed at {DateTime.UtcNow}";
            // Writing the log entry to the specified file in append mode
            using (var writer = new StreamWriter(_logsFilePath, append: true))
            {
                writer.WriteLine(logFormat);
            }
        }

        // Logging any exceptions that occur during resource disposal
        private void LogExceptions(Exception ex)
        {
            if (string.IsNullOrEmpty(_logExceptionPath))
                throw new InvalidOperationException("Exception log file path cannot be null");

            string exceptionLogFormat = $"Exception of type {ex.GetType()} occurred while disposing resource: {ex.Message}";
            // Writing the exception log entry to the specified file in append mode
            using (var writer = new StreamWriter(_logExceptionPath, append: true))
            {
                writer.WriteLine(exceptionLogFormat);
            }
        }

        // Analyzing the logs by reading the log files and displaying the results
        public void AnalyseLogs()
        {
            if (string.IsNullOrEmpty(_logsFilePath) || string.IsNullOrEmpty(_logExceptionPath))
            {
                Console.WriteLine("Log file paths are not specified.");
                return;
            }

            // Reading the logs of disposed resources
            using (var reader = new StreamReader(_logsFilePath))
            {
                string lines = reader.ReadToEnd();
                string[] LogLines = lines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine($"There are {LogLines.Length} disposed resources.");

                List<string> resourcesName = new List<string>();
                foreach (var logLine in LogLines)
                {
                    var match = Regex.Match(logLine, @"^(.*?):");
                    if (match.Success)
                    {
                        resourcesName.Add(match.Groups[1].Value);
                    }
                }

                var newResourceNameAndCouunt = from resource in resourcesName
                                               group resource by resource into resourceGroup
                                               select new { resourcename = resourceGroup.Key, Count = resourceGroup.Count() };

                foreach (var NameAndCount in newResourceNameAndCouunt)
                {
                    Console.WriteLine($"NameOfResource is {NameAndCount.resourcename} it is disposed {NameAndCount.Count} times");
                }


            }

            // Reading the logs of exceptions encountered during resource disposal
            using (var reader = new StreamReader(_logExceptionPath))
            {
                string lines = reader.ReadToEnd();
                string[] stringLines = lines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"There are {stringLines.Length} exceptions encountered while disposing resources.");

            }
        }
    }
}
