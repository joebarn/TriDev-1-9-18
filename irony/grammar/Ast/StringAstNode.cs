using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using grammar.Ast.Irony;

namespace grammar.Ast
{
    public class StringAstNode : AstNode
    {
        protected object _value;

        protected override void OnInit(ParseTreeNode parseTreeNode)
        {
            _value = parseTreeNode.Token.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {_value} [{_value.GetType().Name}]";
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }
    }
}
