# Must be in Publish folder after running "dotnet publish -c Release -o ./publish" command. After we can send it to Azure File Share, we can build the container image using this Dockerfile.
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet","copyapp.dll"]