using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SPTarkov.DI.Annotations;

namespace SPTarkov.Server.Core.Utils.Logger.Handlers;

[Injectable(InjectionType.Singleton)]
public class FileLogHandler : BaseLogHandler
{
    private static ConcurrentDictionary<string, Lock> _fileLocks = new();
    private static Dictionary<string, DateTime> _checkpointCache = new();

    public override LoggerType LoggerType => LoggerType.File;

    public override void Log(SptLogMessage message, BaseSptLoggerReference reference)
    {
        var config = reference as FileSptLoggerReference;

        if (!_fileLocks.TryGetValue(config.FilePath, out var lockObject))
        {
            lockObject = new Lock();
            while (!_fileLocks.TryAdd(config.FilePath, lockObject));
        }

        lock (lockObject)
        {
            var logDirectory = Path.GetDirectoryName(config.FilePath);
            if (!Directory.Exists(Path.GetDirectoryName(config.FilePath)))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string currentCheckpointFilePath = config.FilePath;

            if (config.RollingInterval != RollingInterval.None)
            {
                if (_checkpointCache.TryGetValue(config.FilePath, out var nextCheckpoint))
                {
                    if (nextCheckpoint < config.RollingInterval.GetCurrentCheckpoint())
                    {
                        _checkpointCache[config.FilePath] = config.RollingInterval.GetNextCheckpoint().Date;
                        currentCheckpointFilePath = GetCurrentCheckpointFile(config, false, true);
                    }
                    else
                    {
                        currentCheckpointFilePath = GetCurrentCheckpointFile(config);
                    }
                }
                else
                {
                    _checkpointCache.Add(config.FilePath, config.RollingInterval.GetNextCheckpoint().Date);
                    currentCheckpointFilePath = GetCurrentCheckpointFile(config, true);
                }
            }

            // The AppendAllText will create the file as long as the directory exists
            File.AppendAllText(currentCheckpointFilePath, FormatMessage(message.Message + "\n", message, reference));
        }
    }

    private string GetCurrentCheckpointFile(FileSptLoggerReference config, bool startup = false, bool roll = false)
    {
        var logDirectory = Path.GetDirectoryName(config.FilePath);
        var regex = new Regex($"{Path.GetFileNameWithoutExtension(config.FilePath)}_(\\d+)-(\\d+)-(\\d+)-(\\d+)-\\d+-\\d+(_\\d+)?\\.txt");
        var allCheckpointFiles = Directory.GetFiles(logDirectory);
        var checkpoints = new List<FileCheckpoint>();
        foreach (var checkpointFile in allCheckpointFiles)
        {
            var match = regex.Match(Path.GetFileName(checkpointFile));
            if (match.Success)
            {
                var date = new DateTime(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value),
                    0, 0);
                var sequence = match.Groups[5].Success ? int.Parse(match.Groups[5].Value.TrimStart('_')) : 0;
                checkpoints.Add(new FileCheckpoint { Date = date, Sequence = sequence });
            }
        }

        FileCheckpoint latestCheckpoint = checkpoints
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.Sequence)
            .FirstOrDefault();

        DateTime currentCheckpoint = roll ? config.RollingInterval.GetNextCheckpoint() : config.RollingInterval.GetCurrentCheckpoint();

        if (latestCheckpoint == null || latestCheckpoint.Date != currentCheckpoint.Date)
        {
            return Path.Combine(logDirectory, $"{Path.GetFileNameWithoutExtension(config.FilePath)}_{currentCheckpoint.Date:yyyy-MM-dd-HH-mm-ss}.txt");
        }
        else
        {
            int sequence = startup ? latestCheckpoint.Sequence + 1 : latestCheckpoint.Sequence;
            return Path.Combine(logDirectory, $"{Path.GetFileNameWithoutExtension(config.FilePath)}_{currentCheckpoint.Date:yyyy-MM-dd-HH-mm-ss}{(sequence == 0 ? string.Empty : $"_{sequence}")}.txt");
        }
    }
}
