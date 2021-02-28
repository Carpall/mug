﻿using Mug.TypeSystem;
using System;

namespace Mug.Models.Parser.NodeKinds.Statements
{
    public struct ConstantStatement : INode
    {
        public string NodeKind => "Const";
        public String Name { get; set; }
        public MugType Type { get; set; }
        public INode Body { get; set; }
        public Range Position { get; set; }
    }
}
