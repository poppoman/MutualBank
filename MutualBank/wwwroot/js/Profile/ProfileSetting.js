var veUserDetails = new Vue({
    data: {
        user: []
    },
    created: function () {
        let self = this;
        fetch("/api/UsersController/GetUsers").then(function (result) {
            return result.json();
        })
            .then(function (mydata) {
                self.user = mydata;
            })
    }
}).$mount('#userDetails')
