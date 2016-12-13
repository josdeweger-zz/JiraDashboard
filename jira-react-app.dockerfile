FROM node:7.2.1

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest 
RUN npm install express path

RUN mkdir -p /var/log/pm2

COPY ./Jira.ReactApp/server.js .
COPY ./Jira.ReactApp/pm2.json .

RUN npm run build

ENTRYPOINT pm2 start pm2.json --env development --no-daemon

EXPOSE 3001
