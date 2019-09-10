FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Automata theory/Automata theory.csproj", "Automata theory/"]
RUN dotnet restore "Automata theory/Automata theory.csproj"
COPY . .
WORKDIR "/src/Automata theory"
RUN dotnet build "Automata theory.csproj" -c Release -o /app


FROM build AS publish
RUN dotnet publish "Automata theory.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Automata theory.dll"]