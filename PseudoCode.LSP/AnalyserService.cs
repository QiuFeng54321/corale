using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;

namespace PseudoCode.LSP;

public class AnalysisService
{
    private readonly Dictionary<DocumentUri, Analysis> analyses = new();
    private readonly ILogger<AnalysisService> logger;
    private readonly BufferService documentService;

    public AnalysisService(ILogger<AnalysisService> logger, BufferService documentService)
    {
        this.logger = logger;
        this.documentService = documentService;
    }

    public void Reparse(DocumentUri key)
    {
        var cts = new CancellationTokenSource();
        lock (analyses)
        {
            if (!analyses.ContainsKey(key))
            {
                logger.LogError($"{key} not found");
            }

            analyses[key] = Analyse(key, cts.Token);
        }
    }

    public Analysis GetAnalysis(DocumentUri key)
    {
        return analyses[key];
    }

    public Analysis Analyse(DocumentUri key, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        var source = documentService.GetText(key); // XXX is this a race condition?
        var doc = new Analysis();

        try
        {
            AnalyseSyntax(doc, key, source, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }

        stopwatch.Stop();
        logger.LogInformation("Analysed {Uri} in {ElapsedMilliseconds}ms", key, stopwatch.ElapsedMilliseconds);


        try
        {
            AnalyseSemantics(doc, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }

        return doc;
    }

    private void AnalyseSyntax(Analysis analysis, DocumentUri uri,
        string source, CancellationToken ct)
    {
        logger.LogInformation($"Analysing: {source}");
        // tokenize the whole document. unlike parsing, this is not line-by-line, so a single
        // ! will result in untypedTokens.Location extending to the end of the document
        // assume the bad "token" is \W and, if possible, parse everything up to it

        // XXX we could improve this further by *continuing* tokenization from the next lineseparator and stitching the parts
        try
        {
            analysis.Analyse(source);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
        }
    }

    private void AnalyseSemantics(Analysis analysis, CancellationToken ct)
    {
    }
}