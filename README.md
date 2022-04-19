# Webpage Dumper

A program that downloads and dumps a provided webpage's files to disk in a threaded manner to improve performance.

The project architecture is a DDD (Domain Driven Design) one with dependency injection and inversion of control. It's has a somewhat "enterprise" separation of the different DDD layers to increase decoupling and separation of concern but results in a lot of boilerplate projects and interfaces that one could perhaps do without in a more simple and straight forward setup.

The resulting index.html downloaded from a webpage isn't manipulated by this tool so opening it in a webbrowser might not result in a working webpage. If you where to do the same with for instance Firefox then Firefox would manipulate all paths to point to the downloaded files (Firefox flattens the websites structure on the other hand removing the original folder structure that a webpage may have had had on the server).

There are only unit tests written for this project, no integration tests.

## Install Dotnet

### MacOS

```sh
brew install dotnet
```

### Arch Linux

With yay:
```sh
yay -S dotnet-targeting-pack dotnet-sdk dotnet-runtime dotnet-host aspnet-runtime
```

## Building

```sh
dotnet restore
dotnet build
```

## Dumping a webpage to disk

### Cli with dotnet command

```sh
dotnet run --project src/Application/WebpageDumper.Console -- --address=https://google.com --threads=8 --output=some_folder
```

Or simply:
```sh
dotnet run --project src/Application/WebpageDumper.Console -- -a https://google.com
```

For help:
```sh
dotnet run --project src/Application/WebpageDumper.Console -- --help
```

### MacOS/Linux

```sh
./src/Application/WebpageDumper.Console/bin/Debug/net6.0/WebpageDumper.Console --address=https://google.com --threads=8 -output=some_folder
```

Or simply:
```sh
./src/Application/WebpageDumper.Console/bin/Debug/net6.0/WebpageDumper.Console -a https://google.com
```

For help:
```sh
./src/Application/WebpageDumper.Console/bin/Debug/net6.0/WebpageDumper.Console --help
```
