using Practicum.Interfaces;
using Practicum.Models;
using Practicum.DTOs;
namespace Practicum.Realizations;

public class EventService : IEventService
{
    private readonly List<Event> _events = new();

    private int _nextId = 1;

    public PaginatedResult<Event> GetAll(string? title, DateTime? startAt, DateTime? endAt, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _events.AsEnumerable();


        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        
        if (startAt.HasValue)
            query = query.Where(e => e.StartAt >= startAt.Value);
        
        if (endAt.HasValue)
            query = query.Where(e => e.EndAt <= endAt.Value);

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedResult<Event>
        {
            TotalCount = totalCount,
            Items = items,
            Page = page,
            PageSize = pageSize

        };
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