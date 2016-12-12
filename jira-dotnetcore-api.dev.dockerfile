FROM microsoft/dotnet:1.1.0-sdk-projectjson

WORKDIR /var/www/jira-dotnetcore-api

EXPOSE 3002

ENTRYPOINT dotnet restore & cd Jira.Api & dotnet watch



# To build:
# docker build -f jira-dotnetcore-api.dev.dockerfile --tag dev-jira-dotnetcore-api .

# To run:
# docker run -p 3002:3002 --name dev-jira-dotnetcore-api -v //C//Users//dev//JiraClient//Jira.Api://var//www//jira-dotnetcore-api -w "/var/www/jira-dotnetcore-api" dev-jira-dotnetcore-api

