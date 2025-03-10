using System;
using System.Collections.Generic;
using HotChocolate.Language.Utilities;

namespace HotChocolate.Language;

/// <summary>
/// <para>
/// Represents the GraphQL list type syntax.
/// </para>
/// <para>
/// A GraphQL list is a special collection type which declares the
/// type of each item in the List (referred to as the item type of
/// the list). List values are serialized as ordered lists, where each
/// item in the list is serialized as per the item type.
/// </para>
/// <para>
/// To denote that a field uses a List type the item type is wrapped
/// in square brackets like this: pets: [Pet]. Nesting lists is allowed: matrix: [[Int]].
/// </para>
/// </summary>
public sealed class ListTypeNode
    : INullableTypeNode
    , IEquatable<ListTypeNode>
{
    public ListTypeNode(ITypeNode type)
        : this(null, type)
    {
    }

    public ListTypeNode(Location? location, ITypeNode type)
    {
        Location = location;
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    /// <inheritdoc />
    public SyntaxKind Kind => SyntaxKind.ListType;

    /// <inheritdoc />
    public Location? Location { get; }

    /// <summary>
    /// Gets the element type.
    /// </summary>
    public ITypeNode Type { get; }

    /// <inheritdoc />
    public IEnumerable<ISyntaxNode> GetNodes()
    {
        yield return Type;
    }

    /// <summary>
    /// Creates a new node from the current instance and replaces the
    /// <see cref="Location" /> with <paramref name="location" />.
    /// </summary>
    /// <param name="location">
    /// The location that shall be used to replace the current location.
    /// </param>
    /// <returns>
    /// Returns the new node with the new <paramref name="location" />.
    /// </returns>
    public ListTypeNode WithLocation(Location? location) => new(location, Type);

    /// <summary>
    /// Creates a new node from the current instance and replaces the
    /// <see cref="Type" /> with <paramref name="type" />.
    /// </summary>
    /// <param name="type">
    /// The type that shall be used to replace the current <see cref="Type" />.
    /// </param>
    /// <returns>
    /// Returns the new node with the new <paramref name="type" />.
    /// </returns>
    public ListTypeNode WithType(ITypeNode type) => new(Location, type);

    /// <summary>
    /// Determines whether the specified <see cref="ListTypeNode"/>
    /// is equal to the current <see cref="ListTypeNode"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="ListTypeNode"/> to compare with the current
    /// <see cref="ListTypeNode"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="ListTypeNode"/> is equal
    /// to the current <see cref="ListTypeNode"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(ListTypeNode? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Type.Equals(other.Type);
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to
    /// the current <see cref="ListTypeNode"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="object"/> to compare with the current
    /// <see cref="ListTypeNode"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object"/> is equal to the
    /// current <see cref="ListTypeNode"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return Equals(obj as ListTypeNode);
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="ListTypeNode"/>
    /// object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in
    /// hashing algorithms and data structures such as a hash table.
    /// </returns>
    public override int GetHashCode()
        => HashCode.Combine(Kind, Type);

    /// <summary>
    /// Returns the GraphQL syntax representation of this <see cref="ISyntaxNode"/>.
    /// </summary>
    /// <returns>
    /// Returns the GraphQL syntax representation of this <see cref="ISyntaxNode"/>.
    /// </returns>
    public override string ToString() => SyntaxPrinter.Print(this, true);

    /// <summary>
    /// Returns the GraphQL syntax representation of this <see cref="ISyntaxNode"/>.
    /// </summary>
    /// <param name="indented">
    /// A value that indicates whether the GraphQL output should be formatted,
    /// which includes indenting nested GraphQL tokens, adding
    /// new lines, and adding white space between property names and values.
    /// </param>
    /// <returns>
    /// Returns the GraphQL syntax representation of this <see cref="ISyntaxNode"/>.
    /// </returns>
    public string ToString(bool indented) => SyntaxPrinter.Print(this, indented);

    public static bool operator ==(ListTypeNode? left, ListTypeNode? right)
        => Equals(left, right);

    public static bool operator !=(ListTypeNode? left, ListTypeNode? right)
        => !Equals(left, right);
}
