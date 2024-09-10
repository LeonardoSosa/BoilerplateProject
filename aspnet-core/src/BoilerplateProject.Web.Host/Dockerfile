FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/BoilerplateProject.Web.Host/BoilerplateProject.Web.Host.csproj", "src/BoilerplateProject.Web.Host/"]
COPY ["src/BoilerplateProject.Web.Core/BoilerplateProject.Web.Core.csproj", "src/BoilerplateProject.Web.Core/"]
COPY ["src/BoilerplateProject.Application/BoilerplateProject.Application.csproj", "src/BoilerplateProject.Application/"]
COPY ["src/BoilerplateProject.Core/BoilerplateProject.Core.csproj", "src/BoilerplateProject.Core/"]
COPY ["src/BoilerplateProject.EntityFrameworkCore/BoilerplateProject.EntityFrameworkCore.csproj", "src/BoilerplateProject.EntityFrameworkCore/"]
WORKDIR "/src/src/BoilerplateProject.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/BoilerplateProject.Web.Host", "src/BoilerplateProject.Web.Host"]
COPY ["src/BoilerplateProject.Web.Core", "src/BoilerplateProject.Web.Core"]
COPY ["src/BoilerplateProject.Application", "src/BoilerplateProject.Application"]
COPY ["src/BoilerplateProject.Core", "src/BoilerplateProject.Core"]
COPY ["src/BoilerplateProject.EntityFrameworkCore", "src/BoilerplateProject.EntityFrameworkCore"]
WORKDIR "/src/src/BoilerplateProject.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "BoilerplateProject.Web.Host.dll"]
