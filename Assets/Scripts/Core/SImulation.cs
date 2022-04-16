using System.Collections.Generic;

public class Simulation
{
    private static Dictionary<System.Type, Stack<Event>> eventPool = new Dictionary<System.Type, Stack<Event>>();

    private static PriorityQueue<Event> queue = new PriorityQueue<Event>();

    public static T Create<T>() where T : Event, new()
    {
        Stack<Event> pool;
        if (!eventPool.TryGetValue(typeof(T), out pool))
        {
            pool = new Stack<Event>(4);
            pool.Push(new T());
            eventPool[typeof(T)] = pool;
        }

        if (pool.Count > 0)
        {
            return (T) pool.Pop();
        }
        return new T();
    }



}
