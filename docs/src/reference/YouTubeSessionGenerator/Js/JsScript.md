﻿# JsScript
Represents a JavaScript script to be executed in a JavaScript environment,
- **Type:** Class
- **Namespace:** [YouTubeSessionGenerator.Js](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/Js/)
```cs
public class JsScript
```


## Constructors
Initializes a new instance of the [JsScript](/YouTubeSessionGenerator/reference/YouTubeSessionGenerator/Js/JsScript.html) class.
```cs
public JsScript(
  string code, 
  Object[] args)
```
| Parameter | Summary |
| --------- | ------- |
| [`string`](https://learn.microsoft.com/dotnet/api/system.string) code | The JavaScript code to execute. |
| [`object[]`](https://learn.microsoft.com/dotnet/api/system.object) args | An optional array of arguments that can be referenced within the script using <c>args</c>. |





## Properties

### Code
The JavaScript code to execute.
- **Type:** [string](https://learn.microsoft.com/dotnet/api/system.string)
- **Is Read Only:** `False`

### Args
The arguments that will be available to the script as the <c>args</c> variable.
- **Type:** [object[]](https://learn.microsoft.com/dotnet/api/system.object)
- **Is Read Only:** `False`
