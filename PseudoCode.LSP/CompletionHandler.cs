using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using PseudoCode.Core.Runtime;

namespace PseudoCode.LSP;

public class CompletionHandler : CompletionHandlerBase
{
    private readonly ILogger<CompletionHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;
    
    private readonly BufferService documentService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public CompletionHandler(ILogger<CompletionHandler> logger, ILanguageServerConfiguration configuration, AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        _logger.LogInformation("Completion yay");
    }
    protected override CompletionRegistrationOptions CreateRegistrationOptions(CompletionCapability capability,
        ClientCapabilities clientCapabilities)
    {
        return new CompletionRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
        };
    }
    

    public override async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completion");
        var completions = new List<CompletionItem>();
        var analysis = _analysisService.GetAnalysis(request.TextDocument.Uri);
        var cursor = new SourceLocation(request.Position.Line, request.Position.Character);
        _logger.LogInformation(cursor.ToString());
        var variableInfos = analysis
            .Program
            .GlobalScope
            .TypeTable
            .GetVariableCompletionBefore(cursor);
        foreach (var variableInfo in variableInfos)
        {
            completions.Add(new CompletionItem
            {
                Kind = CompletionItemKind.Variable,
                Label = variableInfo.Name,
                InsertText = variableInfo.Name,
                Documentation = new MarkupContent
                {
                    Kind = MarkupKind.Markdown, 
                    Value = $"```pseudocode\n{variableInfo.Type}```"
                },
            });
        }
        var t = $"{request.PartialResultToken}-{request.WorkDoneToken}-{request.Position}";
        var ins = "hiiiiiii";
        _logger.LogInformation($"T: {t}");
        completions.Add(new CompletionItem
        {
            Kind = CompletionItemKind.Class,
            Label = "hi",
            InsertText = ins,
            Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = t },
        });
        _logger.LogWarning("Complete complete");
        return new CompletionList(completions);
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completion2");
        return Task.FromResult(request);
    }
}