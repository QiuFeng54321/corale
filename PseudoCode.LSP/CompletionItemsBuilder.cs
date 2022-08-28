using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization.Converters;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Types;

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

    public void AddDefinitions(Analysis analysis, SourceLocation cursor)
    {
        var variableInfos = analysis.Program.GlobalScope.GetDefinitionCompletionBefore(cursor);
        foreach (var variableInfo in variableInfos)
        {
            var completionItemKind = variableInfo.Attributes.HasFlag(Definition.Attribute.Type)
                ? CompletionItemKind.Class
                : variableInfo.Type switch
                {
                    EnumType => CompletionItemKind.EnumMember,
                    FunctionType or BuiltinFunctionType => CompletionItemKind.Function,
                    _ => CompletionItemKind.Variable
                };
            var documentation = variableInfo.ToMarkupString();
            if (!string.IsNullOrEmpty(variableInfo.Documentation))
            {
                documentation = $"{documentation}\n\n{variableInfo.Documentation}";
            }
            Items.Add(MakeCompletionItem(
                completionItemKind,
                variableInfo.Name,
                variableInfo.Name,
                documentation));
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