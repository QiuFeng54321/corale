using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace PseudoCode.LSP;

public class ReferencesHandler : ReferencesHandlerBase
{
    private readonly ILogger<ReferencesHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public ReferencesHandler(ILogger<ReferencesHandler> logger, Foo foo, ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        foo.SayFoo();
        logger.LogWarning("hi references");
    }
    protected override ReferenceRegistrationOptions CreateRegistrationOptions(ReferenceCapability capability,
        ClientCapabilities clientCapabilities) => new ()
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
    };

    public override async Task<LocationContainer> Handle(ReferenceParams request, CancellationToken cancellationToken)
    {
        var (definition, range) = _analysisService.GetAnalysis(request.TextDocument.Uri).Program.GlobalScope
            .GetHoveredVariable(request.Position.ToLocation());
        if (definition == null) return new();
        return new LocationContainer(definition.References.Select(r => new Location
        {
            Range = r.ToRange(),
            Uri = request.TextDocument.Uri
        }));
    }
}