using Practicum.Models;

namespace Practicum.Interfaces;

public interface IEventService
{
    List<Event> GetAll();

    Event? GetById(int id);

    Event Add(Event e);

    bool Update(int id, Event e);

    bool Delete(int id);
}