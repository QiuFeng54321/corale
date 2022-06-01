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

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public HoverHandler(ILogger<HoverHandler> logger, Foo foo, ILanguageServerConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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
        return new Hover
        {
            Range = new Range {Start = new Position{Character = 1, Line = 1}, End = new Position{Character = 2, Line = 1}},
            Contents = new MarkedStringsOrMarkupContent(new MarkupContent
                { Kind = MarkupKind.Markdown, Value = "hi" })
        };
    }
}