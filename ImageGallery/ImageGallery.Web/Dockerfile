#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ImageGallery.Web/ImageGallery.Web.csproj", "ImageGallery.Web/"]
COPY ["ImageGallery.Core/ImageGallery.Core.csproj", "ImageGallery.Core/"]
RUN dotnet restore "ImageGallery.Web/ImageGallery.Web.csproj"
COPY . .
WORKDIR "/src/ImageGallery.Web"
RUN dotnet build "ImageGallery.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImageGallery.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageGallery.Web.dll"]