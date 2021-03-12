# IgorTeterkin.Tables
Using C# .NET MVC application to read data from test.json and remote site, and fill-in tables in HTML using JS.
Since this was a test for JS, HTML and CSS basics, some good coding principles were violated in order to provide some logic within js. 
The application does not support Internet Explorer.

• Used MS VisualStudio 2019 - Developer Edition. 
• Created draft of the project based on empty MVC .NEt Framework application template with MVC support.
• Implemented Ajax get request and unobtrusive JS.
• Used jQuery for creating table rows and accessing existing elements.
• Used HtmlAgilityPack to download remote html.
• Used CSS bootstrap for responsive web design.
• Provided descriptive documentation.

In case of "Could not find a part of the path … bin\roslyn\csc.exe" (bug related to NuGet package) - run this in the Package Manager Console:

Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
