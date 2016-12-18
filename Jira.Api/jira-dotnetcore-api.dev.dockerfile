FROM microsoft/dotnet

WORKDIR /var/www/jira-dotnetcore-api

COPY . .

RUN dotnet restore

ENTRYPOINT cd /var/www/jira-dotnetcore-api/Jira.Api && dotnet build && dotnet watch run

EXPOSE 3002