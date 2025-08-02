<table align="center">
  <tr>
    <td>
      <img align="middle" src="YouTubeSessionGenerator/icon.png" alt="Logo" width="80" height="80">
    </td>
    <td>
      <h1>YouTubeSessionGenerator</h1>
      <p>Generate valid trusted sessions for YouTube including VisitorData, PoTokens & RolloutTokens.</p>
      <h3>
        <a href="https://icysnex.github.io/YouTubeSessionGenerator/">
          <img alt="Documentation" src="https://img.shields.io/badge/Documentation-c44f4f?style=for-the-badge">
        </a>
        <span> ˙ </span>
        <a href="https://www.nuget.org/packages/YouTubeSessionGenerator">
          <img alt="NuGet Version" src="https://img.shields.io/nuget/v/YouTubeSessionGenerator?style=for-the-badge&color=c44f4f">
        </a>
        <span> ˙ </span>
        <a href="https://icysnex.github.io/YouTubeSessionGenerator/license.html">
          <img alt="License" src="https://img.shields.io/github/license/IcySnex/YouTubeSessionGenerator?style=for-the-badge&color=c44f4f">
        </a>
      </h3>
    </td>
  </tr>
</table>

---

## ✨ Features
- **Trusted Sessions**: Generate sessions that pass YouTube’s anti-bot checks - unlock access to protected endpoints
- **Easy to Use**: Just a few lines of code required - no need to worry about the low-level details
- **Configurable**: Built for .NET with flexible config & pluggable JS environment support - no browser needed
<br><br>

## 🚀 Getting Started
```bash
# Install the latest version from NuGet
dotnet add package YouTubeSessionGenerator
```

```cs
using YouTubeSessionGenerator;
using YouTubeSessionGenerator.Js.Environments;

using NodeEnvironment myCustomJsEnvironment = new(); // Make sure this gets disposed!

YouTubeSessionConfig config = new()
{
    JsEnvironment = myCustomJsEnvironment,  // Required when generating Proof of Origin Tokens
    HttpClient = myCustomHttpClient,        // Optional: Provide your own HttpClient
    Logger = myCustomLogger                 // Optional: Enable logging
};
YouTubeSessionCreator creator = new(config);

string visitorData = await creator.VisitorDataAsync();
string poToken = await creator.ProofOfOriginTokenAsync(visitorData);
string rolloutToken = await creator.RolloutTokenAsync();
```
To learn more, visit the full [Getting Started](https://icysnex.github.io/YouTubeSessionGenerator/guide/getting-started.html) & [Configuration](https://icysnex.github.io/YouTubeSessionGenerator/guide/configuration.html) Guide.
<br><br>

## 🧠 How It Works
YouTube uses several internal tokens to validate client requests. This library reverse-engineers the behavior of official YouTube clients to generate those tokens, including:
- **Visitor Data**: Session-level identity token
- **Proof of Origin Token**: BotGuard-validated, JS-signed token
- **Rollout Token**: Determines feature flags and experiments per session

Fore more details, see [here](https://icysnex.github.io/YouTubeSessionGenerator/guide/).
<br><br>

## 🙏 Credits
This library was made possible thanks to the incredible reverse-engineering work and research by the open-source community, especially those who uncovered and shared insights into YouTube’s internal APIs.

Special thanks to the following projects and individuals:
- [LuanRT/BgUtils](https://github.com/LuanRT/BgUtils) - for their extensive description of how PoToken are generated.
- [LuanRT/YouTube.js](https://github.com/LuanRT/YouTube.js) - for their detailed understanding of the InnerTube API.
- [yt-dlp](https://github.com/yt-dlp/yt-dlp/wiki/PO-Token-Guide) - for their research of when and where PoTokens are required.
- [Hao Kuang](https://kuangbyte.medium.com/peeking-behind-the-curtain-decoding-youtubes-api-design-through-network-traffic-e3a68463df05) - for their detailed explanation of YouTube's internal API design.
