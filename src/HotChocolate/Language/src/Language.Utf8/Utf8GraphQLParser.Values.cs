using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static HotChocolate.Language.Properties.LangUtf8Resources;
using static HotChocolate.Language.TokenPrinter;

namespace HotChocolate.Language;

// Implements the parsing rules in the Values section.
public ref partial struct Utf8GraphQLParser
{
    /// <summary>
    /// Parses a value.
    /// <see cref="IValueNode" />:
    /// - Variable [only if isConstant is <c>false</c>]
    /// - IntValue
    /// - FloatValue
    /// - StringValue
    /// - BooleanValue
    /// - NullValue
    /// - EnumValue
    /// - ListValue[isConstant]
    /// - ObjectValue[isConstant]
    /// <see cref="BooleanValueNode" />: true or false.
    /// <see cref="NullValueNode" />: null
    /// <see cref="EnumValueNode" />: Name but not true, false or null.
    /// </summary>
    /// <param name="isConstant">
    /// Defines if only constant values are allowed;
    /// otherwise, variables are allowed.
    /// </param>
    private IValueNode ParseValueLiteral(bool isConstant)
    {
        if (_reader.Kind == TokenKind.LeftBracket)
        {
            return ParseList(isConstant);
        }

        if (_reader.Kind == TokenKind.LeftBrace)
        {
            return ParseObject(isConstant);
        }

        if (TokenHelper.IsScalarValue(in _reader))
        {
            return ParseScalarValue();
        }

        if (_reader.Kind == TokenKind.Name)
        {
            return ParseEnumValue();
        }

        if (_reader.Kind == TokenKind.Dollar && !isConstant)
        {
            return ParseVariable();
        }

        throw Unexpected(_reader.Kind);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private StringValueNode ParseStringLiteral()
    {
        TokenInfo start = Start();

        var isBlock = _reader.Kind == TokenKind.BlockString;
        var value = ExpectString();
        Location? location = CreateLocation(in start);

        return new StringValueNode(location, value, isBlock);
    }

    /// <summary>
    /// Parses a list value.
    /// <see cref="ListValueNode" />:
    /// - [ ]
    /// - [ Value[isConstant]+ ]
    /// </summary>
    /// <param name="isConstant">
    /// Defines if only constant values are allowed;
    /// otherwise, variables are allowed.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ListValueNode ParseList(bool isConstant)
    {
        TokenInfo start = Start();

        if (_reader.Kind != TokenKind.LeftBracket)
        {
            throw new SyntaxException(
                _reader,
                ParseMany_InvalidOpenToken,
                TokenKind.LeftBracket,
                Print(in _reader));
        }

        var items = new List<IValueNode>();

        // skip opening token
        MoveNext();

        while (_reader.Kind != TokenKind.RightBracket)
        {
            items.Add(ParseValueLiteral(isConstant));
        }

        // skip closing token
        Expect(TokenKind.RightBracket);

        Location? location = CreateLocation(in start);

        return new ListValueNode
        (
            location,
            items
        );
    }

    /// <summary>
    /// Parses an object value.
    /// <see cref="ObjectValueNode" />:
    /// - { }
    /// - { Value[isConstant]+ }
    /// </summary>
    /// <param name="isConstant">
    /// Defines if only constant values are allowed;
    /// otherwise, variables are allowed.
    /// </param>
    private ObjectValueNode ParseObject(bool isConstant)
    {
        TokenInfo start = Start();

        if (_reader.Kind != TokenKind.LeftBrace)
        {
            throw new SyntaxException(
                _reader,
                ParseMany_InvalidOpenToken,
                TokenKind.LeftBrace,
                Print(in _reader));
        }

        var fields = new List<ObjectFieldNode>();

        // skip opening token
        MoveNext();

        while (_reader.Kind != TokenKind.RightBrace)
        {
            TokenInfo fieldStart = Start();
            NameNode name = ParseName();
            ExpectColon();
            IValueNode value = ParseValueLiteral(isConstant);
            Location? fieldLocation = CreateLocation(in fieldStart);

            fields.Add(new ObjectFieldNode(fieldLocation, name, value));
        }

        // skip closing token
        Expect(TokenKind.RightBrace);

        Location? location = CreateLocation(in start);

        return new ObjectValueNode
        (
            location,
            fields
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IValueNode ParseScalarValue()
    {
        if (TokenHelper.IsString(in _reader))
        {
            return ParseStringLiteral();
        }

        TokenInfo start = Start();
        TokenKind kind = _reader.Kind;

        if (!TokenHelper.IsScalarValue(in _reader))
        {
            throw new SyntaxException(_reader, Parser_InvalidScalarToken, _reader.Kind);
        }

        ReadOnlyMemory<byte> value = _reader.Value.ToArray();
        FloatFormat? format = _reader.FloatFormat;
        MoveNext();

        Location? location = CreateLocation(in start);

        if (kind == TokenKind.Float)
        {
            return new FloatValueNode
            (
                location,
                value,
                format ?? FloatFormat.FixedPoint
            );
        }

        if (kind == TokenKind.Integer)
        {
            return new IntValueNode
            (
                location,
                value
            );
        }

        throw Unexpected(kind);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IValueNode ParseEnumValue()
    {
        TokenInfo start = Start();

        Location? location;

        if (_reader.Value.SequenceEqual(GraphQLKeywords.True))
        {
            MoveNext();
            location = CreateLocation(in start);
            return new BooleanValueNode(location, true);
        }

        if (_reader.Value.SequenceEqual(GraphQLKeywords.False))
        {
            MoveNext();
            location = CreateLocation(in start);
            return new BooleanValueNode(location, false);
        }

        if (_reader.Value.SequenceEqual(GraphQLKeywords.Null))
        {
            MoveNext();
            if (_createLocation)
            {
                location = CreateLocation(in start);
                return new NullValueNode(location);
            }
            return NullValueNode.Default;
        }

        var value = _reader.GetString();
        MoveNext();
        location = CreateLocation(in start);

        return new EnumValueNode
        (
            location,
            value
        );
    }
}
