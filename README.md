# CommonPDFServices

## start project
- new services command (create the project)
```bash
dotnet new webapi -n CommonPDFServices
```
## Package
```bash
dotnet add package IronPdf
```

```bash
dotnet add package DotNetEnv
```


# RUN BY DOT NET CLI

- Restore the project dependencies using the .NET CLI:
```
dotnet restore
```

- Build the project to ensure everything is correctly set up:


```
dotnet build
```

-Start the application using the .NET CLI:


```
dotnet run
```

# RUB BY DOCKER
```bash
docker build -t common-pdf-services .
```

```bash
docker run -d -p 8080:80 common-pdf-services
```