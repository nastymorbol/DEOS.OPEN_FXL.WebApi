<a name='assembly'></a>
# OPEN_FXL.WebApi

## Contents

- [Client](#T-FxlApiV1-Client 'FxlApiV1.Client')
  - [AddLogEntryAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-AddLogEntryAsync-System-String,FxlApiV1-LogEntry,System-Threading-CancellationToken- 'FxlApiV1.Client.AddLogEntryAsync(System.String,FxlApiV1.LogEntry,System.Threading.CancellationToken)')
  - [BuildControllerProgramAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-BuildControllerProgramAsync-System-String,FxlApiV1-BuildParameters,System-Threading-CancellationToken- 'FxlApiV1.Client.BuildControllerProgramAsync(System.String,FxlApiV1.BuildParameters,System.Threading.CancellationToken)')
  - [CleanControllerProgramAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-CleanControllerProgramAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.CleanControllerProgramAsync(System.String,System.Threading.CancellationToken)')
  - [CreateControllerAsync(cancellationToken,projectId)](#M-FxlApiV1-Client-CreateControllerAsync-System-String,FxlApiV1-ControllerInfo,System-Threading-CancellationToken- 'FxlApiV1.Client.CreateControllerAsync(System.String,FxlApiV1.ControllerInfo,System.Threading.CancellationToken)')
  - [CreateProjectAsync(cancellationToken)](#M-FxlApiV1-Client-CreateProjectAsync-FxlApiV1-ProjectInfo,System-Threading-CancellationToken- 'FxlApiV1.Client.CreateProjectAsync(FxlApiV1.ProjectInfo,System.Threading.CancellationToken)')
  - [DeleteControllerAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-DeleteControllerAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.DeleteControllerAsync(System.String,System.Threading.CancellationToken)')
  - [DeleteFupPageAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-DeleteFupPageAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.DeleteFupPageAsync(System.String,System.Threading.CancellationToken)')
  - [DeleteProjectAsync(cancellationToken,projectId)](#M-FxlApiV1-Client-DeleteProjectAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.DeleteProjectAsync(System.String,System.Threading.CancellationToken)')
  - [GetControllerInfoAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-GetControllerInfoAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetControllerInfoAsync(System.String,System.Threading.CancellationToken)')
  - [GetControllersAsync(cancellationToken,projectId)](#M-FxlApiV1-Client-GetControllersAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetControllersAsync(System.String,System.Threading.CancellationToken)')
  - [GetDefinitionsAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-GetDefinitionsAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetDefinitionsAsync(System.String,System.Threading.CancellationToken)')
  - [GetFupPageInfoAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-GetFupPageInfoAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetFupPageInfoAsync(System.String,System.Threading.CancellationToken)')
  - [GetFupPagesAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-GetFupPagesAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetFupPagesAsync(System.String,System.Threading.CancellationToken)')
  - [GetProcessStatusAsync(cancellationToken,processId)](#M-FxlApiV1-Client-GetProcessStatusAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetProcessStatusAsync(System.String,System.Threading.CancellationToken)')
  - [GetProjectInfoAsync(cancellationToken,projectId)](#M-FxlApiV1-Client-GetProjectInfoAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.GetProjectInfoAsync(System.String,System.Threading.CancellationToken)')
  - [GetProjectsAsync(cancellationToken)](#M-FxlApiV1-Client-GetProjectsAsync-System-Threading-CancellationToken- 'FxlApiV1.Client.GetProjectsAsync(System.Threading.CancellationToken)')
  - [InsertFupPageAsync(cancellationToken,controllerId)](#M-FxlApiV1-Client-InsertFupPageAsync-System-String,FxlApiV1-FupPageInfo,System-Threading-CancellationToken- 'FxlApiV1.Client.InsertFupPageAsync(System.String,FxlApiV1.FupPageInfo,System.Threading.CancellationToken)')
  - [SetDefinitionsAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-SetDefinitionsAsync-System-String,System-Collections-Generic-IEnumerable{FxlApiV1-Definition},System-Threading-CancellationToken- 'FxlApiV1.Client.SetDefinitionsAsync(System.String,System.Collections.Generic.IEnumerable{FxlApiV1.Definition},System.Threading.CancellationToken)')
  - [SetFupPageInfoAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-SetFupPageInfoAsync-System-String,FxlApiV1-FupPageInfo,System-Threading-CancellationToken- 'FxlApiV1.Client.SetFupPageInfoAsync(System.String,FxlApiV1.FupPageInfo,System.Threading.CancellationToken)')
  - [UpdateMacroAsync(cancellationToken,fupPageId)](#M-FxlApiV1-Client-UpdateMacroAsync-System-String,System-Threading-CancellationToken- 'FxlApiV1.Client.UpdateMacroAsync(System.String,System.Threading.CancellationToken)')
- [FxlClient](#T-FXL-WebApi-FxlClient 'FXL.WebApi.FxlClient')
  - [#ctor(client)](#M-FXL-WebApi-FxlClient-#ctor-FxlApiV1-Client- 'FXL.WebApi.FxlClient.#ctor(FxlApiV1.Client)')
  - [CreateProjectAsync(name,company,language,programmer,utf8)](#M-FXL-WebApi-FxlClient-CreateProjectAsync-System-String,System-String,FxlApiV1-ProjectInfoLanguage,System-String,System-Boolean- 'FXL.WebApi.FxlClient.CreateProjectAsync(System.String,System.String,FxlApiV1.ProjectInfoLanguage,System.String,System.Boolean)')
  - [DeleteProject(project)](#M-FXL-WebApi-FxlClient-DeleteProject-FXL-WebApi-Project- 'FXL.WebApi.FxlClient.DeleteProject(FXL.WebApi.Project)')
  - [GetControllerAsync(projectname,controllername)](#M-FXL-WebApi-FxlClient-GetControllerAsync-System-String,System-String- 'FXL.WebApi.FxlClient.GetControllerAsync(System.String,System.String)')
  - [GetFupPageAsync(projectname,controllername,fupFilename)](#M-FXL-WebApi-FxlClient-GetFupPageAsync-System-String,System-String,System-String- 'FXL.WebApi.FxlClient.GetFupPageAsync(System.String,System.String,System.String)')
  - [GetProjectAsync()](#M-FXL-WebApi-FxlClient-GetProjectAsync-System-String- 'FXL.WebApi.FxlClient.GetProjectAsync(System.String)')
  - [GetProjectsAsync()](#M-FXL-WebApi-FxlClient-GetProjectsAsync 'FXL.WebApi.FxlClient.GetProjectsAsync')

<a name='T-FxlApiV1-Client'></a>
## Client `type`

##### Namespace

FxlApiV1

<a name='M-FxlApiV1-Client-AddLogEntryAsync-System-String,FxlApiV1-LogEntry,System-Threading-CancellationToken-'></a>
### AddLogEntryAsync(cancellationToken,controllerId) `method`

##### Summary

Add an entry to the log file.

##### Returns

Log entry successfully added.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [FxlApiV1.LogEntry](#T-FxlApiV1-LogEntry 'FxlApiV1.LogEntry') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-BuildControllerProgramAsync-System-String,FxlApiV1-BuildParameters,System-Threading-CancellationToken-'></a>
### BuildControllerProgramAsync(cancellationToken,controllerId) `method`

##### Summary

Build controller program.

##### Returns

Controller program build request accepted.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [FxlApiV1.BuildParameters](#T-FxlApiV1-BuildParameters 'FxlApiV1.BuildParameters') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-CleanControllerProgramAsync-System-String,System-Threading-CancellationToken-'></a>
### CleanControllerProgramAsync(cancellationToken,controllerId) `method`

##### Summary

Clean controller program.

##### Returns

Controller program successfully cleaned.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-CreateControllerAsync-System-String,FxlApiV1-ControllerInfo,System-Threading-CancellationToken-'></a>
### CreateControllerAsync(cancellationToken,projectId) `method`

##### Summary

Creates a new controller.

##### Returns

Controller successfully created.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| projectId | [FxlApiV1.ControllerInfo](#T-FxlApiV1-ControllerInfo 'FxlApiV1.ControllerInfo') | Id of project. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-CreateProjectAsync-FxlApiV1-ProjectInfo,System-Threading-CancellationToken-'></a>
### CreateProjectAsync(cancellationToken) `method`

##### Summary

Creates a new project.

##### Returns

Project successfully created.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [FxlApiV1.ProjectInfo](#T-FxlApiV1-ProjectInfo 'FxlApiV1.ProjectInfo') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-DeleteControllerAsync-System-String,System-Threading-CancellationToken-'></a>
### DeleteControllerAsync(cancellationToken,controllerId) `method`

##### Summary

Deletes a controller.

##### Returns

Controller successfully deleted.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-DeleteFupPageAsync-System-String,System-Threading-CancellationToken-'></a>
### DeleteFupPageAsync(cancellationToken,fupPageId) `method`

##### Summary

Deletes a FUP page.

##### Returns

FUP page successfully deleted.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of FUP page. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-DeleteProjectAsync-System-String,System-Threading-CancellationToken-'></a>
### DeleteProjectAsync(cancellationToken,projectId) `method`

##### Summary

Deletes a project.

##### Returns

Project successfully deleted.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| projectId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of project. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetControllerInfoAsync-System-String,System-Threading-CancellationToken-'></a>
### GetControllerInfoAsync(cancellationToken,controllerId) `method`

##### Summary

Returns the info for a controller.

##### Returns

A JSON object with project info.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetControllersAsync-System-String,System-Threading-CancellationToken-'></a>
### GetControllersAsync(cancellationToken,projectId) `method`

##### Summary

Returns the list of controllers in a project.

##### Returns

A JSON array with controllers.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| projectId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of project. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetDefinitionsAsync-System-String,System-Threading-CancellationToken-'></a>
### GetDefinitionsAsync(cancellationToken,fupPageId) `method`

##### Summary

Returns the definitions of a macro.

##### Returns

A JSON object with list of definitions.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of fup page/macro. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetFupPageInfoAsync-System-String,System-Threading-CancellationToken-'></a>
### GetFupPageInfoAsync(cancellationToken,fupPageId) `method`

##### Summary

Returns the info for a FUP page.

##### Returns

A JSON object with fuppage info.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of FUP page. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetFupPagesAsync-System-String,System-Threading-CancellationToken-'></a>
### GetFupPagesAsync(cancellationToken,controllerId) `method`

##### Summary

Returns the list of fuppages of a controller.

##### Returns

A JSON object with a list of fuppages.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetProcessStatusAsync-System-String,System-Threading-CancellationToken-'></a>
### GetProcessStatusAsync(cancellationToken,processId) `method`

##### Summary

Return the status of a long running process.

##### Returns

Process is still running

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| processId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of process. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetProjectInfoAsync-System-String,System-Threading-CancellationToken-'></a>
### GetProjectInfoAsync(cancellationToken,projectId) `method`

##### Summary

Returns the info for a project.

##### Returns

A JSON object with project info.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| projectId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of project. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-GetProjectsAsync-System-Threading-CancellationToken-'></a>
### GetProjectsAsync(cancellationToken) `method`

##### Summary

Returns the list of visible projects in the current workspace.

##### Returns

A JSON array of project names.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-InsertFupPageAsync-System-String,FxlApiV1-FupPageInfo,System-Threading-CancellationToken-'></a>
### InsertFupPageAsync(cancellationToken,controllerId) `method`

##### Summary

Insert fup page/macro.

##### Returns

A JSON object with fuppage info.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| controllerId | [FxlApiV1.FupPageInfo](#T-FxlApiV1-FupPageInfo 'FxlApiV1.FupPageInfo') | Id of controller. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-SetDefinitionsAsync-System-String,System-Collections-Generic-IEnumerable{FxlApiV1-Definition},System-Threading-CancellationToken-'></a>
### SetDefinitionsAsync(cancellationToken,fupPageId) `method`

##### Summary

Sets definitions of a macro.

##### Returns

Definitions successfully set.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [System.Collections.Generic.IEnumerable{FxlApiV1.Definition}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{FxlApiV1.Definition}') | Id of macro. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-SetFupPageInfoAsync-System-String,FxlApiV1-FupPageInfo,System-Threading-CancellationToken-'></a>
### SetFupPageInfoAsync(cancellationToken,fupPageId) `method`

##### Summary

Changes the infos of a FUP page. Only change of macro status supported for now.

##### Returns

A JSON object with fuppage info.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [FxlApiV1.FupPageInfo](#T-FxlApiV1-FupPageInfo 'FxlApiV1.FupPageInfo') | Id of FUP page. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='M-FxlApiV1-Client-UpdateMacroAsync-System-String,System-Threading-CancellationToken-'></a>
### UpdateMacroAsync(cancellationToken,fupPageId) `method`

##### Summary

Updates a macro.

##### Returns

Macro successfully updated.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cancellationToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A cancellation token that can be used by other objects or threads to receive notice of cancellation. |
| fupPageId | [System.Threading.CancellationToken](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.CancellationToken 'System.Threading.CancellationToken') | Id of FUP page. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [FxlApiV1.ApiException](#T-FxlApiV1-ApiException 'FxlApiV1.ApiException') | A server side error occurred. |

<a name='T-FXL-WebApi-FxlClient'></a>
## FxlClient `type`

##### Namespace

FXL.WebApi

<a name='M-FXL-WebApi-FxlClient-#ctor-FxlApiV1-Client-'></a>
### #ctor(client) `constructor`

##### Summary

Connect to OPEN FXL WebServer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| client | [FxlApiV1.Client](#T-FxlApiV1-Client 'FxlApiV1.Client') |  |

<a name='M-FXL-WebApi-FxlClient-CreateProjectAsync-System-String,System-String,FxlApiV1-ProjectInfoLanguage,System-String,System-Boolean-'></a>
### CreateProjectAsync(name,company,language,programmer,utf8) `method`

##### Summary

Create a new Project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The Projectname |
| company | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The Company |
| language | [FxlApiV1.ProjectInfoLanguage](#T-FxlApiV1-ProjectInfoLanguage 'FxlApiV1.ProjectInfoLanguage') | The Language |
| programmer | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The Programmer |
| utf8 | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | The used Encoding |

<a name='M-FXL-WebApi-FxlClient-DeleteProject-FXL-WebApi-Project-'></a>
### DeleteProject(project) `method`

##### Summary

Delete the given Project

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| project | [FXL.WebApi.Project](#T-FXL-WebApi-Project 'FXL.WebApi.Project') |  |

<a name='M-FXL-WebApi-FxlClient-GetControllerAsync-System-String,System-String-'></a>
### GetControllerAsync(projectname,controllername) `method`

##### Summary

Returns an Controller within the given ProjectName

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The project name |
| controllername | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The controller name |

<a name='M-FXL-WebApi-FxlClient-GetFupPageAsync-System-String,System-String,System-String-'></a>
### GetFupPageAsync(projectname,controllername,fupFilename) `method`

##### Summary

Returns an singe Fup Page Item

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectname | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The project name |
| controllername | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The controller name |
| fupFilename | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | FUP-File Name (const.f) |

<a name='M-FXL-WebApi-FxlClient-GetProjectAsync-System-String-'></a>
### GetProjectAsync() `method`

##### Summary

Get Projects in the current active Workspace (async)

##### Returns



##### Parameters

This method has no parameters.

<a name='M-FXL-WebApi-FxlClient-GetProjectsAsync'></a>
### GetProjectsAsync() `method`

##### Summary

Get Projects in the current active Workspace

##### Returns



##### Parameters

This method has no parameters.
