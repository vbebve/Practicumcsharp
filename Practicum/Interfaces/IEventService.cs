using Practicum.DTOs;
using Practicum.Models;

namespace Practicum.Interfaces;

public interface IEventService
{
    PaginatedResult<Event> GetAll(string? title, DateTime? startAt, DateTime? endAt, int page, int pageSize);

    Event? GetById(int id);

    Event Add(Event e);

    bool Update(int id, Event e);

    bool Delete(int id);
}