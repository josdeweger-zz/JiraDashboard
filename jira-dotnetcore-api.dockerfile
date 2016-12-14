FROM microsoft/dotnet

WORKDIR /var/www/jira-dotnetcore-api

COPY ./Jira.Api .

ENV ASPNETCORE_JIRA_REST_API=https://jira.redhotminute.com
ENV ASPNETCORE_JIRA_USERNAME=jos.deweger
ENV ASPNETCORE_JIRA_PASSWORD=SamuraiPizzaCat1983!
ENV ASPNETCORE_JIRA_AUTHENTICATION_RESOURCE=rest/auth/latest/session
ENV ASPNETCORE_JIRA_WORKLOG_RESOURCE=rest/tempo-timesheets/latest/worklogs
ENV ASPNETCORE_FAULTMARGIN_PERCENTAGE=50

RUN dotnet restore

ENTRYPOINT cd /var/www/jira-dotnetcore-api/Jira.Api && dotnet build && dotnet run

EXPOSE 3002