using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace PseudoCode.LSP;

class DefinitionHandler : DefinitionHandlerBase
{
    private readonly ILogger<DefinitionHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public DefinitionHandler(ILogger<DefinitionHandler> logger, Foo foo, ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        foo.SayFoo();
        logger.LogWarning("hi");
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(DefinitionCapability capability,
        ClientCapabilities clientCapabilities) => new DefinitionRegistrationOptions
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
    };


    public override async Task<LocationOrLocationLinks> Handle(DefinitionParams request,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("hover");
        var (definition, range) = _analysisService.GetAnalysis(request.TextDocument.Uri).Program.GlobalScope
            .GetHoveredVariable(request.Position.ToLocation());
        if (definition == null) return null;
        var link = new LocationLink
        {
            OriginSelectionRange = new Range(range.Start.ToPosition(), range.End.ToPosition()),
            TargetRange = new Range(definition.SourceRange.Start.ToPosition(), definition.SourceRange.End.ToPosition()),
            TargetSelectionRange = new Range(definition.SourceRange.Start.ToPosition(),
                definition.SourceRange.End.ToPosition()),
            TargetUri = request.TextDocument.Uri
        };
        return new LocationOrLocationLinks(link);
    }
}