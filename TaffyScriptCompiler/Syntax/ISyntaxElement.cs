﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.FrontEnd;

namespace TaffyScript.Syntax
{
    public interface ISyntaxElement
    {
        //Todo: Element should know file pos.
        SyntaxNode Parent { get; set; }
        SyntaxType Type { get; }
        TokenPosition Position { get; }
        bool IsToken { get; }
        void Accept(ISyntaxElementVisitor visitor);
    }
}