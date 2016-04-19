
var Hello = React.createClass({
    getInitialState: function() {
        return {
            "createUserModel": {},
            "meta":{},
            "data": {
                "activeUsers": [],
                "deactivatedUsers": []
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
                var currentState = this.state;
                currentState["projectionStatus"] = data;
                this.state = currentState;
                self.loadCommentsFromServer();
            }.bind(this),
            error: function(xhr, status, err) {
            }.bind(this)
        });
					
    },
				
    loadCommentsFromServer: function() {

        $.ajax({
            url: this.props.url,
            dataType: 'json',
            cache: false,
            success: function(data) {
                var currentState = this.state;
                currentState["data"] = data;
                this.setState(currentState);
            }.bind(this),
            error: function(xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
					
    },
				  
    componentDidMount: function() {
        // this.loadCommentsFromServer();
        this.loadCommentStateFromServer();
        // setInterval(this.loadCommentStateFromServer, this.props.pollInterval);
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
					
        var currentState = this.state;

        var currentUsers = $.map(currentState.data.activeUsers, function(a,b){
            return a;
        });
        console.log(currentUsers.length);
        var freshUsers = currentUsers.filter(function(user){
            return user.username.toString() != e.toString();
        });
        console.log(freshUsers.length);
					
        currentState["data"]["activeUsers"] = freshUsers;
        this.setState(currentState);
						
        console.log(arguments);
        arguments[1].preventDefault();
					
        DailyOps.events.userDeactivated(arguments[0]).then(this.loadCommentStateFromServer);
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
            // console.log(a);
            return a;
        });

        var deactivatedUsers = $.map(this.state.data.deactivatedUsers, function(a,b){
            // console.log(a);
            return a;
        });
	  
        var projectionStatus = JSON.stringify(this.state.projectionStatus);
	  
        return (
          <div className="row">
          <div className="col-sm-4">
              <h3>Create users</h3>
              <form onSubmit={this.createUser.bind(this)}>
              <fieldset>
                  <legend>Da form</legend>
                  <input placeholder="User name" type="text" name="username" id="createuserUsername"  onChange={this.createuserUsernameChanged.bind(this)}/>
                  <input placeholder="Display name" type="text" name="displayname" id="createuserDisplayName" onChange={this.createuserDisplaynameChanged.bind(this)} />
			  
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
				<a className="btn btn-default btn-xs fakeable" data-username="gagga" href="#" onClick={this.deactiveUser.bind(this, item.username)}>deactivate</a>
			</div>
          );
        }, this)}
        </div>
        <div className="col-sm-3">
<h5>Deactived users</h5>
{deactivatedUsers.map(function(item, i) {
    var k = CryptoJS.SHA256(item.username).toString();
    return (
      <div key={k}>
        <h4>{item.displayName} <small>({item.username})</small></h4>
        <a className="btn btn-default btn-xs fakeable" data-username={item.username} href="#" onClick={this.reactivateUser}>Re-activate</a>
        <a className="btn btn-default btn-xs" data-username={item.username} href="#" onClick={this.deleteUser}>Delete</a>
				
    </div>
  );
}, this)}
</div>
</div>
    );
}
});
 
ReactDOM.render(
    <Hello name="World" url="http://localhost:2113/projection/activeusers/state" pollInterval={1000} />,
			    document.getElementById('container')
			);
			