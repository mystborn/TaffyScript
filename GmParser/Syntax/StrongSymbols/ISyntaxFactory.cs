﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
{
    public interface ISyntaxElementFactory
    {
        ISyntaxNode CreateNode(SyntaxType type);
        ISyntaxNode CreateNode(SyntaxType type, string value);
        ISynt
    }
}
