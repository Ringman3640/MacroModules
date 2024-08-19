# MacroModules.App

The App project implements the GUI application that allows users to visually interact with MacroModules.Model through a block-programming model. It additionally implements common Windows application features such as copy and paste, undo and redo, and macro saving and loading.

This project follows the MVVM architecture pattern, which separates the application view from the application logic. In the long-term, this improves troubleshooting and simplifies development through separation of concerns (you don't need to worry about breaking the app logic when changing the view and vice versa).

The following is a class diagram that provides an overview of the structure of the project. The class diagram only covers the major classes in the library. Smaller and repetitive classes, such as individual module and property views, are not included for brevity.

<div align="center">
  <img src="/src/MacroModules.App/readme-images/app-class-diagram.png" alt="A class diagram of the MacroModules.App project library"/>
</div>

The class diagram file can be found [here](https://drive.google.com/file/d/1mhjlr0qb6uxkux4KH07x518Ccc9zuCif/view?usp=drive_link). It can be opened in [draw.io](https://app.diagrams.net/).
