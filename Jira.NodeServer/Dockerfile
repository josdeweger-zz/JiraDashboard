FROM node:argon
RUN mkdir /app
WORKDIR /app
COPY package.json /app

COPY . /app
RUN ["npm", "install"]
RUN ["chmod", "-R", "+x", "/app"]

EXPOSE 3001
CMD ["babel-node", "server.js"]