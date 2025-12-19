using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using YouTubeSessionGenerator.BotGuard;
using YouTubeSessionGenerator.Js;
using YouTubeSessionGenerator.Utils;

namespace YouTubeSessionGenerator;

/// <summary>
/// Contains methods to generate valid trusted sessions for YouTube including Visitor Data, Proof of Origin Tokens &amp; Rollout Tokens.
/// </summary>
public class YouTubeSessionCreator
{
    readonly BotGuardClient? botGuardClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="YouTubeSessionCreator"/> class.
    /// </summary>
    /// <param name="config">The configuration for this YouTube session creator</param>
    public YouTubeSessionCreator(
        YouTubeSessionConfig? config = null)
    {
        Config = config ?? new YouTubeSessionConfig();

        if (Config.JsEnvironment is not null)
            botGuardClient = new(Config.JsEnvironment, Config.Logger);
    }


    /// <summary>
    /// The configuration for this YouTube session creator.
    /// </summary>
    public YouTubeSessionConfig Config { get; }



    /// <exception cref="InvalidDataException">Occurs when the visitor data could not be extracted from the HTML content.</exception>"
    /// <exception cref="HttpRequestException">Occurs when the HTTP request fails.</exception>"
    /// <exception cref="OperationCanceledException">Occurs when this task was cancelled.</exception>
    async Task<string> ExtractContextPropertyAsync(
        string property,
        CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = new(HttpMethod.Get, Endpoints.Embed("um0ETkJABmI"));
        HttpResponseMessage respone = await Config.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        respone.EnsureSuccessStatusCode();
        string responseHtml = await respone.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        Match match = Regex.Match(responseHtml, $"\"{property}\":\"([^\"]+)");
        if (!match.Success)
            throw new InvalidDataException("Visitor data could not be extracted from the HTML content.");

        return match.Groups[1].Value;
    }


    /// <summary>
    /// Generates Visitor Data for a YouTube session.
    /// </summary>
    /// <param name="cancellationToken">The token to cancel this task.</param>
    /// <returns>The Visitor Data</returns>
    /// <exception cref="InvalidDataException">Occurs when the visitor data could not be extracted from the HTML content.</exception>"
    /// <exception cref="HttpRequestException">Occurs when the HTTP request fails.</exception>"
    /// <exception cref="OperationCanceledException">Occurs when this task was cancelled.</exception>
    public Task<string> VisitorDataAsync(
        CancellationToken cancellationToken = default) =>
        ExtractContextPropertyAsync("visitorData", cancellationToken);

    /// <summary>
    /// Generates rollout token for a YouTube session.
    /// </summary>
    /// <param name="cancellationToken">The token to cancel this task.</param>
    /// <returns>The rollout token</returns>
    /// <exception cref="InvalidDataException">Occurs when the visitor data could not be extracted from the HTML content.</exception>"
    /// <exception cref="HttpRequestException">Occurs when the HTTP request fails.</exception>"
    /// <exception cref="OperationCanceledException">Occurs when this task was cancelled.</exception>
    public Task<string> RolloutTokenAsync(
        CancellationToken cancellationToken = default) =>
        ExtractContextPropertyAsync("rolloutToken", cancellationToken);

    /// <summary>
    /// Generates a Proof of Origin Token (PoToken) for a YouTube session.
    /// </summary>
    /// <param name="identifier">The Visitor Data, Data Sync ID or Video ID connected to this proof of origin token.</param>
    /// <param name="contentBinding">The content to which the BotGuard snapshout is bound.</param>
    /// <param name="cancellationToken">The token to cancel this task.</param>
    /// <returns>The Proof of Origin Token.</returns>
    /// <exception cref="HttpRequestException">Occurs when the HTTP request fails.</exception>"
    /// <exception cref="JsonException">Occurs when the JSON could not be deseriealized.</exception>"
    /// <exception cref="InvalidOperationException">Occurs when the BotGuard client is not configured due to no JavaScript environment being provided.</exception>
    /// <exception cref="BotGuardException">Occurs when the internal BotGuard client failes to produce a result.</exception>
    /// <exception cref="JsException">Occurs when the JavaScript environment throws an error.</exception>
    /// <exception cref="OperationCanceledException">Occurs when this task was cancelled.</exception>
    public async Task<string> ProofOfOriginTokenAsync(
        string identifier,
        BotGuardContentBinding? contentBinding = null,
        CancellationToken cancellationToken = default)
    {
        if (botGuardClient is null)
            throw new InvalidOperationException("BotGuard client is not configured. Please provide a JavaScript environment.");

        // Create BotGuard challenge
        Config.Logger?.LogInformation("[YouTubeSessionGenerator-CreateProofOfOriginTokenAsync] Creating BotGuard challenge...");

        HttpRequestMessage challengeRequest = new(HttpMethod.Post, Endpoints.CreateUrl)
        {
            Content = new StringContent($"[ \"{Keys.RequestKey}\" ]", Encoding.UTF8, "application/json+protobuf"),
            Headers =
            {
                { "x-goog-api-key", Keys.GoogleApiKey },
                { "x-user-agent", Keys.GoogleUserAgent }
            }
        };
        HttpResponseMessage challengeResponse = await Config.HttpClient.SendAsync(challengeRequest, cancellationToken);

        challengeResponse.EnsureSuccessStatusCode();
        string challengeResponseBody = await challengeResponse.Content.ReadAsStringAsync(cancellationToken);

        BotGuardChallenge challenge = BotGuardChallenge.Parse(challengeResponseBody);

        // Prepare JS environment
        Config.Logger?.LogInformation("[YouTubeSessionGenerator-CreateProofOfOriginTokenAsync] Preparing JavaScript environment...");

        await botGuardClient.LoadAsync(challenge.InterpreterJs.PrivateDoNotAccessOrElseSafeScriptWrappedValue, challenge.Program, challenge.GlobalName);
        string botguardResponse = await botGuardClient.SnapshotAsync(contentBinding);


        // Retrieving Integrity Token
        Config.Logger?.LogInformation("[YouTubeSessionGenerator-CreateProofOfOriginTokenAsync] Retrieving Integrity Token...");

        HttpRequestMessage itRequest = new(HttpMethod.Post, Endpoints.GenerateItUrl)
        {
            Content = new StringContent($"[ \"{Keys.RequestKey}\", \"{botguardResponse}\" ]", Encoding.UTF8, "application/json+protobuf"),
            Headers =
            {
                { "x-goog-api-key", Keys.GoogleApiKey },
                { "x-user-agent", Keys.GoogleUserAgent },
            }
        };
        HttpResponseMessage itResponse = await Config.HttpClient.SendAsync(itRequest, cancellationToken);

        itResponse.EnsureSuccessStatusCode();
        string itResponseBody = await itResponse.Content.ReadAsStringAsync(cancellationToken);

        JsonArray itResponseRawData = JsonSerializer.Deserialize<JsonArray>(itResponseBody) ?? throw new JsonException("Failed to deserialize integrity token.");
        string integrityToken = itResponseRawData[0]?.GetValue<string>() ?? throw new JsonException("Integrity token is null.");

        // Mint poToken
        byte[] integrityTokenBytes = integrityToken.ToBytesFromBase64();
        await botGuardClient.LoadMintAsync(integrityTokenBytes);

        byte[] poTokenBytes = await botGuardClient.MintAsync(identifier);
        string poToken = poTokenBytes.ToBase64FromBytes(true);

        return poToken;
    }
}