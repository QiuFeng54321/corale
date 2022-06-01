using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

namespace PseudoCode.LSP;

class TextDocumentSyncHandler : TextDocumentSyncHandlerBase
{
    private readonly ILogger<TextDocumentSyncHandler> logger;
    private readonly ILanguageServerConfiguration configuration;
    private readonly BufferService documentService;
    private readonly AnalysisService semanticService;

    public TextDocumentSyncHandler(ILogger<TextDocumentSyncHandler> logger, ILanguageServerConfiguration configuration,
        BufferService documentService, AnalysisService semanticService)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.documentService = documentService;
        this.semanticService = semanticService;
        logger.LogWarning("Sync Handler");
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(
        SynchronizationCapability capability, ClientCapabilities clientCapabilities) => new()
    {
        DocumentSelector = DocumentSelector.ForLanguage("pseudocode"),
        Change = TextDocumentSyncKind.Incremental,
        Save = new SaveOptions { IncludeText = false } // we don't need it for anything
    };

    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new(uri, "pseudocode");

    public override async Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"{request.TextDocument.Uri} Open {request.TextDocument.Text}");
        documentService.Add(request.TextDocument.Uri, request.TextDocument.Text);

        semanticService.Reparse(request.TextDocument.Uri);
        return Unit.Value;
    }

    public override Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"{request.TextDocument.Uri} Close");
        if (configuration.TryGetScopedConfiguration(request.TextDocument.Uri, out var disposable))
        {
            disposable.Dispose();
        }

        documentService.Remove(request.TextDocument.Uri);

        return Unit.Task;
    }

    public override async Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"{request.TextDocument.Uri} Change");
        foreach (var change in request.ContentChanges)
        {
            logger.LogInformation(change.Text);
            if (change.Range != null)
            {
                documentService.ApplyIncrementalChange(request.TextDocument.Uri, change.Range, change.Text);
            }
            else
            {
                documentService.ApplyFullChange(request.TextDocument.Uri, change.Text);
            }
        }
        semanticService.Reparse(request.TextDocument.Uri);
        return Unit.Value;
    }

    public override Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }
}