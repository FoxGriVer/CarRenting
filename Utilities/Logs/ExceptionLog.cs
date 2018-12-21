
/// <summary>
/// Метод для записи в отдельный файл ошибок
/// </summary>
namespace Utilities.Logs
{
    public class ExceptionLog : LogTools
    {
        public ExceptionLog() : base("exceptionlog")
        {
            getLog();
        }
    }
}
