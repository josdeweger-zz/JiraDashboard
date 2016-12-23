FROM node:7.2.1

ENV REACT_APP_TEAM_NAME "El Mango"
ENV REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL "http://rhm-d-dock01.boolhosting.tld:3002/customer/status"
ENV REACT_APP_JIRA_API_CUSTOMER_STATUS_URL "http://rhm-d-dock01.boolhosting.tld:3001/settings"

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest
RUN mkdir -p /var/log/pm2

COPY . .

RUN cd /var/www/jira-react-app && ls

RUN npm install
RUN chmod a+x /var/www/jira-react-app/node_modules/.bin/react-scripts
RUN npm run build

ENTRYPOINT pm2 start pm2.json --env development --no-daemon

EXPOSE 3000
