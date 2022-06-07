using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using PseudoCode.Core.Analyzing;

namespace PseudoCode.LSP;

public class CodeActionHandler : CodeActionHandlerBase
{
    private readonly ILogger<CodeActionHandler> _logger;
    private readonly ILanguageServerConfiguration _configuration;
    private readonly AnalysisService _analysisService;
    private readonly DiagnosticService _diagnosticService;

    private readonly DocumentSelector _documentSelector = DocumentSelector.ForLanguage("pseudocode");

    public CodeActionHandler(ILogger<CodeActionHandler> logger, ILanguageServerConfiguration configuration,
        AnalysisService analysisService, DiagnosticService diagnosticService)
    {
        _logger = logger;
        _configuration = configuration;
        _analysisService = analysisService;
        _diagnosticService = diagnosticService;
        _logger.LogWarning("Code action yay");
    }

    protected override CodeActionRegistrationOptions CreateRegistrationOptions(CodeActionCapability capability,
        ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode"),
        CodeActionKinds = new Container<CodeActionKind>(CodeActionKind.QuickFix),
        ResolveProvider = false
    };

    public override async Task<CommandOrCodeActionContainer> Handle(CodeActionParams request,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("Codeactioning");
        return new CommandOrCodeActionContainer(request.Context.Diagnostics.Where(d => d.Code.HasValue).Select(d => JsonConvert.DeserializeObject<Feedback>(d.Code?.String)).Where(f => f.Replacements.Count != 0)
            .Select(f => new CommandOrCodeAction(new CodeAction
            {
                Kind = CodeActionKind.QuickFix,
                Title = f.Message,
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [request.TextDocument.Uri] = f.Replacements.Select(replacement => new TextEdit
                        {
                            Range = replacement.SourceRange.ToRange(),
                            NewText = replacement.Text
                        })
                    }
                },
                
            })));
    }

    public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("CodeAction");
        return Task.FromResult(request);
    }
}