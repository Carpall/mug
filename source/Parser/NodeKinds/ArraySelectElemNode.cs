﻿using System;

namespace Mug.Models.Parser.NodeKinds
{
    public class ArraySelectElemNode : INode
    {
        public string NodeKind => "ArraySelectElemNode";
        public INode Left { get; set; }
        public INode IndexExpression { get; set; }
        public Range Position { get; set; }
    }
}
