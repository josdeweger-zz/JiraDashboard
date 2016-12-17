FROM node:latest

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest

RUN mkdir -p /var/log/pm2

EXPOSE 3001

ENTRYPOINT pm2 start pm2.dev.json --env development --watch --no-daemon
