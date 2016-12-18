Jira Dashboard provides teams with the possibility to keep track of spent hours versus reserved hours per project in a specific period (usually a sprint).

The application consists of three services:

* A client (React) for displaying data in charts
* A server (Node) for serving up and saving the project settings
* An API (dotnet core) for retrieving all the worklogs from the Jira api and calculating whether a project is on track or not
