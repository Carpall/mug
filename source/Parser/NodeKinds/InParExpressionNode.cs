﻿using Mug.Models.Lexer;
using Mug.Models.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mug.Models.Parser.NodeKinds
{
    public class InParExpressionNode : INode
    {
        public INode Content { get; set; }
        public Range Position { get; set; }
        public string Stringize(string indent = "")
        {
            return indent+$"InParExpressionNode: {{\n{Content.Stringize(indent+"   ")}\n{indent}}}";
        }
    }
}