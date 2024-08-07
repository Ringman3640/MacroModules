# Source Structure

The MacroModules solution is separated into four separate projects.

### MacroModules.MacroLibrary
`MacroLibrary` acts as a wrapper for the Windows API when system calls are needed (such as when monitoring, modifying, and sending mouse and keyboard inputs). Any calls to the Windows API are handled through classes from this project. The wrappers simplify calls to the Windows API and isolates all P/Invokes (which is better for maintainability and debugging).

### MacroModules.Model
The `Model` is responsible for implementing all the functionality necessary to support macro creation and execution. This includes the implementation of Modules and macro executors. With the Model, macros can be designed and executed programatically. The `Model` relies on `MacroLibrary` to implement Module behaviors.

### MacroModules.App
The MacroModules `App` is the application that provides GUI access to the Model. It implements the block programming model using the [Windows Presentation Foundation (WPF)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/getting-started/introduction-to-wpf-in-vs?view=netframeworkdesktop-4.8) framework following the Model-View-ViewModel (MVVM) architecture pattern. The App implements the View and ViewModel portions of MVVM. It then relies on the `Model` to provide macro functionality. `App` also relies on `AppControls` for common WPF controls.

### MacroModules.AppControls
`AppControls` is an independent project that defines several WPF controls used by the App. These controls include general buttons and Module property editors. `AppControls` is separate from `App` since the controls are not defined using the MVVM pattern for faster and more simple development.
