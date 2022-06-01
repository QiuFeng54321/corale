using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace PseudoCode.LSP;

public class CompletionHandler : CompletionHandlerBase
{
    private readonly ILogger<CompletionHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;

    private readonly DocumentSelector _documentSelector = new DocumentSelector(
        new DocumentFilter {
            Pattern = "**/*.pseudo"
        }
    );

    public CompletionHandler(ILogger<CompletionHandler> logger, ILanguageServerConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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
        return new CompletionList(completions);
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completion2");
        return Task.FromResult(request);
    }
}