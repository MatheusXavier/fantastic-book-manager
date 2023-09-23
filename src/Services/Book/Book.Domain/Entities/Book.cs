using Ardalis.GuardClauses;

namespace Book.Domain.Entities;

public class Book : BaseEntity<Guid>
{
    public string Title { get; private set; }

    public string Author { get; private set; }

    public string Genre { get; private set; }

    public Guid UserId { get; private set; }

    public Book(Guid id, Guid userId, string title, string author, string genre)
    {
        Guard.Against.Default(id, nameof(id));
        Guard.Against.Default(userId, nameof(userId));
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(author, nameof(author));
        Guard.Against.NullOrWhiteSpace(genre, nameof(genre));

        Id = id;
        UserId = userId;
        Title = title;
        Author = author;
        Genre = genre;
    }

    public void Update(string title, string author, string genre)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(author, nameof(author));
        Guard.Against.NullOrWhiteSpace(genre, nameof(genre));

        Title = title;
        Author = author;
        Genre = genre;
    }
}
