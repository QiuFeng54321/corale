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

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public CompletionHandler(ILogger<CompletionHandler> logger, ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
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
        _logger.LogInformation($"Completion at {request.Position}");
        var completionBuilder = new CompletionItemsBuilder();
        completionBuilder.AddBasicCompletionItems();
        var analysis = _analysisService.GetAnalysis(request.TextDocument.Uri);
        // ANTLR4 location line starts with 1
        var cursor = request.Position.ToLocation();
        _logger.LogInformation(cursor.ToString());
        
        completionBuilder.AddDefinitions(analysis, cursor);
        _logger.LogWarning("Complete complete");
        return new CompletionList(completionBuilder.Items);
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completion2");
        return Task.FromResult(request);
    }
}