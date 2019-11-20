FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /dotnetapp
RUN echo "elo"
RUN echo $(pwd)
RUN echo $(pwd)/bin/Docker
RUN ls $(pwd)/bin/Docker
RUN echo "po elo"
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT ["dotnet", "StockAdvisor.Api.dll"]