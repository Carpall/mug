﻿using System.ComponentModel;

namespace Mug.Models.Lexer
{
    public enum TokenKind
    {
        [Description("bad")]
        Bad,
        [Description("ident")]
        Identifier,
        [Description("const string")]
        ConstantString,
        [Description("const num")]
        ConstantDigit,
        [Description("eof")]
        EOF,
        [Description("const char")]
        ConstantChar,
        [Description("func")]
        KeyFunc,
        [Description("var")]
        KeyVar,
        [Description("const")]
        KeyConst,
        [Description("(")]
        OpenPar,
        [Description(")")]
        ClosePar,
        [Description(";")]
        Semicolon,
        [Description("?")]
        KeyTVoid,
        [Description(":")]
        Colon,
        [Description("{")]
        OpenBrace,
        [Description("}")]
        CloseBrace,
        [Description("[")]
        OpenBracket,
        [Description("]")]
        CloseBracket,
        [Description("const float num")]
        ConstantFloatDigit,
        [Description(".")]
        Dot,
        [Description(",")]
        Comma,
        [Description("==")]
        BooleanEQ,
        [Description("=")]
        Equal,
        [Description("/")]
        Slash,
        [Description("!=")]
        BooleanNEQ,
        [Description("i32")]
        KeyTi32,
        [Description("i64")]
        KeyTi64,
        [Description("unknown")]
        KeyTunknown,
        [Description("chr")]
        KeyTchr,
        [Description("str")]
        KeyTstr,
        [Description("u1")]
        KeyTbool,
        [Description("u8")]
        KeyTu8,
        [Description("u32")]
        KeyTu32,
        [Description("u64")]
        KeyTu64,
        [Description("+=")]
        AddAssignment,
        [Description("-=")]
        SubAssignment,
        [Description("+")]
        Plus,
        [Description("-")]
        Minus,
        [Description("*")]
        Star,
        [Description("return")]
        KeyReturn,
        [Description("const bool")]
        ConstantBoolean,
        [Description("if")]
        KeyIf,
        [Description("elif")]
        KeyElif,
        [Description("else")]
        KeyElse,
        [Description("<")]
        BooleanMinor,
        [Description(">")]
        BooleanMajor,
        [Description("<=")]
        BooleanMinEQ,
        [Description(">=")]
        BooleanMajEQ,
        [Description("!")]
        Negation,
        [Description("while")]
        KeyWhile,
        [Description("to")]
        KeyTo,
        [Description("in")]
        KeyIn,
        [Description("for")]
        KeyFor,
        [Description("..")]
        RangeDots,
        [Description("as")]
        KeyAs,
        [Description("continue")]
        KeyContinue,
        [Description("break")]
        KeyBreak,
        [Description("type")]
        KeyType,
        [Description("pub")]
        KeyPub,
        [Description("new")]
        KeyNew,
        [Description("use")]
        KeyUse,
        [Description("import")]
        KeyImport,
        [Description("*=")]
        MulAssignment,
        [Description("/=")]
        DivAssignment,
        [Description("++")]
        OperatorIncrement,
        [Description("--")]
        OperatorDecrement,
        [Description("|")]
        BooleanOR,
        [Description("&")]
        BooleanAND,
        [Description("ptr")]
        KeyTPtr,
        [Description("@[")]
        OpenPragmas,
        [Description("priv")]
        KeyPriv,
        [Description("enum")]
        KeyEnum
    }
}