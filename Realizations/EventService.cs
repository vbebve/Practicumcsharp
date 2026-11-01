using Practicum.Interfaces;
using Practicum.Models;

namespace Practicum.Realizations;

public class EventService : IEventService
{
    private readonly List<Event> _events = new();

    private int _nextId = 1;

    public List<Event> GetAll()
    {
        return _events.ToList();
    }

    public Event? GetById(int id)
    {
        return _events.FirstOrDefault(e => e.Id == id);
    }

    public Event Add(Event e)
    {
        e.Id = _nextId++;
        _events.Add(e);

        return e;
    }

    public bool Update(int id, Event e)
    {
        var existingEvent = GetById(id);

        if (existingEvent == null)
            return false;

        existingEvent.Title = e.Title;
        existingEvent.Description = e.Description;
        existingEvent.StartAt = e.StartAt;
        existingEvent.EndAt = e.EndAt;

        return true;
    }

    public bool Delete(int id)
    {
        var existingEvent = GetById(id);

        if (existingEvent == null)
            return false;

        _events.Remove(existingEvent);

        return true;
    }
}