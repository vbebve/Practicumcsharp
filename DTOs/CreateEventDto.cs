using System.ComponentModel.DataAnnotations;

namespace Practicum.DTOs;

public class CreateEventDto :IValidatableObject
{
    [Required(ErrorMessage = "Название мероприятия обязательно")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartAt == default)
            yield return new ValidationResult(
                "Дата начала обязательна",
                new[] { nameof(StartAt) });

        if (EndAt == default)
            yield return new ValidationResult(
                "Дата окончания обязательна",
                new[] { nameof(EndAt) });

        if (StartAt != default && EndAt != default && EndAt <= StartAt)
            yield return new ValidationResult(
                "Дата окончания должна быть позже даты начала",
                new[] { nameof(EndAt) });
    }
}