FROM microsoft/dotnet:1.0.1-runtime

WORKDIR /var/www/jira-dotnetcore-api

COPY Jira.Api/Jira.Api/out .

ENTRYPOINT ["dotnet", "Jira.Api.dll"]

EXPOSE 3002

# To build:
# docker build -f jira-dotnetcore-api.dockerfile --tag jira-dotnetcore-api ../

# To run:
# docker run -p 3002:3002 --name jira-dotnetcore-api jira-dotnetcore-api
# docker run -d -p 3002:3002 --name jira-dotnetcore-api jira-dotnetcore-api

