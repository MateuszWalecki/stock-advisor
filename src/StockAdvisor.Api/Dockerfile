FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /dotnetapp
COPY ../bin/Docker .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT ["dotnet", "StockAdvisor.Api.dll"]