namespace SampSharp.Entities;

/// <summary>Represents an identifier of an entity.</summary>
public readonly record struct EntityId
{
    private readonly Guid _id;

    /// <summary>An empty entity identifier.</summary>
    public static readonly EntityId Empty = new();

    /// <summary>Initializes a new instance of the <see cref="EntityId" /> struct.</summary>
    /// <param name="id">The id</param>
    private EntityId(Guid id)
    {
        _id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityId" /> struct.
    /// </summary>
    /// <returns></returns>
    public static EntityId NewEntityId()
    {
        return new EntityId(Guid.NewGuid());
    }

    /// <summary>Gets a value indicating whether this handle is empty.</summary>
    public bool IsEmpty => _id == Guid.Empty;
    
    /// <inheritdoc />
    public override string ToString()
    {
        return IsEmpty
            ? "(Empty)"
            : $"(Id = {_id})";
    }

    /// <summary>Performs an implicit conversion from <see cref="Component" /> to <see cref="EntityId" />. Returns the
    /// entity of the component.</summary>
    /// <param name="component">The component.</param>
    /// <returns>The entity of the component.</returns>
    public static implicit operator EntityId(Component component)
    {
        return component?.Entity ?? default;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="EntityId" /> to <see cref="bool" />.  Returns <see langword="true" /> if the
    /// specified <paramref name="value" /> is not empty.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true" /> if the specified <paramref name="value" /> is not empty; otherwise, <see langword="false" />.</returns>
    public static implicit operator bool(EntityId value)
    {
        return !value.IsEmpty;
    }

    /// <summary>
    /// Implements the operator <see langword="true" />. Returns <see langword="true" /> if the specified <paramref name="value" /> is not
    /// empty.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true" /> if the specified <paramref name="value" /> is not empty; otherwise, <see langword="false" />.</returns>
    public static bool operator true(EntityId value)
    {
        return !value.IsEmpty;
    }

    /// <summary>Implements the operator <see langword="false" />. Returns <see langword="true" /> if the specified <paramref name="value" /> is
    /// empty.</summary>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true" /> if the specified <paramref name="value" /> is empty; otherwise, <see langword="false" />.</returns>
    public static bool operator false(EntityId value)
    {
        return value.IsEmpty;
    }

    /// <summary>Implements the operator <c>!</c>. Returns <see langword="true" /> if the specified <paramref name="value" /> is empty.</summary>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true" /> if the specified <paramref name="value" /> is empty; otherwise, <see langword="false" />.</returns>
    public static bool operator !(EntityId value)
    {
        return value.IsEmpty;
    }
}