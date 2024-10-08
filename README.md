# MacroModules

An application for designing and running custom macros.

## About
<div align="center">
  <img src="/readme-images/about-screenshot-1.png" alt="A screenshot of the application" width="70%"/>
</div>

MacroModules is a Windows desktop application for designing and running your own custom macros.

A macro is a set of computer actions that execute sequentially, usually triggered by a user input such as a key press. They allow for repetitive computer tasks to be partially or fully automated. 

MacroModules uses a block programming model to design macros. Individual computer actions are represented by visual blocks called **Modules** that can be connected together with other Modules. A macro is made a chain consisting of one or more connected Modules.

The length of a macro varies based on the complexity of the macro behavior. A macro may consist of one or two Modules to support accessibility needs. A macro may also consist of tens of Modules to fully automate a tedious computer task.

<div align="center">
  <img src="/readme-images/about-screenshot-2.png" alt="A screenshot of a large macro consisting of many Modules" width="70%"/>
</div>

This project was created for my 2024 capstone project at UW Bothell. Information about its development can be found at the project's [drive folder](https://drive.google.com/drive/folders/1FRP3eGT4JY8pzh0u0oQeg14dNLEAfzBx?usp=sharing).

## Build
An executable file cannot be provided as I do not have a code signing certificate. The application must be built from source. This is done using [Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/community/). Be sure to check `.NET desktop development` during installation.

1. Clone the source code
2. Open `MacroModules.sln`
3. Make sure the build is set to `Release` and the startup project is `MarcoModules.App`
4. Build the project from the toolbar or using `CTRL+SHIFT+B`

When built, the generated executable file can be found at `/src/MacroModules.App/bin/Release/net8.0-windows`.

## Status and Future Updates
The project is currently incomplete and is missing many key features that were originally planned. The current project architecture makes it very difficult to progress with development without doing very expensive reworks.

I intend to eventually complete this project, but I do not know when that will happen. It may be in my best interest to completely rework the project as the application currently has major performance issues (it can reach more than 1 gb of RAM usage wtf). This may also be the easier option was I will need to rework a lot of the architecture anyways. 
