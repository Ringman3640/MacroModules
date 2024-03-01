.\SvgToXaml\SvgToXaml.exe BuildDict /inputdir:.\SvgSource /outputname:ModuleIcons /outputdir:. /buildhtmlfile:false

powershell -Command "(Get-Content ModuleIcons.xaml) -replace '#FF000000', '{StaticResource Module.Icon.Color}' | Out-File -encoding ASCII ModuleIcons.xaml"