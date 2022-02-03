namespace ABDDB.Replication.Utils
{
    public class Combinators
    {
        public static Task<IList<T>> WhenSome<T>(Task<T>[] tasks, int count, CancellationToken token = default)
        {
            if (tasks is null)
                throw new ArgumentNullException(nameof(tasks));

            var tcs = new TaskCompletionSource<IList<T>>(tasks);
            int numTask = tasks.Length;
            TaskFactory factory = new TaskFactory(token);
            var result = new List<T>(count);
            for (int i = 0; i < numTask; i++)
            {
                var task = tasks[i];
                task.ContinueWith(completed =>
                {
                    lock (result)
                    {
                        result.Add(task.Result);
                        if (result.Count == count)
                            tcs.TrySetResult(result);
                    }
                }, token);
            }
            return tcs.Task;
        }

        public static Task WhenSome(Task[] tasks, int count, CancellationToken token = default)
        {
            if (tasks is null)
                throw new ArgumentNullException(nameof(tasks));

            var tcs = new TaskCompletionSource(tasks);
            int numTask = tasks.Length;
            int completedCount = 0;
            for (int i = 0; i < numTask; i++)
            {
                var task = tasks[i];
                task.ContinueWith(completed =>
                {
                    lock (completed)
                    {
                        completedCount++;
                        if (completedCount == count)
                            tcs.TrySetResult();
                    }
                }, token);
            }
            return tcs.Task;
        }
    }
}