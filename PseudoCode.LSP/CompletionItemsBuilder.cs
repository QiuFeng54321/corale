using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization.Converters;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime;

namespace PseudoCode.LSP;

public class CompletionItemsBuilder
{
    public readonly List<CompletionItem> Items = new();

    public CompletionItem MakeCompletionItem(CompletionItemKind itemKind, string label, string insertText,
        string documentation)
    {
        return new CompletionItem
        {
            Kind = itemKind,
            Label = label,
            InsertText = insertText,
            Documentation = new MarkupContent
            {
                Kind = MarkupKind.Markdown,
                Value = documentation
            }
        };
    }

    public void AddBasicCompletionItems()
    {
        AddArray();
        AddBoolean();
        AddKeywords();
    }

    public void AddTypes(Analysis analysis, SourceLocation cursor)
    {
        var typeInfos = analysis.Program.GlobalScope.GetTypeCompletionBefore(cursor);
        foreach (var typeInfo in typeInfos)
        {
            Items.Add(MakeCompletionItem(CompletionItemKind.Class, typeInfo.Name, typeInfo.Name,
                typeInfo.Type.ToString()));
        }
    }

    public void AddVariables(Analysis analysis, SourceLocation cursor)
    {
        var variableInfos = analysis.Program.GlobalScope.GetVariableCompletionBefore(cursor);
        foreach (var variableInfo in variableInfos)
        {
            Items.Add(MakeCompletionItem(CompletionItemKind.Variable, variableInfo.Name, variableInfo.Name,
                variableInfo.Type.ToString()));
        }
    }

    public void AddBoolean()
    {
        Items.Add(MakeCompletionItem(CompletionItemKind.Value, "TRUE", "TRUE", "BOOLEAN"));
        Items.Add(MakeCompletionItem(CompletionItemKind.Value, "FALSE", "FALSE", "BOOLEAN"));
    }

    public void AddArray()
    {
        Items.Add(MakeCompletionItem(CompletionItemKind.Class, "ARRAY", "ARRAY", "ARRAY"));
    }

    public void AddKeywords()
    {
        foreach (var keyword in new[]
                 {
                     "DECLARE", "OF", "CASE", "OTHERWISE", "IF", "THEN", "ENDIF", "ELSE", "WHILE", "DO", "ENDWHILE",
                     "FOR",
                     "TO", "STEP", "NEXT", "REPEAT", "UNTIL", "FUNCTION", "RETURNS", "PROCEDURE", "ENDFUNCTION",
                     "ENDPROCEDURE",
                     "CONSTANT", "INPUT", "OUTPUT", "OPENFILE", "CLOSEFILE", "READFILE", "WRITEFILE", "SEEK",
                     "PUTRECORD",
                     "GETRECORD", "READ", "WRITE", "APPEND", "RANDOM", "CALL", "RETURN", "TYPE", "ENDTYPE"
                 })
        {
            Items.Add(MakeCompletionItem(CompletionItemKind.Keyword, keyword, keyword, ""));
        }
    }
}