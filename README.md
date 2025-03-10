# Kusto Query Language

Kusto Query Language is a simple yet powerful language to query structured, semi-structured, and unstructured data. It assumes a relational data model of tables and columns with a minimal set of data types. The language is very expressive, easy to read and understand the query intent, and optimized for authoring experiences. The Kusto Query Language is optimal for querying telemetry, metrics, and logs with deep support for text search and parsing, time-series operators and functions, analytics and aggregation, geospatial, vector similarity searches, and many other language constructs that provide the optimal language for data analysis.

For documentation, see [Kusto Query Language (KQL) overview](https://learn.microsoft.com/en-us/kusto/query/?view=azure-data-explorer).

## Content

This repo contains a C# parser and a semantic analyzer as well as a translator project that generates the same libraries in Java Script. See [usage examples](src/Kusto.Language/readme.md)

## API Package

This source code is also available as a [package on nuget.org](https://www.nuget.org/packages/Microsoft.Azure.Kusto.Language/)

## Query Editor

If you need to provide a query authoring experience for the language, consider using the [Kusto language plugin for the Monaco Editor](https://github.com/Azure/monaco-kusto)

## Contribute

There are many ways to contribute to Kusto Query Language.

* [Submit bugs](https://github.com/microsoft/Kusto-Query-Language/issues) and help us verify fixes as they are checked in.
* Review the [source code changes](https://github.com/microsoft/Kusto-Query-Language/commits/master).
* Engage with other Kusto Query Language users and developers on [StackOverflow](https://stackoverflow.com/questions/tagged/kusto-query-language).

## Getting Help / Reporting Problems

* [Stack Overflow](https://stackoverflow.com/) - Ask questions about how to use Kusto. Start posts with 'KQL'. This is monitored by Kusto team members.
* [User Voice](https://aka.ms/adx.uservoice) - Suggest new features or changes to existing features.
* [Azure Data Explorer](https://dataexplorer.azure.com) - Give feedback or report problems using the user feedback button (top-right near settings).
* [Azure Support](https://learn.microsoft.com/en-us/azure/azure-portal/supportability/how-to-create-azure-support-request) - Report problems with the Kusto service.
* Open an issue here - for problems specifically with this library.
* Start a discussion - talk about this library, or anything related to Kusto.

## Microsoft Open Source Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).

Resources:

* [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/)
* [Microsoft Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/)
* Contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with questions or concerns




