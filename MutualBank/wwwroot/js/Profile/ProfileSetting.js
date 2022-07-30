var veUserDetails = new Vue({
    data: {
        user: []
    },
    created: function () {
        let self = this;
        fetch("/api/UsersController/getusers").then(function (result) {
            console.log("result =", result)
            return result.json();
        })
            .then(function (mydata) {
                self.user = mydata;
            })
    }
}).$mount('#userDetails')
