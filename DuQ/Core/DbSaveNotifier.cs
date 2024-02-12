using Microsoft.EntityFrameworkCore;

namespace DuQ.Core;

public class DbSaveNotifier
{
    public event Action? OnDbSave;

    public void NotifyDbSaved(object? sender, SavedChangesEventArgs eventArgs)
    {
        OnDbSave?.Invoke();
    }
}
