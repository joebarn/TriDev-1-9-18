using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;
using grammar.Ast;
using grammar.Ast.Irony;

namespace grammar
{
    /*

        filters
        [

            filter
            [
                from="blah blah";
                subject="blah";
            ]

            filter
            [
                from="blah blah";
                subject="blah";
            ]


        ]


    */

    [Language("Demo", "1.0", "A Demo Language")]
    public class DemoGrammar : Grammar
    {

        public DemoGrammar() : base(false) // true means case sensitive
        {
            GrammarComments = @"A Demo Language. Case-insensitive.";

            //comment
            CommentTerminal eolComment = new CommentTerminal("eolComment", "//", "\n", "\r");
            NonGrammarTerminals.Add(eolComment);

            //number
            NumberLiteral integer_terminal = new NumberLiteral("integer", NumberOptions.IntOnly | NumberOptions.AllowSign, typeof(IntegerAstNode));
            integer_terminal.DefaultIntTypes = new TypeCode[] { TypeCode.Int32 };
            integer_terminal.AddPrefix("0b", NumberOptions.Binary);
            integer_terminal.AddPrefix("0x", NumberOptions.Hex);

            //label
            IdentifierTerminal label_terminal = new IdentifierTerminal("label_terminal");

            //string
            StringLiteral string_terminal = new StringLiteral("string", "\"",StringOptions.None,typeof(StringAstNode));

            //keywords
            KeyTerm FILTERS = ToTerm("filters");
            KeyTerm FILTER = ToTerm("filter");
            KeyTerm OPEN = ToTerm("[");
            KeyTerm CLOSE = ToTerm("]");

            //name
            NonTerminal name = new NonTerminal("name", typeof(NameAstNode));
            name.Rule = label_terminal;

            //value
            NonTerminal value = new NonTerminal("value");
            value.Rule = integer_terminal | string_terminal;

            //nvPair
            NonTerminal nvPair = new NonTerminal("nvPair", typeof(NvPairAstNode));
            nvPair.Rule = name + "=" + value + ";";  //note: you don't have to excplitly declare KeyTerms

            //nvPairs
            NonTerminal nvPairs = new NonTerminal("nvPairs");
            nvPairs.Rule = MakeStarRule(nvPairs, nvPair);

            //filter
            NonTerminal filter = new NonTerminal("filter", typeof(FilterAstNode));
            filter.Rule = FILTER + OPEN + nvPairs + CLOSE;

            //filters
            NonTerminal filters = new NonTerminal("filters");
            filters.Rule = MakeStarRule(filters, filter);

            //file
            NonTerminal file = new NonTerminal("file", typeof(FileAstNode));
            file.Rule = FILTERS + OPEN + filters + CLOSE;

            Root = file;

            LanguageFlags |= LanguageFlags.CreateAst;

        }

        public override void BuildAst(LanguageData language, ParseTree parseTree)
        {
            if (LanguageFlags.IsSet(LanguageFlags.CreateAst))
            {
                var astContext = new AstContext(language);
                astContext.DefaultNodeType = typeof(DefaultAstNode);
                astContext.DefaultLiteralNodeType = typeof(DefaultAstNode);
                astContext.DefaultIdentifierNodeType = typeof(DefaultAstNode);


                var astBuilder = new AstBuilder(astContext);
                astBuilder.BuildAst(parseTree);
            }
        }

    }
}
