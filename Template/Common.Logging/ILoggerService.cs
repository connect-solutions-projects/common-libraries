using $safeprojectname$.Common.Logging;
using $safeprojectname$.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Common.Logging
{
    public interface ILoggerService<T>
    {
        void Log(string message, SeverityLevel level, Exception? exception = null);
        void Log(Result result, string? area = null, SeverityLevel level = SeverityLevel.Error);
        void Log<TData>(Result<TData> result, string? area = null, SeverityLevel level = SeverityLevel.Error);
    }
}
