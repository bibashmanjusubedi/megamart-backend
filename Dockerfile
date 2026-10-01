# Stage 1: Build the application using .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["megamart-backend.csproj","./"]
RUN dotnet restore 


# Copy the rest of the source code and build
COPY . . 
RUN dotnet publish -c Release -o /app/out

# Stage 2: Run the application using lightweight .NET 10 ASP.NET runtime
FROM mcr.microsoft.com/dotnet/astnet:10 as final
COPY --from=build /app/out .

# Bind port dynamically to match Railway's expected environment variable
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS = http://+:${PORT:-8080}
ENTRYPOINT ["dotnet","megamart-backend.dll"]


