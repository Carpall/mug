﻿using Mug.Models.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mug.Models.Parser.NodeKinds
{
    public class MemberNode : INode
    {
        public INode Base { get; set; }
        public INode Member { get; set; }
        public Range Position { get; set; }
    }
}
