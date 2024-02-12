using Microsoft.EntityFrameworkCore;

namespace DuQ.Core;

public class DbSaveNotifier
{
    public event Func<Task>? OnDbSave;

    public void NotifyDbSaved(object? sender, SavedChangesEventArgs eventArgs)
    {
        OnDbSave?.Invoke();
    }
}
