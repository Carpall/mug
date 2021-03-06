﻿using LLVMSharp.Interop;
using Mug.Models.Generator;
using Mug.Models.Parser.NodeKinds;
using Mug.Models.Parser.NodeKinds.Statements;
using Mug.TypeSystem;
using System;
using System.Collections.Generic;

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
                    return ((StructureInfo)BaseType).LLVMValue;

                if (TypeKind == MugValueTypeKind.Enum)
                    return (((LLVMTypeRef, EnumStatement))BaseType).Item1;

                if (TypeKind == MugValueTypeKind.Array)
                    return LLVMTypeRef.CreatePointer(((MugValueType)BaseType).LLVMType, 0);

                return (LLVMTypeRef)BaseType;
            }
        }
        public MugValueType PointerBaseElementType
        {
            get
            {
                return (MugValueType)BaseType;
            }
        }

        public MugValueType ArrayBaseElementType
        {
            get
            {
                if (TypeKind == MugValueTypeKind.String)
                    return Char;

                if (TypeKind == MugValueTypeKind.Array)
                    return (MugValueType)BaseType;

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

        public static MugValueType Struct(string name, MugValueType[] body, string[] structure, Range[] positions)
        {
            return new MugValueType()
            {
                BaseType = new StructureInfo()
                {
                    Name = name,
                    FieldNames = structure,
                    FieldPositions = positions,
                    FieldTypes = body,
                    LLVMValue = LLVMTypeRef.CreateStruct(GetLLVMTypes(body), false)
                },
                TypeKind = MugValueTypeKind.Struct
            };
        }

        private static LLVMTypeRef[] GetLLVMTypes(MugValueType[] body)
        {
            var result = new LLVMTypeRef[body.Length];

            for (int i = 0; i < body.Length; i++)
                result[i] = body[i].LLVMType;

            return result;
        }

        public static MugValueType Enum(MugValueType basetype, EnumStatement enumstatement)
        {
            return new MugValueType { TypeKind = MugValueTypeKind.Enum, BaseType = (basetype.LLVMType, enumstatement) };
        }

        public static MugValueType Array(MugValueType basetype)
        {
            return new MugValueType { TypeKind = MugValueTypeKind.Array, BaseType = basetype };
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
                MugValueTypeKind.Struct => ((StructureInfo)BaseType).Name,
                MugValueTypeKind.Pointer => $"*{BaseType}",
                MugValueTypeKind.Enum => (((LLVMTypeRef, EnumStatement))BaseType).Item2.Name,
                MugValueTypeKind.Array => $"[{BaseType}]"
            };
        }

        public bool MatchSameIntType(MugValueType st)
        {
            return MatchIntType() && st.MatchIntType() && TypeKind == st.TypeKind;
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

        public bool IsIndexable()
        {
            return TypeKind == MugValueTypeKind.Array || TypeKind == MugValueTypeKind.String;
        }

        public bool IsPointer()
        {
            return TypeKind == MugValueTypeKind.Pointer;
        }

        public StructureInfo GetStructure()
        {
            return (StructureInfo)BaseType;
        }

        public EnumStatement GetEnum()
        {
            return (((LLVMTypeRef, EnumStatement))BaseType).Item2;
        }

        public bool IsSameEnumOf(MugValueType st)
        {
            return IsEnum() && st.IsEnum() && GetEnum().Equals(st.GetEnum());
        }

        public bool IsEnum()
        {
            return BaseType is (LLVMTypeRef, EnumStatement);
        }
    }
}
