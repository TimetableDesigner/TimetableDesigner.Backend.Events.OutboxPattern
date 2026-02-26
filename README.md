<p align="center"><img width="100px" src=".gitea/readme/icon.png"/></p>

<h1 align="center">TimetableDesigner.Backend.Events.OutboxPattern</h1>

<h3 align="center"><b>Advanced school timetable editor, allows you to easily create and manage school timetable.</b></h3>

<p align="center">TimetableDesigner.Backend.Events is collection of NuGet packages (and NuGet package itself) that support communication between microservices in the TimetableDesigner project using message queues. This NuGet package contains helper classes used to implement outbox pattern.</p>

---

## Informations

> [!Important]
> **For Github users:**
>
> This is only mirror repository. All changes are first uploaded to the repository <a href="https://repos.mateuszskoczek.com/TimetableDesigner/TimetableDesigner.Backend.Events.OutboxPattern">here</a>. Releases are also published on original repository. However, Github repository handles issues and pull requests for better accessibility.

## NuGet registry status

<table>
    <tr>
        <th>
            Subpackage
        </th>
        <th>
            Status
        </th>
    </tr>
    <tr>
        <td>
            <b>TimetableDesigner.Backend.Events.OutboxPattern</b>
        </td>
        <td>
            <a href="https://repos.mateuszskoczek.com/TimetableDesigner/-/packages/nuget/timetabledesigner.backend.events.outboxpattern/"><img src="https://img.shields.io/gitea/v/release/TimetableDesigner/TimetableDesigner.Backend.Events.OutboxPattern?gitea_url=https%3A%2F%2Frepos.mateuszskoczek.com%2F&display_name=release&label=nuget"></a>
        </td>
    </tr>
    <tr>
        <th colspan=2>
            Other subpackages
        </th>
    </tr>
    <tr>
        <td>
            TimetableDesigner.Backend.Events
        </td>
        <td>
            <a href="https://repos.mateuszskoczek.com/TimetableDesigner/-/packages/nuget/timetabledesigner.backend.events/"><img src="https://img.shields.io/gitea/v/release/TimetableDesigner/TimetableDesigner.Backend.Events?gitea_url=https%3A%2F%2Frepos.mateuszskoczek.com%2F&display_name=release&label=nuget"></a>
        </td>
    </tr>
    <tr>
        <td>
            TimetableDesigner.Backend.Events.Extensions.AspNetCore.OpenApi
        </td>
        <td>
            <a href="https://repos.mateuszskoczek.com/TimetableDesigner/-/packages/nuget/timetabledesigner.backend.events.extensions.aspnetcore.openapi/"><img src="https://img.shields.io/gitea/v/release/TimetableDesigner/TimetableDesigner.Backend.Events.Extensions.AspNetCore.OpenApi?gitea_url=https%3A%2F%2Frepos.mateuszskoczek.com%2F&display_name=release&label=nuget"></a>
        </td>
    </tr>
    <tr>
        <td>
            TimetableDesigner.Backend.Events.Providers.RabbitMQ
        </td>
        <td>
            <a href="https://repos.mateuszskoczek.com/TimetableDesigner/-/packages/nuget/timetabledesigner.backend.events.providers.rabbitmq/"><img src="https://img.shields.io/gitea/v/release/TimetableDesigner/TimetableDesigner.Backend.Events.Providers.RabbitMQ?gitea_url=https%3A%2F%2Frepos.mateuszskoczek.com%2F&display_name=release&label=nuget"></a>
        </td>
    </tr>
</table>

## Installation and usage

You can download package from organization package registry or .nupkg file itself from Releases tab.

To download package from organization package registry, you have to add new NuGet package source. You will need access details, which you can obtain by contacting the repository owner. 

**CLI:**

```
dotnet nuget add source --name TimetableDesigner --username <username> --password <password> https://repos.mateuszskoczek.com/api/packages/TimetableDesigner/nuget/index.json
dotnet add package --source TimetableDesigner TimetableDesigner.Backend.Events.OutboxPattern
```

**Package reference in .csproj file:**

```
<PackageReference Include="TimetableDesigner.Backend.Events.OutboxPattern" Version="<version>" />
```

## Attribution and contribution

This project is open source on MIT License, so you can just copy and upload again to your repository. But according to the license, you must include information about the original author. You can find license <a href="https://repos.mateuszskoczek.com/TimetableDesigner/TimetableDesigner.Backend.Events.OutboxPattern/src/branch/main/LICENSE">here</a>.

However, the preferred way to contribute would be to propose improvements in a pull request, through issues, or through other means of communication.