# MyMediaRenamer

[![Actions Status](https://github.com/alisterpineda/MyMediaRenamer/workflows/Build/badge.svg)](https://github.com/alisterpineda/MyMediaRenamer/actions)

MyMediaRenamer is cross-platform bulk renaming tool for video and image files using their embedded metadata (e.g. EXIF).

## Installation

The latest release is available [here](https://github.com/alisterpineda/MyMediaRenamer/releases).

Since the binaries of the command-line interface (CLI) application are portable, there are no installation steps required to be done prior to using them. Just simply extract them to a local directory and run via console.

Note that due to the nature of portable .NET Core applications, expect a notable delay on first time use of the application.

A GUI version of this app is available for Windows only. Be sure to have installed [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) or beyond to be able to use this app.

## Usage

```
 .\MyMediaRenamer.Cli.exe --help


Usage: MyMediaRenamer [arguments] [options]

Arguments:
  File Name Pattern  Pattern used to determine the new file name of each file.
  Media Files        File(s) to rename.

Options:
  -h|--help         Show help information
  --default-string  The default string used when a tag fails to produce a valid string
  -d|--dry-run      Do a dry-run where the program does not actually rename any files.
  -r|--recursive    Recursively access files under given directories.
  --skip-null       Skip renaming a file if a tag fails to produce a valid string.
```

View the [wiki](https://github.com/alisterpineda/MyMediaRenamer/wiki) for more details.

## Screenshots

_Note: The following screenshots show outdated file name pattern formats. Please see the wiki for details._

### Console Application in .NET Core

#### Windows (Powershell)

![Powershell Example](/docs/powershell_example.png)

#### Linux (Bash on Kubuntu 18.04)

![Linux Example](/docs/bash_example.png)

### Windows GUI Application (WPF)

![WPF Example](/docs/wpf_example.gif)

![Metadata Viewer Example](/docs/metadata_viewer_example.png)