# Webpage Dumper

A program that downloads and dumps a provided webpage files to disk in a threaded manner to improve performance.
The project architecture is a DDD one with dependency injection and inversion of control.

## Install

### MacOS

```sh
brew install dotnet
```

## Running

### Cli with dotnet command

```sh
dotnet run --project src/Application/WebpageDumper.Console -- --address=https://google.com --threads=8 --output=some_folder
```

Or simply:
```sh
dotnet run --project src/Application/WebpageDumper.Console -- -a https://google.com
```
### MacOS

```sh
./src/Application/WebpageDumper.Console/bin/Debug/net6.0/WebpageDumper.Console --address=https://google.com --threads=8 -output=some_folder
```

Or simply:
```sh
./src/Application/WebpageDumper.Console/bin/Debug/net6.0/WebpageDumper.Console -a https://google.com
```
