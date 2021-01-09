﻿using Mug.Models.Lexer;
using Mug.Models.Parser.NodeKinds.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mug.Models.Parser.NodeKinds
{
    public class FieldNode : INode
    {
        public String Name { get; set; }
        public Token Type { get; set; }
        public Modifier Modifier { get; set; }
        public Range Position { get; set; }

        public string Stringize(string indent = "")
        {
            return indent+$"FieldNode: {{\n{indent}   Name: {Name},\n{indent}   Modifier: {Modifier},\n{indent}   Type: {{\n{indent}      {Type.Stringize(indent+"      ")}\n{indent}   }}\n{indent}\n{indent}}}";
        }
    }
}
