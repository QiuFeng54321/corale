using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.LSP;

public class DocumentHighlightHandler : DocumentHighlightHandlerBase
{
    private readonly ILogger<DocumentHighlightHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public DocumentHighlightHandler(ILogger<DocumentHighlightHandler> logger, Foo foo,
        ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        foo.SayFoo();
        logger.LogWarning("hi document highlight");
    }

    protected override DocumentHighlightRegistrationOptions CreateRegistrationOptions(
        DocumentHighlightCapability capability,
        ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
    };

    public override async Task<DocumentHighlightContainer?> Handle(DocumentHighlightParams request,
        CancellationToken cancellationToken)
    {
        var (definition, range) =
            Scope.GetHoveredVariable(_analysisService.GetAnalysis(request.TextDocument.Uri).AllVariableDefinitions,
                request.Position.ToLocation());
        if (definition == null) return new DocumentHighlightContainer();
        return new DocumentHighlightContainer(definition.References.Select(r => new DocumentHighlight
        {
            Range = r.ToRange(),
            Kind = DocumentHighlightKind.Text
        }));
    }
}