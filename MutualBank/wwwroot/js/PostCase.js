//Vue初始化表單選項
vmPostCase = new Vue({
    el: "#postCase",
    data: {
        areaCity: [],
        areaTown: [],
        isAreaSelected: true,
        skillTags: [],
        title: "",
        intro: "",
        serDate: "",
        CaseSerArea: -1,
        CaseNeedHelp: -1,
        CaseSkilId:-1
    },
    methods: {
        getAreaCity: function () {
            $.ajax({
                url: "/Nav/_LayoutApi/GetAreaCity",
                type: "GET"
            }).
                done(function (res) {
                    vmPostCase.areaCity = res;
                })
                .fail(function (res) {
                    console.log(res);
                });
        },
        getAreaTown: function (e) {
            var SelectedCity = e.target.value;
            $.ajax({
                url: "/Nav/_LayoutApi/GetAreaTown",
                type: "GET",
                data: {
                    AreaCity: SelectedCity
                }
            }).
                done(function (res) {
                    vmPostCase.isAreaSelected = false;
                    vmPostCase.areaTown = res;
                })
                .fail(function (res) {
                    console.log(res);
                });
        },
        getSkillTags: function () {
            $.ajax({
                url: "/Case/GetSkillTags",
                type: "POST",
            }).done(function (res) {
                vmPostCase.skillTags = res;
            })
                .fail(function (res) {
                    console.log(res);
                });
        }


    },
    computed: {
        titleValid: function () {
            return this.title.length > 0 ? false : true;
        },
        introValid: function () {
            return this.intro.length > 0 ? false : true;
        },
        serDateValid: function () {
            return this.serDate.length > 0 ? false : true;
        },
        isSubmitAble: function () {
            if (this.title.length > 0 && this.intro.length > 0 && this.serDate.length > 0
                && this.CaseSerArea != -1 && this.CaseNeedHelp != -1 &&this.CaseSkilId!=-1)
            {
                return  false;
            }
            return true;
        }
    },
    mounted: function () {
        this.getAreaCity();
        this.getSkillTags();
    }
});




//傳送Case表單
$("#caseForm").submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    $.ajax({
        url: "/Case/AddCase",
        type: "POST",
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        enctype: 'multipart/form-data',
        processData: false,
    }).done(function (res) {
            $("#caseForm").html("<div id='successAdd'><h6>成功送出！</h6></div>");
            $("#successAdd").append("<p>已新增貼文</p>").hide().fadeIn(500);
        })
        .fail(function (res) {
            console.log(res);
        });;

    return false;

});







//toggle 關閉選擇預約時開
function disableDate() {
    dateApp.setAttribute('disabled', 'disabled');
    dateApp.value = getDateString(currentDate);//時間恢復為當天
    dateRemind.innerText = FinalDate(new Date(), 14);//預告下假日期
}
//toggle 開放選擇預約時開
function ableDate() {
    dateApp.removeAttribute('disabled');
}
//選擇日期後更新提醒時間
function updateDateRemind() {
    dateRemind.innerText = FinalDate(dateApp.value, 14);
}
//預覽上傳圖片
function previewPic(e) {
    maxSize_2MB = 1 * 1024 * 1024;

    if (e.files[0].size > maxSize_2MB) {
        alert('圖片大小超過2MB！無法上傳');
        e.value = "";
    }
    else {
        pic.src = URL.createObjectURL(e.files[0])

    }
}