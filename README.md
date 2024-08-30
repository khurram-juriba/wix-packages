# wix-packages 1.4

## Scenarios
- Creating Shortcuts
- Writing to special folders, Desktop, My Documents, ProgramData
- Writing to USERPROFILE folder
- Creating Symbolic Links
- Creating Scheduled Tasks
- Creating Environment Variables
- Setup installing MSIs; Simple Application /w Runtime
	- setup.exe install theconsole
		- This will install TheConsoleApp.exe in "C:\Program Files (x86)\Juriba\TheConsoleApp"
	- setup.exe install simpleapp
	- setup.exe install simpleapp runtime
- User/System Registry scenarios
- Event Logs, Exit Codes and Reboot
	- "C:\Program Files (x86)\Juriba\TheConsoleApp\TheConsoleApp.exe" eventlogs
	- "C:\Program Files (x86)\Juriba\TheConsoleApp\TheConsoleApp.exe" exitcode 5000
	- "C:\Program Files (x86)\Juriba\TheConsoleApp\TheConsoleApp.exe" reboot

## Features
- MSI + Setup app
- Administrative launch for setup (for scheduled tasks + symbolic links)
- Verification in setup + console
- Uninstallation in MSI + Setup app
