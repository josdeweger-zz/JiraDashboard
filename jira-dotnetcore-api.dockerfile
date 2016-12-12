FROM microsoft/dotnet:1.0.1-runtime

WORKDIR /var/www/jira-dotnetcore-api

COPY ./Jira.Api/Jira.Api/out .

ENTRYPOINT ["dotnet", "Jira.Api.dll"]

EXPOSE 3002

