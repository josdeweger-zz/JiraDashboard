FROM node:7.2.1

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest
RUN mkdir -p /var/log/pm2

ARG REACT_APP_TEAM_NAME
ARG REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL
ARG REACT_APP_JIRA_API_CUSTOMER_STATUS_URL

ENV REACT_APP_TEAM_NAME ${REACT_APP_TEAM_NAME}
ENV REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL ${REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL}
ENV REACT_APP_JIRA_API_CUSTOMER_STATUS_URL ${REACT_APP_JIRA_API_CUSTOMER_STATUS_URL}

WORKDIR /var/www/jira-react-app

COPY . .

RUN npm install 
RUN npm run build

ENTRYPOINT pm2 start pm2.json --env development --no-daemon

EXPOSE 3000
