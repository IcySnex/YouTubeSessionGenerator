# YouTubeSessionCreator
Contains methods to generate valid trusted sessions for YouTube including Visitor Data, Proof of Origin Tokens &amp; Rollout Tokens.
- **Type:** Class
- **Namespace:** [YouTubeSessionGenerator](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/)
```cs
public class YouTubeSessionCreator
```


## Constructors
Initializes a new instance of the [YouTubeSessionCreator](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/YouTubeSessionCreator.html) class.
```cs
public YouTubeSessionCreator(
  YouTubeSessionConfig config)
```
| Parameter | Summary |
| --------- | ------- |
| [`YouTubeSessionConfig`](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/YouTubeSessionConfig.html) config | The configuration for this YouTube session creator |



## Methods

### VisitorDataAsync
Generates Visitor Data for a YouTube session.
```cs
public Task<string> VisitorDataAsync(
  CancellationToken cancellationToken)
```
| Parameter | Summary |
| --------- | ------- |
| *(optional)* [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) cancellationToken | The token to cancel this task. |

### RolloutTokenAsync
Generates rollout token for a YouTube session.
```cs
public Task<string> RolloutTokenAsync(
  CancellationToken cancellationToken)
```
| Parameter | Summary |
| --------- | ------- |
| *(optional)* [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) cancellationToken | The token to cancel this task. |

### ProofOfOriginTokenAsync
Generates a Proof of Origin Token (PoToken) for a YouTube session.
```cs
public Task<string> ProofOfOriginTokenAsync(
  string identifier, 
  BotGuardContentBinding contentBinding, 
  CancellationToken cancellationToken)
```
| Parameter | Summary |
| --------- | ------- |
| [`string`](https://learn.microsoft.com/dotnet/api/system.string) identifier | The Visitor Data, Data Sync ID or Video ID connected to this proof of origin token. |
| *(optional)* [`BotGuardContentBinding`](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/BotGuard/BotGuardContentBinding.html) contentBinding | The content to which the BotGuard snapshout is bound. |
| *(optional)* [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) cancellationToken | The token to cancel this task. |



## Properties

### Config
The configuration for this YouTube session creator.
- **Type:** [YouTubeSessionConfig](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/YouTubeSessionConfig.html)
- **Is Read Only:** `True`
