FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 3002

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["ik_word_management/ik_word_management.csproj", "ik_word_management/"]
RUN dotnet restore "ik_word_management/ik_word_management.csproj"
COPY . .
WORKDIR "/src/ik_word_management"
RUN dotnet build "ik_word_management.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ik_word_management.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ik_word_management.dll"]