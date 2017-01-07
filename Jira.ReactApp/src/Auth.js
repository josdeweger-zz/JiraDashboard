
import cookie from 'react-cookie';
import 'whatwg-fetch';

const authCookieName = 'JSessionId';
const authUrl = process.env.REACT_APP_JIRA_API_URL + '/authentication';

var Auth = {
    self: this,

    requireAuth: function(nextState, replace) {
        var sessionId = cookie.load(authCookieName);
        console.log(sessionId);

        if (!sessionId) {
            replace({ pathname: '/login' });
        }
    },

    getSessionCookie: function() {
        return cookie.load(authCookieName);
    },

    authenticate: function(e, formData) {
        console.log('authenticate');
        e.preventDefault();

        return fetch(authUrl, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: formData.username,
                password: formData.password
            })
        })
        .then(function(response) {
            console.log('status', response.status);

            return response.json().then(function(jsonResponse) {
                console.log('jsonResponse', jsonResponse);
                var loginData = {};
                loginData.data = jsonResponse;

                if (response.status === 200) {
                    cookie.save(authCookieName, jsonResponse.session);
                    loginData.loggedIn = true;
                    loginData.message = 'Successfully logged in';
                } else if (response.status === 401) {
                    loginData.loggedIn = false;
                    loginData.message = 'Wrong username or password';
                } else if (response.status === 403) {
                    loginData.loggedIn = true;
                    loginData.message = 'Login denied. You probably need to enter a captcha on jira.redhotminute.com';
                } else {
                    loginData.loggedIn = true;
                    loginData.message ='Something went horribly wrong!';
                }

                return loginData;
            });
        })
        .catch(function (error) {
            return {loggedIn: false, message: 'Something went horribly wrong!', data: error};
        })
    }
}

export default Auth;