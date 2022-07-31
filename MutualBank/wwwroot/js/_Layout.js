﻿var vmNav = new Vue({
    el: "#navBar",
    data: {
        areaCity: [],
        areaTown: [],
        isDefaultShowing: true,
        isCitySelected: true,
        selectedCity: '',
        selectedTown: '',
        userPoint: 0
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
            if (typeof (e) == "object") {
                SelectedCity = e.target.value;
            }
            else {
                SelectedCity = e;
            }
            $.ajax({
                url: "/Nav/_LayoutApi/GetAreaTown",
                type: "GET",
                data: {
                    AreaCity: SelectedCity
                }
            }).
                done(function (res) {
                    vmNav.isDefaultShowing = false;
                    vmNav.areaTown = res;
                    //區域資料預設為顯示第一筆
                    if (typeof (e) == "object") {
                        vmNav.selectedTown = res[0];
                        vmNav.isCitySelected = false;
                    }
                })
                .fail(function (res) {
                    console.log(res);
                });
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

let barManu = document.querySelector('.topR .barManu ');
let iconManu = document.querySelector('.navBar .topR .iconMaun');
iconManu.addEventListener('click', function () {
    barManu.classList.toggle('active');
    //點選時切換icon
    iconManu.classList.toggle('fa-bars');
    iconManu.classList.toggle('fa-xmark');
    //開啟背景關閉功能
    panelsToggle.classList.add('bgOn');
})


//-點擊背景時關閉面板
panelsToggle.addEventListener('click', function () {
    panelcity.classList.remove('active');
    panelTown.classList.remove('active');
    barManu.classList.remove('active');
    panelsToggle.classList.remove('On');
    panelsToggle.classList.remove('bgOn');
    if (barManu.classList.contains('active')) {
        //關閉barManu，並將X恢復為原icon
        barManu.classList.remove('active');
        iconManu.classList.toggle('fa-bars');
        iconManu.classList.toggle('fa-xmark');
    }
})