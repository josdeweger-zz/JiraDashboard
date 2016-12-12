FROM node:latest

WORKDIR /var/www/jira-react-app

RUN npm install -g pm2@latest

RUN mkdir -p /var/log/pm2

EXPOSE 3001

ENTRYPOINT npm install & pm2 start pm2.json --env development --no-daemon

# To build:
# docker build -f jira-react-app.dockerfile --tag jira-react-app .

# To run:
# docker run -p 3000:3000 --name jira-react-app -v //c//users//dev//jiraclient//Jira.ReactApp://var//www//jira-react-app -w "/var/www/jira-react-app" jira-react-app
# docker run -d -p 3000:3000 --name jira-react-app jira-react-app 
