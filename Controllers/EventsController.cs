using Microsoft.AspNetCore.Mvc;
using Practicum.Interfaces;
using Practicum.Models;
using Practicum.DTOs;

namespace Practicum.Controllers;

[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET /events
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_eventService.GetAll());
    }

    // GET /events/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var eventItem = _eventService.GetById(id);

        if (eventItem == null)
            return NotFound();

        return Ok(eventItem);
    }
        
    // POST /events
    [HttpPost]
    public IActionResult Create([FromBody] CreateEventDto newEventDto)
    {
        var newEvent = new Event
        {
            Title = newEventDto.Title,
            Description = newEventDto.Description,
            StartAt = newEventDto.StartAt,
            EndAt = newEventDto.EndAt
        };

        var createdEvent = _eventService.Add(newEvent);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdEvent.Id },
            createdEvent);
    }

    // PUT /events/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateEventDto updatedEventDto)
    {
        var updatedEvent = new Event
        {
            Title = updatedEventDto.Title,
            Description = updatedEventDto.Description,
            StartAt = updatedEventDto.StartAt,
            EndAt = updatedEventDto.EndAt
        };

        var result = _eventService.Update(id, updatedEvent);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE /events/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _eventService.Delete(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}