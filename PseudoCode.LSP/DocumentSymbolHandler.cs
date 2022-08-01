using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.LSP;

public class DocumentSymbolHandler : DocumentSymbolHandlerBase
{
    private readonly ILogger<DocumentSymbolHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public DocumentSymbolHandler(ILogger<DocumentSymbolHandler> logger, Foo foo,
        ILanguageServerConfiguration configuration,
        AnalysisService analysisService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        foo.SayFoo();
        logger.LogWarning("hi document symbol");
    }

    protected override DocumentSymbolRegistrationOptions CreateRegistrationOptions(DocumentSymbolCapability capability,
        ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode")
    };

    public override async Task<SymbolInformationOrDocumentSymbolContainer> Handle(DocumentSymbolParams request,
        CancellationToken cancellationToken)
    {
        var definitions = _analysisService.GetAnalysis(request.TextDocument.Uri).AllDefinitions;
        if (definitions == null) return new SymbolInformationOrDocumentSymbolContainer();
        return new SymbolInformationOrDocumentSymbolContainer(definitions.Select(r => new SymbolInformationOrDocumentSymbol(new DocumentSymbol
        {
            Range = r.SourceRange.ToRange(),
            SelectionRange = r.SourceRange.ToRange(),
            Kind = GetKind(r),
            Name = r.Name,
            Detail = r.ToString()
        })));
    }

    private static SymbolKind GetKind(Definition r) => r.TypeName switch
    {
        "ARRAY" => SymbolKind.Array,
        "INTEGER" or "REAL" => SymbolKind.Number,
        "STRING" or "CHARACTER" => SymbolKind.String,
        "FUNCTION" => SymbolKind.Function,
        "NULL" => SymbolKind.Null,
        "ENUM" when r.Attributes.HasFlag(Definition.Attribute.Immutable) => SymbolKind.EnumMember,
        _ => SymbolKind.Class
    };
}