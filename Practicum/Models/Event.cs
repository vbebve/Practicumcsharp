using System.ComponentModel.DataAnnotations;

namespace Practicum.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

}