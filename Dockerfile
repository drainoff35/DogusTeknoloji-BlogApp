FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DogusTeknoloji-BlogApp/DogusTeknoloji-BlogApp.csproj", "DogusTeknoloji-BlogApp/"]
COPY ["DogusTeknoloji-BlogApp.Core/DogusTeknoloji-BlogApp.Core.csproj", "DogusTeknoloji-BlogApp.Core/"]
COPY ["DogusTeknoloji-BlogApp.Infrastructure/DogusTeknoloji-BlogApp.Infrastructure.csproj", "DogusTeknoloji-BlogApp.Infrastructure/"]
COPY ["DogusTeknoloji-BlogApp.Services/DogusTeknoloji-BlogApp.Services.csproj", "DogusTeknoloji-BlogApp.Services/"]
RUN dotnet restore "DogusTeknoloji-BlogApp/DogusTeknoloji-BlogApp.csproj"
COPY . .
WORKDIR "/src/DogusTeknoloji-BlogApp"
RUN dotnet build "DogusTeknoloji-BlogApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DogusTeknoloji-BlogApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DogusTeknoloji-BlogApp.dll"]
