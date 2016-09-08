var Hello = React.createClass({
    getInitialState: function() {
        return {
            "createUserModel": {},
            "meta": {},
            "data": {
                "activeUsers": [],
                "deactivatedUsers": []
            },
            "processInfo": {
                "label": "Loading",
                "description": "Connecting to database..."
            }
        };
    },

    loadCommentStateFromServer: function() {
        var self = this;
        $.ajax({
            url: "http://localhost:2113/projection/activeusers",
            dataType: 'json',
            cache: false,
            success: function(data) {
                console.log(data);
                var currentState = self.state;
                currentState["projectionStatus"] = data;
                self.state = currentState;
                self.loadCommentsFromServer();
            },//.bind(this),
            error: function(xhr, status, err) {
            },//.bind(this)
        });
					
    },
				
    loadCommentsFromServer: function() {
        var self = this;
        $.ajax({
            url: this.props.url,
            dataType: 'json',
            cache: false,
            success: function(data) {
                var currentState = self.state;
                currentState["data"] = data;
                self.setState(currentState);
            }, //.bind(this),
            error: function(xhr, status, err) {
                console.error(self.props.url, status, err.toString());
            }//.bind(this)
        });
					
    },
				  
    componentDidMount: function() {
        this.loadCommentStateFromServer();
    },
				  
    componentDidUpdate: function() {
        var btns = $("a.fakeable");
        var randompos = Math.floor(Math.random()*btns.length);
        var randombtn = btns[randompos];
					
        var f = function() {
            randombtn.click();
        };
        // setTimeout(f, 100);
    },

				  	
				  
    createuserUsernameChanged: function(e) {
					
        var createUserModel = this.state["createUserModel"];
        createUserModel["username"] = e.target.value;
					
        e.preventDefault();
    },
				  
    createuserDisplaynameChanged: function(e) {
					
        var createUserModel = this.state["createUserModel"];
        createUserModel["displayName"] = e.target.value;
        e.preventDefault();
    },
					
					
				  
    createUser: function(e) {
					
        var createUserModel = this.state["createUserModel"];
        var username = createUserModel.username;
        var displayName = createUserModel.displayName;
        e.preventDefault();

        createUserModel = {};
        DailyOps.events.userCreated(username, displayName).then(this.loadCommentStateFromServer);
    },
				  
    deactiveUser: function(e) {

        var username = e.target.attributes.getNamedItem("data-username").value;

        var currentState = this.state;

        var currentUsers = $.map(currentState.data.activeUsers, function(a,b){
            return a;
        });

        var freshUsers = currentUsers.filter(function(user){
            return user.username.toString() != e.toString();
        });
					
        currentState["data"]["activeUsers"] = freshUsers;
        this.setState(currentState);
        e.preventDefault();
					
        DailyOps.events.userDeactivated(username).then(this.loadCommentStateFromServer);
    },
				  
    reactivateUser: function(e) {
        var username = e.target.attributes.getNamedItem("data-username").value;
        e.preventDefault();
        DailyOps.events.userReactivated(username).then(this.loadCommentStateFromServer);
    },

				  
    deleteUser: function(e) {
        var username = e.target.attributes.getNamedItem("data-username").value;
        e.preventDefault();
        DailyOps.events.userDeleted(username).then(this.loadCommentStateFromServer);
    },
				  
    render: function() {
        var activeUsers = $.map(this.state.data.activeUsers, function(a,b){
            return a;
        });

        var deactivatedUsers = $.map(this.state.data.deactivatedUsers, function(a,b){
            return a;
        });
	  
        var projectionStatus = JSON.stringify(this.state.projectionStatus);
	  
        return (
          <div className="row">
          <div className="col-sm-4">
              <h3>Create users</h3>
              <form onSubmit={this.createUser}>
              <fieldset>
                  <legend>Da form</legend>
                  <input placeholder="User name" type="text" name="username" id="createuserUsername"  onChange={this.createuserUsernameChanged}/>
                  <input placeholder="Display name" type="text" name="displayname" id="createuserDisplayName" onChange={this.createuserDisplaynameChanged} />
			  
                  <button type="submit" className="btn btn-default btn-lg">Create user</button>
              </fieldset>
            </form>
              <hr />
              <h2>Projection status</h2>
              <pre><code>{projectionStatus}</code></pre>
          </div>
          <div className="col-sm-4">
              <h3>Active users</h3>
        {activeUsers.map(function(item, i) {
            // console.log(item);
            var k = CryptoJS.SHA256(item.username).toString();
            return (
              <div key={k}>
				<h4>{item.displayName} <small>({item.username})</small></h4>
				<a className="btn btn-default btn-xs fakeable" data-username={item.username} href="#" onClick={this.deactiveUser}>deactivate</a>
			</div>
          );
        }, this)}
        </div>
        <div className="col-sm-3">
		  <h5>Deactived users</h5>
            {deactivatedUsers.map(function(item, i) {
                return (
                    <DeactivatedUser reactivateUser={this.reactivateUser} deleteUser={this.deleteUser} username={item.username} displayName={item.displayName} />

              );
            }, this)}
        </div>
</div>
    );
}
});

    var DeactivatedUser = React.createClass({
        
        reactivateUser: function(e) {
            if (typeof this.props.reactivateUser === 'function') {
                this.props.reactivateUser(e);
            }
        },
        
        deleteUser: function(e) {
            if (typeof this.props.deleteUser === 'function') {
                this.props.deleteUser(e);
            }
        },

        render: function() {
            var displayName = this.props.displayName;
            var username = this.props.username;
            var k = CryptoJS.SHA256(username).toString();

            return (
              <div key={k}>
                <h4>DeactivatedUser {displayName} <small>({username})</small></h4>
                <a className="btn btn-default btn-xs fakeable" data-username={username} href="#" onClick={this.reactivateUser}>Re-activate</a>
                <a className="btn btn-default btn-xs" data-username={username} href="#" onClick={this.deleteUser}>Delete</a>
            </div>
          );
    }
    });

 
ReactDOM.render(
    <Hello name="World" url="http://localhost:2113/projection/activeusers/state" pollInterval={1000} />,
			    document.getElementById('container')
			);