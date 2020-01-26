using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Provides especialized instances of ITaskScope <see cref="ITaskScope"/>.
    /// </summary>
    public static class TaskScopeFactory
    {
        public static ITaskScopeMutable Create() => new TaskScope();
    }
}
