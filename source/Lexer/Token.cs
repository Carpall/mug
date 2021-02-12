﻿using Mug.Models.Parser;
using System;

namespace Mug.Models.Lexer
{
    public struct Token : INode
    {
        public TokenKind Kind { get; }
        public string Value { get; }
        public Range Position { get; set; }

        public Token(TokenKind kind, string value, Range position)
        {
            Kind = kind;
            Value = value;
            Position = position;
        }
        
        public override string ToString()
        {
            return $"{Position} {Kind}: {Value}";
        }
        
        public override bool Equals(object other)
        {
            return other is Token token &&
                   token.Kind.Equals(this.Kind) &&
                   token.Value.Equals(this.Value) &&
                   token.Position.Equals(this.Position);
        }
    }
}
