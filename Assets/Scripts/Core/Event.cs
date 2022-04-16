using System;

/// <summary>
/// An event encapsulates an action that can be scheduled to happen at some future point in time.
/// It has a precondition method to check whether the event should actually be executed when its time comes, because
/// some conditions may have changed since the event was scheduled.
/// </summary>
/// 
public abstract class Event : IComparable<Event>
{
    private float tick;

    public abstract void Execute();

    public virtual bool Precondition()
    {
        return true;
    }

    public void ExecuteEvent()
    {
        if (Precondition())
        {
            Execute();
        }
    }

    public int CompareTo(Event other)
    {
        return tick.CompareTo(other.tick);
    }
}
