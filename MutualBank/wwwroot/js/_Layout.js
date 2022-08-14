var vmNav = new Vue({
    el: "#navBar",
    data: {
        areaCity: [],
        areaTown: [],
        selectedCity: "default",
        selectedTown: "default",
        userPoint: 0,
        areaText:"區域"
    },
    methods: {
        getAreaCity: function () {
            $.ajax({
                url: "/Nav/_LayoutApi/GetAreaCity",
                type: "GET"
            }).
                done(function (res) {
                    vmNav.areaCity = res;
                })
                .fail(function (res) {
                    console.log(res);
                });
        },
        getAreaTown: function (e) {
            var SelectedCity = "";
            //如果是"保留上次搜尋區域文字"的流程在使用此方法，會直接傳入字串當作e
            if (typeof (e) == "object") {
                SelectedCity = e.target.value;
            }
            else {
                SelectedCity = e;
            }
            if (SelectedCity != "default") {
                $.ajax({
                    url: "/Nav/_LayoutApi/GetAreaTown",
                    type: "GET",
                    data: {
                        AreaCity: SelectedCity
                    }
                }).
                    done(function (res) {
                        vmNav.areaTown = res;
                        //default town
                        if (typeof (e) == "object") {
                            vmNav.selectedTown = "default";
                            vmNav.areaText = "全部";
                        }
                    })
                    .fail(function (res) {
                        console.log(res);
                    });
            }
            else {
                vmNav.areaTown = [];
                vmNav.areaText = "區域";
                this.selectedTown = "default";
            }
        },
        getUserpoint: function () {
            fetch("/Nav/_LayoutApi/GetUserPoint")
                .then(res =>
                    res.json()
                )
                .then(data => {
                    vmNav.userPoint = data;
                })
                .catch(x =>
                    console.log(x));
        }
    },
    created: function () {
        this.getAreaCity();
        this.getUserpoint();
    }
});


//-RWD Manu控制
let panelsToggle = document.querySelector('.panelsToggle');

let barManu = document.querySelector('.topR .barManu');
let iconManu = document.querySelector('.navBar .topR .iconMaun');
iconManu.addEventListener('click', function () {
    barManu.classList.toggle('active');
    //點選時切換icon與背景黑幕
    if (iconManu.classList.contains("fa-xmark")) {
        panelsToggle.classList.remove('bgOn');
        panelsToggle.classList.remove('On');
        iconManu.classList.remove('fa-xmark');
        iconManu.classList.add('fa-bars');
    }
    else  {
        panelsToggle.classList.add('bgOn');
        panelsToggle.classList.add('On');
        iconManu.classList.add('fa-xmark');
        iconManu.classList.remove('fa-bars');
    }
})


//-點擊背景時關閉所有面板
panelsToggle.addEventListener('click', function () {
    if (barManu.classList.contains('active')) {
        //關閉barManu，並將X恢復為原 menu icon
        barManu.classList.remove('active');
        iconManu.classList.add('fa-bars');
        iconManu.classList.remove('fa-xmark');
    }
    barManu.classList.remove('active');
    panelsToggle.classList.remove('On');
    panelsToggle.classList.remove('bgOn');

    
})