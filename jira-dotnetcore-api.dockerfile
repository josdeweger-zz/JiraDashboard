FROM microsoft/dotnet

WORKDIR /var/www/jira-dotnetcore-api

COPY ./Jira.Api .

RUN dotnet restore

ENTRYPOINT cd /var/www/jira-dotnetcore-api/Jira.Api && dotnet build && dotnet run

EXPOSE 3002