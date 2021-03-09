﻿using LLVMSharp.Interop;
using Mug.Models.Generator;
using Mug.Models.Parser.NodeKinds.Statements;
using System;

namespace Mug.MugValueSystem
{
    public struct MugValueType
    {
        private object BaseType { get; set; }
        public MugValueTypeKind TypeKind { get; set; }
        public LLVMTypeRef LLVMType
        {
            get
            {
                if (TypeKind == MugValueTypeKind.Pointer)
                    return LLVMTypeRef.CreatePointer(((MugValueType)BaseType).LLVMType, 0);

                if (TypeKind == MugValueTypeKind.Struct)
                    return ((Tuple<LLVMTypeRef, TypeStatement>)BaseType).Item1;

                return (LLVMTypeRef)BaseType;
            }
        }

        public MugValueType ArrayBaseElementType
        {
            get
            {
                if (TypeKind == MugValueTypeKind.String)
                    return Char;

                throw new("");
            }
        }

        public static MugValueType From(LLVMTypeRef type, MugValueTypeKind kind)
        {
            return new MugValueType() { BaseType = type, TypeKind = kind };
        }

        public static MugValueType Pointer(MugValueType type)
        {
            return new MugValueType() { TypeKind = MugValueTypeKind.Pointer, BaseType = type };
        }

        public static MugValueType Struct(MugValueType[] body, TypeStatement structure)
        {
            return new MugValueType()
            {
                BaseType = new Tuple<LLVMTypeRef, TypeStatement>(LLVMTypeRef.CreateStruct(IRGenerator.MugTypesToLLVMTypes(body), false), structure),
                TypeKind = MugValueTypeKind.Struct
            };
        }

        public static MugValueType Bool => From(LLVMTypeRef.Int1, MugValueTypeKind.Bool);
        public static MugValueType Int8 => From(LLVMTypeRef.Int8, MugValueTypeKind.Int8);
        public static MugValueType Int32 => From(LLVMTypeRef.Int32, MugValueTypeKind.Int32);
        public static MugValueType Int64 => From(LLVMTypeRef.Int64, MugValueTypeKind.Int64);
        public static MugValueType Void => From(LLVMTypeRef.Void, MugValueTypeKind.Void);
        public static MugValueType Char => From(LLVMTypeRef.Int8, MugValueTypeKind.Char);
        public static MugValueType String => From(LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0), MugValueTypeKind.String);

        public override string ToString()
        {
            return TypeKind switch
            {
                MugValueTypeKind.Bool => "u1",
                MugValueTypeKind.Int8 => "u8",
                MugValueTypeKind.Int32 => "i32",
                MugValueTypeKind.Int64 => "i64",
                MugValueTypeKind.Void => "?",
                MugValueTypeKind.Char => "chr",
                MugValueTypeKind.String => "str",
                MugValueTypeKind.Struct => $"{((Tuple<LLVMTypeRef, TypeStatement>)BaseType).Item2.Name}",
                MugValueTypeKind.Pointer => $"ptr {BaseType}".Replace("*", "")
            };
        }

        public bool MatchAnyTypeOfIntType()
        {
            return
                LLVMType == LLVMTypeRef.Int1 ||
                LLVMType == LLVMTypeRef.Int8 ||
                LLVMType == LLVMTypeRef.Int32 ||
                LLVMType == LLVMTypeRef.Int64;
        }

        public bool MatchIntType()
        {
            return
                TypeKind == MugValueTypeKind.Int8 ||
                TypeKind == MugValueTypeKind.Int32 ||
                TypeKind == MugValueTypeKind.Int64;
        }

        public bool IsPointer()
        {
            return TypeKind == MugValueTypeKind.Pointer;
        }

        public TypeStatement GetStructure()
        {
            return ((Tuple<LLVMTypeRef, TypeStatement>)BaseType).Item2;
        }
    }
}