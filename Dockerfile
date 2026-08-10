# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AncinInsaat/AncinInsaat.csproj AncinInsaat/
RUN dotnet restore AncinInsaat/AncinInsaat.csproj

COPY AncinInsaat/ AncinInsaat/
RUN dotnet publish AncinInsaat/AncinInsaat.csproj -c Release -o /app/publish --no-restore

# App_Data isn't wwwroot and has no <Content> item in the csproj, so
# `dotnet publish` does not copy it — the committed SQLite database has to
# be placed into the publish output explicitly.
RUN mkdir -p /app/publish/App_Data \
    && cp AncinInsaat/App_Data/ancinInsaat.db /app/publish/App_Data/ancinInsaat.db


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render terminates TLS at its edge and forwards plain HTTP to the
# container; without this, Program.cs's UseHttpsRedirection()/UseHsts()
# would see every request as HTTP and redirect it back to HTTPS forever.
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

COPY --from=build --chown=$APP_UID:$APP_UID /app/publish .

USER $APP_UID

EXPOSE 8080

# Render assigns the listen port at container start via $PORT, which isn't
# known at build time, so ASPNETCORE_HTTP_PORTS is resolved here instead of
# via a Dockerfile ENV.
ENTRYPOINT ["/bin/sh", "-c", "ASPNETCORE_HTTP_PORTS=${PORT:-8080} exec dotnet AncinInsaat.dll"]
