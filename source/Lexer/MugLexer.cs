﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;

namespace Mug.Models.Lexer
{
    public class MugLexer
    {
        public string Source;
        public string ModuleName;
        public List<Token> TokenCollection;
        string CurrentSymbol = "";
        int CurrentIndex = 0;
        int CurrentLine = 0;
        public int Length
        {
            get
            {
                return TokenCollection == null ? TokenCollection.Count : 0;
            }
        }
        public MugLexer(string moduleName, string source)
        {
            ModuleName = moduleName;
            Source = source;
        }
        bool AddKeyword(TokenKind kind, int len)
        {
            TokenCollection.Add(new (CurrentLine, kind, null, new (CurrentIndex-len, CurrentIndex)));
            return true;
        }
        bool GetKeyword(string s) => s switch
        {
            "return" => AddKeyword(TokenKind.KeyReturn, s.Length),
            "if" => AddKeyword(TokenKind.KeyIF, s.Length),
            "elif" => AddKeyword(TokenKind.KeyELIF, s.Length),
            "else" => AddKeyword(TokenKind.KeyELSE, s.Length),
            "func" => AddKeyword(TokenKind.KeyFunc, s.Length),
            "var" => AddKeyword(TokenKind.KeyVar, s.Length),
            "const" => AddKeyword(TokenKind.KeyConst, s.Length),
            "self" => AddKeyword(TokenKind.KeySelf, s.Length),
            "str" => AddKeyword(TokenKind.KeyTstr, s.Length),
            "chr" => AddKeyword(TokenKind.KeyTchr, s.Length),
            "bit" => AddKeyword(TokenKind.KeyTbool, s.Length),
            "i8" => AddKeyword(TokenKind.KeyTi8, s.Length),
            "i32" => AddKeyword(TokenKind.KeyTi32, s.Length),
            "i64" => AddKeyword(TokenKind.KeyTi64, s.Length),
            "u8" => AddKeyword(TokenKind.KeyTu8, s.Length),
            "u32" => AddKeyword(TokenKind.KeyTu32, s.Length),
            "u64" => AddKeyword(TokenKind.KeyTu64, s.Length),
            "unknown" => AddKeyword(TokenKind.KeyTunknown, s.Length),
            _ => false
        };
        TokenKind IllegalChar()
        {
            this.Throw(CurrentIndex, CurrentLine, "Found illegal SpecialSymbol: mug's syntax does not use this character");
            return TokenKind.Bad;
        }
        bool MatchNext(char next) => CurrentIndex + 1 < Source.Length && Source[CurrentIndex+1] == next;
        TokenKind GetSpecial(char c) => c switch
        {
            '(' => TokenKind.OpenPar,
            ')' => TokenKind.ClosePar,
            '[' => TokenKind.OpenBracket,
            ']' => TokenKind.CloseBracket,
            '{' => TokenKind.OpenBrace,
            '}' => TokenKind.CloseBrace,
            '<' => TokenKind.BoolOperatorMinor,
            '>' => TokenKind.BoolOperatorMajor,
            '=' => TokenKind.Equal,
            '!' => TokenKind.Negation,
            '+' => TokenKind.Plus,
            '-' => TokenKind.Minus,
            '*' => TokenKind.Star,
            '/' => TokenKind.Slash,
            ',' => TokenKind.Comma,
            ';' => TokenKind.Semicolon,
            ':' => TokenKind.Colon,
            '.' => TokenKind.Dot,
            '?' => TokenKind.KeyTVoid,
            _ => IllegalChar()
        };

        void AddToken(TokenKind kind, string value)
        {
            if (kind == TokenKind.Identifier)
                CheckValidIdentifier(value);
            if (value is not null)
                TokenCollection.Add(new(CurrentLine, kind, value, new(CurrentIndex - value.ToString().Length, CurrentIndex)));
            else
                TokenCollection.Add(new(CurrentLine, kind, null, new(CurrentIndex, CurrentIndex + 1)));
        }
        void AddSpecial(TokenKind kind)
        {
            InsertCurrentSymbol();
            TokenCollection.Add(new (CurrentLine, kind, null, new(CurrentIndex, CurrentIndex+1)));
        }
        void AddMultiple(TokenKind kind, int count)
        {
            InsertCurrentSymbol();
            TokenCollection.Add(new(CurrentLine, kind, null, new(CurrentIndex, CurrentIndex + count)));
        }
        void InsertCurrentSymbol()
        {
            if (!string.IsNullOrWhiteSpace(CurrentSymbol))
            {
                ProcessSymbol(CurrentSymbol);
                CurrentSymbol = "";
            }
        }
        bool IsDigit(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return long.TryParse(s, out long l);
        }
        bool IsFloatDigit(ref string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            if (s[0] == '.')
                s = '0' + s;
            return double.TryParse(s, out double d);
        }
        bool InsertKeyword(string s)
        {
            return GetKeyword(s);
        }
        void CheckValidIdentifier(string identifier)
        {
            var bad = new Token(CurrentLine, TokenKind.Bad, null, new(CurrentIndex - identifier.Length, CurrentIndex));
            if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
                this.Throw(bad, "Invalid identifier, following the mug's syntax rules, an ident cannot start with `", identifier[0].ToString(), "`;");
            if (identifier.Contains('.'))
                this.Throw(bad, "Invalid identifier, following the mug's syntax rules, an ident cannot contain `.`;");
        }
        bool IsBoolean(string value)
        {
            return value == "true" || value == "false";
        }
        void ProcessSymbol(string value)
        {
            if (IsDigit(value))
                AddToken(TokenKind.ConstantDigit, value);
            else if (IsFloatDigit(ref value))
                AddToken(TokenKind.ConstantFloatDigit, value);
            else if (IsBoolean(value))
                AddToken(TokenKind.ConstantBoolean, value);
            else if (!InsertKeyword(value))
            {
                CheckValidIdentifier(value);
                AddToken(TokenKind.Identifier, value);
            }
        }
        bool NextIsDigit()
        {
            return char.IsDigit(Source[CurrentIndex+1]);
        }
        void ProcessChar(char current)
        {
            if (current == '.' && NextIsDigit())
            {
                CurrentSymbol += '.';
                return;
            }
            if (current == '"')
            {
                CurrentSymbol += Source[CurrentIndex];
                while (CurrentIndex++ < Source.Length && Source[CurrentIndex] != '"')
                    CurrentSymbol += Source[CurrentIndex];
                AddToken(TokenKind.ConstantString, CurrentSymbol+'"');
                CurrentSymbol = "";
                return;
            }
            else if (current == '\'')
            {
                CurrentSymbol += Source[CurrentIndex];
                while (CurrentIndex++ < Source.Length && Source[CurrentIndex] != '\'')
                    CurrentSymbol += Source[CurrentIndex];
                AddToken(TokenKind.ConstantChar, CurrentSymbol+='\'');
                if (CurrentSymbol.Length > 3 || CurrentSymbol.Length < 3)
                    this.Throw(TokenCollection[^1], "Invalid characters in ConstantChar: it can only contain a character, not ", (CurrentSymbol.Length-2).ToString());
                CurrentSymbol = "";
                return;
            }
            else if (current == '\n')
                CurrentLine++;
            if (char.IsControl(current) || char.IsWhiteSpace(current))
                InsertCurrentSymbol();
            else if (char.IsLetterOrDigit(current) || current == '_')
                CurrentSymbol += current;
            else
                switch (current)
                {
                    case '=':
                        if (MatchNext('=')) { AddMultiple(TokenKind.BoolOperatorEQ, 2); CurrentIndex++; break; }
                        goto default;
                    case '!':
                        if (MatchNext('=')) { AddMultiple(TokenKind.BoolOperatorNEQ, 2); CurrentIndex++; break; }
                        goto default;
                    case '+':
                        if (MatchNext('+')) { AddMultiple(TokenKind.Increment, 2); CurrentIndex++; break; }
                        else if (MatchNext('=')) { AddMultiple(TokenKind.IncrementAssign, 2); CurrentIndex++; break; }
                        goto default;
                    case '-':
                        if (MatchNext('-')) { AddMultiple(TokenKind.Decrement, 2); CurrentIndex++; break; }
                        else if (MatchNext('=')) { AddMultiple(TokenKind.DecrementAssign, 2); CurrentIndex++; break; }
                        goto default;
                    case ':':
                        if (MatchNext(':')) { AddMultiple(TokenKind.Block, 2); CurrentIndex++; break; }
                        goto default;
                    case '<':
                        if (MatchNext('=')) { AddMultiple(TokenKind.BoolOperatorMinEQ, 2); CurrentIndex++; break; }
                        goto default;
                    case '>':
                        if (MatchNext('=')) { AddMultiple(TokenKind.BoolOperatorMajEQ, 2); CurrentIndex++; break; }
                        goto default;
                    default:
                        AddSpecial(GetSpecial(current));
                        break;
                }
        }
        void FixCollection()
        {
            // collecting <ident> <::> <ident> in one token
            for (int i = 0; i < TokenCollection.Count; i++)
                if (TokenCollection[i].Kind == TokenKind.Block &&
                    TokenCollection[i - 1].Kind == TokenKind.Identifier &&
                    TokenCollection[i + 1].Kind == TokenKind.Identifier)
                {
                    TokenCollection.RemoveAt(i);
                    var token = TokenCollection[i - 1];
                    TokenCollection[i - 1] = new Token(token.LineAt, token.Kind, token.Value+"::"+ TokenCollection[i].Value, token.Position);
                    TokenCollection.RemoveAt(i);
                }
        }
        public List<Token> Tokenize()
        {
            if (TokenCollection is not null)
                return TokenCollection;
            TokenCollection = new ();
            do
                ProcessChar(Source[CurrentIndex]);
            while (CurrentIndex++ < Source.Length-1);
            AddSpecial(TokenKind.EOF);
            FixCollection();
            return TokenCollection;
        }
    }
}