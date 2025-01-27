using System.Collections.Concurrent;

//CustomItem
public class CustomItem<T>  where T : IDisposable

{
    public T item { get; set; }
    public DateTime CratedAt { get; set; }

    public CustomItem(T item, DateTime cratedAt)
    {
        this.item = item;
        CratedAt = cratedAt;
    }


}
//CustomItemPool
public class CustomItemPool<T>   where T : CustomItem<T>,IDisposable
{
    private ConcurrentBag<CustomItem<T>>? _pool;
    private readonly int _objectLifeTime;
    private readonly Timer _cleanTimerOfObject;
    private bool _disposed;
    public CustomItemPool(int objectLifeTime)
    {
        _objectLifeTime = objectLifeTime;
        _cleanTimerOfObject = new Timer(CleanUp, null, TimeSpan.Zero, TimeSpan.FromSeconds(_objectLifeTime / 10));
    }


    public bool IsExpired(DateTime CreatedAt)
    {
        return (DateTime.Now - CreatedAt).TotalSeconds > _objectLifeTime;
    }

    //CallBack Method for Timer
    private void CleanUp(object? state)
    {
        foreach (var item in _pool.ToArray())
        {
            if (IsExpired(item.CratedAt))
            {
                _pool.TryTake(out var expiredObject);
                expiredObject.item.Dispose();
            }
        }
    }

    //Adding CustomItem into CustomItemPool

    public void ReturnObject(CustomItem<T> item)
    {
        if(_disposed)
            throw new ObjectDisposedException(nameof(CustomItemPool<T>));

        if(item == null)
            throw new ArgumentNullException(nameof(item));

        _pool.Add(item);
    }
    public virtual void Dispose()
    {
        if (_disposed)
            return;

        _cleanTimerOfObject.Dispose();

        while (_pool.TryTake(out var pooledObject))
        {
            pooledObject.item.Dispose();
        }
        _disposed = true;
    }
    public T GetObject()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(GetObject));

        while (_pool.TryTake(out var pooledItem))
        {
            if (IsExpired(pooledItem.CratedAt))
            {
                pooledItem.item.Dispose();
                continue;
            }
            return pooledItem.item;
        }
        return null;
    }

   

}



