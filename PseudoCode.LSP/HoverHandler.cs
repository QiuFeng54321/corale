using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace PseudoCode.LSP;

class HoverHandler : HoverHandlerBase
{
    private readonly ILogger<HoverHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public HoverHandler(ILogger<HoverHandler> logger, Foo foo, ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        foo.SayFoo();
        logger.LogWarning("hi");
    }
    protected override HoverRegistrationOptions CreateRegistrationOptions(HoverCapability capability,
        ClientCapabilities clientCapabilities) => new HoverRegistrationOptions
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
    };

    public override async Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        
        _logger.LogWarning("hover");
        var (hoveredVar, range) = _analysisService.GetAnalysis(request.TextDocument.Uri).Program.GlobalScope
            .GetHoveredVariable(request.Position.ToLocation());
        if (hoveredVar == null) return null;
        return new Hover
        {
            Range = new Range {Start = range.Start.ToPosition(), End = range.End.ToPosition()},
            Contents = new MarkedStringsOrMarkupContent(new MarkupContent
                { Kind = MarkupKind.Markdown, Value = $"{hoveredVar.Type}" })
        };
    }
}