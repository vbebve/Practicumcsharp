using Practicum.Models;
using Practicum.Realizations;
using Xunit;

namespace Practicum.Tests.Services;

public class EventServiceTests
{
    private readonly EventService _service = new();


    private static Event CreateTestEvent(
        string title = "Test event",
        DateTime? startAt = null,
        DateTime? endAt = null)
    {
        return new Event
        {
            Title = title,
            Description = "Test description",
            StartAt = startAt ?? new DateTime(2026, 11, 15),
            EndAt = endAt ?? new DateTime(2026, 11, 16)
        };
    }


    // Успешные сценарии


    [Fact]
    public void Add_ValidEvent_ReturnsCreatedEventWithId()
    {
        // Arrange
        var newEvent = CreateTestEvent();

        // Act
        var result = _service.Add(newEvent);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test event", result.Title);
    }

    [Fact]
    public void GetAll_ReturnsAllEvents()
    {
        // Arrange
        _service.Add(CreateTestEvent("Event 1"));
        _service.Add(CreateTestEvent("Event 2"));

        // Act
        var result = _service.GetAll(null, null, null, 1, 10);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsEvent()
    {
        // Arrange
        var newEvent = _service.Add(CreateTestEvent());

        // Act
        var result = _service.GetById(newEvent.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newEvent.Id, result.Id);
        Assert.Equal(newEvent.Title, result.Title);
    }

    [Fact]
    public void Update_ExistingId_UpdatesEvent()
    {
        // Arrange
        var existingEvent = _service.Add(CreateTestEvent());

        var updatedEvent = CreateTestEvent(
            title: "Updated event",
            startAt: new DateTime(2026, 12, 1),
            endAt: new DateTime(2026, 12, 2));

        // Act
        var result = _service.Update(existingEvent.Id, updatedEvent);

        // Assert
        Assert.True(result);

        var actualEvent = _service.GetById(existingEvent.Id);

        Assert.NotNull(actualEvent);
        Assert.Equal("Updated event", actualEvent.Title);
        Assert.Equal(new DateTime(2026, 12, 1), actualEvent.StartAt);
        Assert.Equal(new DateTime(2026, 12, 2), actualEvent.EndAt);
    }

    [Fact]
    public void Delete_ExistingId_RemovesEvent()
    {
        // Arrange
        var newEvent = _service.Add(CreateTestEvent());

        // Act
        var result = _service.Delete(newEvent.Id);

        // Assert
        Assert.True(result);
        Assert.Null(_service.GetById(newEvent.Id));
    }

    // Фильтрация

    [Fact]
    public void GetAll_FilterByTitle_ReturnsMatchingEvents()
    {
        // Arrange
        _service.Add(CreateTestEvent("C# Conference"));
        _service.Add(CreateTestEvent("Java Conference"));
        _service.Add(CreateTestEvent("Python Workshop"));

        // Act
        var result = _service.GetAll("conference", null, null, 1, 10);

        // Assert
        Assert.Equal(2, result.TotalCount);

        Assert.All(result.Items, e =>
            Assert.Contains(
                "Conference",
                e.Title,
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetAll_FilterByStartDate_ReturnsMatchingEvents()
    {
        // Arrange
        _service.Add(CreateTestEvent(
            "Event 1",
            new DateTime(2026, 11, 10),
            new DateTime(2026, 11, 11)));

        _service.Add(CreateTestEvent(
            "Event 2",
            new DateTime(2026, 11, 20),
            new DateTime(2026, 11, 21)));

        // Act
        var result = _service.GetAll(
            null,
            new DateTime(2026, 11, 15),
            null,
            1,
            10);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Event 2", result.Items[0].Title);
    }

    [Fact]
    public void GetAll_FilterByEndDate_ReturnsMatchingEvents()
    {
        // Arrange
        _service.Add(CreateTestEvent(
            "Event 1",
            new DateTime(2026, 11, 10),
            new DateTime(2026, 11, 12)));

        _service.Add(CreateTestEvent(
            "Event 2",
            new DateTime(2026, 11, 20),
            new DateTime(2026, 11, 22)));

        // Act
        var result = _service.GetAll(
            null,
            null,
            new DateTime(2026, 11, 15),
            1,
            10);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Event 1", result.Items[0].Title);
    }

    [Fact]
    public void GetAll_CombinedFilters_ReturnsMatchingEvents()
    {
        // Arrange
        _service.Add(CreateTestEvent(
            "C# Conference",
            new DateTime(2026, 11, 10),
            new DateTime(2026, 11, 12)));

        _service.Add(CreateTestEvent(
            "C# Conference",
            new DateTime(2026, 11, 20),
            new DateTime(2026, 11, 22)));

        _service.Add(CreateTestEvent(
            "Java Conference",
            new DateTime(2026, 11, 10),
            new DateTime(2026, 11, 12)));

        // Act
        var result = _service.GetAll(
            "c#",
            new DateTime(2026, 11, 1),
            new DateTime(2026, 11, 15),
            1,
            10);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("C# Conference", result.Items[0].Title);
    }

    // Пагинация

    [Fact]
    public void GetAll_Pagination_ReturnsCorrectPage()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
        {
            _service.Add(CreateTestEvent($"Event {i}"));
        }

        // Act
        var result = _service.GetAll(
            null, null, null,
            page: 2,
            pageSize: 2);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(2, result.Items.Count);

        Assert.Equal("Event 3", result.Items[0].Title);
        Assert.Equal("Event 4", result.Items[1].Title);
    }

    [Fact]
    public void GetAll_Pagination_LastPage_ReturnsRemainingEvents()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
        {
            _service.Add(CreateTestEvent($"Event {i}"));
        }

        // Act
        var result = _service.GetAll(
            null, null, null,
            page: 3,
            pageSize: 2);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Single(result.Items);
        Assert.Equal("Event 5", result.Items[0].Title);
    }

    // Неуспешные сценарии

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        // Act
        var result = _service.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_NonExistingId_ReturnsFalse()
    {
        // Arrange
        var updatedEvent = CreateTestEvent("Updated event");

        // Act
        var result = _service.Update(999, updatedEvent);

        // Assert
        Assert.False(result);
    }

}
