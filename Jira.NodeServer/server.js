import express from 'express';
import jsonfile from 'jsonfile';
import bodyParser from 'body-parser';

const jsonSettingsFilePath = "config.json";

const app = express();

app.set('port', (process.env.PORT || 3001));

app.use(function(req, res, next) {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE');
    res.header('Access-Control-Allow-Headers', 'Content-Type');

    next();
});

app.use(bodyParser.json());

app.get('/', function (req, res) {
  res.send('Hello World 2!');
});

app.get('/settings', (req, res) => {
    jsonfile.readFile(jsonSettingsFilePath, function(err, settings) {
        if(err) {
            console.log(err);
            var error = {
                status: 500,
                message: 'Could not get settings from ' + jsonSettingsFilePath + ' with error: ' + JSON.stringify(err)
            };
            next(error);
        } else {
            res.status(200).send(settings);
        }
    });
});

app.post('/settings', (req, res, next) => {
    jsonfile.writeFile(jsonSettingsFilePath, req.body, function (err) {
        if(err) {
            console.log(err);
            var error = {
                status: 500,
                message: 'Could not save settings to ' + jsonSettingsFilePath + ' with error: ' + JSON.stringify(err)
            };
            next(error);
        } else {
            res.status(200).send("Settings successfully saved");
        }
    });
});

app.use(function(err, req, res, next) {
    res.status(err.status || 500).send({
        message: err.message,
        error: err
    });
});

app.listen(app.get('port'), () => {
    console.log(`Find the server at: http://localhost:${app.get('port')}/`); // eslint-disable-line no-console
});