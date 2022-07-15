//初始化Area資料
var vm = new Vue({
    data: {
        areaCity: [],
        areaTown: [],
        isAreaSelected: true
    },
    methods: {
        getAreaCity: function () {
            $.ajax({
                url: "Nav/_LayoutApi/GetAreaCity",
                type: "GET"
            }).
                done(function (res) {
                    vm.areaCity = res;
                })
                .fail(function (res) {
                    console.log(res);
                });
        },
        getAreaTown: function (e) {
            var SelectedCity = e.target.innerText;
            cityName.innerText = SelectedCity;
            $.ajax({
                url: "Nav/_LayoutApi/GetAreaTown",
                type: "GET",
                data: {
                    AreaCity: SelectedCity
                }
            }).
                done(function (res) {
                    vm.isAreaSelected = false;
                    vm.areaTown = res;
                })
                .fail(function (res) {
                    console.log(res);
                });
        },
        updateTownName: function (e) {
            var SelectedTown = e.target.innerText;
            townName.innerText = SelectedTown;
        }
    },
    mounted: function () {
        //初始化縣市
        this.getAreaCity();
    }
});
vm.$mount("#navBar");


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


//-postQuery 點選+號後顯示面板
let btnPost = document.querySelector('.topR .barManu li:first-child');
let panelPost = document.querySelector('.postLink');
btnPost.addEventListener('click', function () {
    panelPost.classList.toggle('active');
    //開啟背景的點擊關閉功能
    panelsToggle.classList.add('bgOn');
    //同時關閉barManu，並將X恢復為原icon
    barManu.classList.remove('active');
    iconManu.classList.toggle('fa-bars');
    iconManu.classList.toggle('fa-xmark');

})
let iconPost = document.querySelector('.wrap-panel .iconPost');
iconPost.addEventListener('click', function () {
    panelsToggle.classList.remove('On');
    panelsToggle.classList.remove('bgOn');
    panelPost.classList.remove('active');

});

//-點擊背景時關閉面板
panelsToggle.addEventListener('click', function () {
    panelcity.classList.remove('active');
    panelTown.classList.remove('active');
    panelPost.classList.remove('active');
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