// namespace Painter.Inspection;
//
// public class LoggerManager
// {
//     private List<ILogger> _loggers = [];
//
//     private LoggerManager()
//     {
//         _loggers.Add(new FileLogger($"./{DateTime.Now:yyyy-MM-dd hh:mm:ss}.log"));
//     }
//
//     public static LoggerManager Instance = new();
//
//     public void Register(ILogger logger)
//     {
//         _loggers.Add(logger);
//     }
//
//     public void Log(LogType type, string message)
//     {
//         foreach (var logger in _loggers)
//         {
//             logger.Log(type, message);
//         }
//     }
// }