﻿enum TokenKind {
    BuiltInKeywordFunc = 0,
    BuiltInKeywordPub = 1,
    BuiltInKeywordSelf = 2,
    BuiltInKeywordClass = 3,
    BuiltInKeywordDefine = 4,
    BuiltInKeywordExtern = 5,
    BuiltInKeywordUse = 6,
    BuiltInKeywordReturn = 7,
    BuiltInKeywordIf = 8,
    BuiltInKeywordElif = 9,
    BuiltInKeywordElse = 10,
    ConstString,
    ConstChar,
    ConstInt32,
    ConstBool,
    ConstInt64,
    ConstInt16,
    ConstFloat,
    ConstFloat64,
    ConstFloat16,
    ConstIdentifier,
    SymbolCloseBracket,
    SymbolOpenBracket,
    ControlEndOfFile,
    ControlIndent,
    SymbolOpenBrace,
    SymbolCloseBrace,
    SymbolEqual,
    OperatorEqualEqual,
    OperatorPlusEqual,
    SymbolPlus,
    OperatorMinusEqual,
    SymbolMinus,
    OperatorSlashEqual,
    SymbolSlash,
    SymbolStar,
    SymbolStarEqual,
    SymbolDot,
    SymbolColon,
    OperatorSelectStaticMethod,
    SymbolMajor,
    OperatorMajorEqual,
    OperatorMinorEqual,
    SymbolMinor,
    SymbolOpenParenthesis,
    SymbolCloseParenthesis,
    SymbolSemiColon,
    SymbolComma,
    ControlEndOfLine,
    SymbolNegation,
    OperatorNotEqual,
    Const,
}