FROM node:latest

WORKDIR /var/www/jira-node-server

RUN npm install -g pm2@latest

RUN mkdir -p /var/log/pm2

EXPOSE 3001

ENTRYPOINT ["pm2", "start", "pm2.json", "--env", "development", "--no-daemon"]

# To build:
# docker build -f jira-node-server.dockerfile --tag jira-node-server .

# To run:
# docker run -p 3001:3001 --name jira-node-server -v \\$(PWD)//Jira.NodeServer:\\var\\www\\jira-node-server -w "/var/www/jira-node-server" jira-node-server
# docker run -d -p 3001:3001 --name jira-node-server jira-node-server 
