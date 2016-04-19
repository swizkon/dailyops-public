DailyOps = window['DailyOps'] || {};

DailyOps.users = (function () {

    return {
        current: function () {

            var localUserRaw = localStorage.getItem("user") || "{}";
            var localUser = JSON.parse(localUserRaw);
            return localUser;
        },
        /*
        register: function (username, pwd) {

            // Do some hashing....
            localStorage.setItem("user", username);
            this.login(username, pwd);
            return true;
        },
        */
        localLogin: function (username, password) {

            var userObj = {
                "provider": "Local",
                "displayName": username + "(local)",
                "userId": username,
                "image": "http://scribblebox.com/img/anon.png",
                "userKey": CryptoJS.SHA256(username).toString()
            };

            // Do some hashing....
            localStorage.setItem("user", JSON.stringify(userObj));
            return userObj;
        },
        localSignOut: function (username) {
            localStorage.removeItem("user");
            return { "then": function (callback) {
                callback();
            } 
            };
        }
    };
})();

