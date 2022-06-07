using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using PseudoCode.Core.Analyzing;
using Range = System.Range;

namespace PseudoCode.LSP;

public class DiagnosticService
{
    private readonly ILanguageServerFacade _facade;
    private readonly ILogger<DiagnosticService> _logger;

    public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
    };

    public DiagnosticService(ILanguageServerFacade facade, ILogger<DiagnosticService> logger)
    {
        _facade = facade;
        _logger = logger;
        _logger.LogInformation("Diagnostic");
    }

    public void Update(DocumentUri uri, Analysis analysis)
    {
        var diagnostics = analysis.Program.AnalyserFeedbacks.Select(feedback => new Diagnostic
            {
                Severity = feedback.Severity switch
                {
                    Feedback.SeverityType.Error => DiagnosticSeverity.Error,
                    Feedback.SeverityType.Warning => DiagnosticSeverity.Warning,
                    Feedback.SeverityType.Information => DiagnosticSeverity.Information,
                    Feedback.SeverityType.Hint => DiagnosticSeverity.Hint,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Source = "pseudocode",
                Range = feedback.SourceRange.ToRange(),
                // Code = w.Kind.ToString(),
                Message = feedback.Message,
                Code = new DiagnosticCode(JsonConvert.SerializeObject(feedback, SerializerSettings))
            })
            .ToList();
        _facade.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
        {
            Uri = uri,
            Diagnostics = diagnostics
        });
    }
}
