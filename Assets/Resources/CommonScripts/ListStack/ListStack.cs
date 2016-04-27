using System.Collections.Generic;

/// <summary>
/// List - Stack combination class
/// </summary>
/// <typeparam name="T"> List type</typeparam>
public class ListStack<T>
{
    private List<T> items = new List<T>();

    public int Count
    {
        get
        {
            return items.Count;
        }
    }

    public void Push(T item)
    {
        items.Add(item);
    }
    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }
        else
            return default(T);
    }

    public bool Contains(T item)
    {
        if (items.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(T item)
    {
        items.Remove(item);
    }

    public T GetItem(int itemAtPosition)
    {
        return items[itemAtPosition];
    }

    public void Remove(int itemAtPosition)
    {
        items.RemoveAt(itemAtPosition);
    }
}
