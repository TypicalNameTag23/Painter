// using System.Text;
//
// namespace Painter.Inspection;
//
// // Simple thing for debugging, not good at all
// public class FileLogger(string filePath) : ILogger
// {
//     public void Log(LogType type, string message)
//     {
//         var content = $"({DateTime.Now:yyyy-MM-dd hh:mm:ss}) [{type.ToString()}]: {message}\n";
//         File.AppendAllTextAsync(filePath, content);
//     }
// }