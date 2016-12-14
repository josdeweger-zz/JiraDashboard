FROM node:7.2.1

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest

RUN mkdir -p /var/log/pm2

COPY ./Jira.ReactApp .

ENV REACT_APP_TEAM_NAME=ElMango
ENV REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL=http://localhost:3001/settings
ENV REACT_APP_JIRA_API_CUSTOMER_STATUS_URL=http://localhost:3002/customer/status

RUN npm install 
RUN npm run build

ENTRYPOINT pm2 start pm2.json --env development --no-daemon

EXPOSE 3000
