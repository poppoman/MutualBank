//Vue初始化表單選項
vmPostCase = new Vue({
    el: "#postCase",
    data: {
        areaCity: [],
        areaTown: [],
        isAreaSelected: true,
        skillTags: []
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
    mounted: function () {
        this.getAreaCity();
        this.getSkillTags();
    }
});

//表單送出Case
btnSubmit.addEventListener("click", addCase);
function addCase(e) {
    var dataString = $("#caseForm").serialize();
    $.ajax(
        {
            url: "/Case/AddCase",
            type: "POST",
            dataType: "text",
            data: dataString
        }
    )
        .done(function () {
            $("#caseForm").html("<div id='successAdd'><h6>成功送出！</h6></div>");
            $("#successAdd").append("<p>已新增貼文</p>").hide().fadeIn(500);
        })
        .fail(function (res) {
            console.log(res);
        });

}



//toggle 開放選擇預約時開
function disableDate() {
    dateApp.setAttribute('disabled', 'disabled');
    timeApp.setAttribute('disabled', 'disabled');
    dateRemind.innerText = FinalDate(new Date(), 14);
}
//toggle 開放選擇預約時開
function ableDate() {
    dateApp.removeAttribute('disabled');
    timeApp.removeAttribute('disabled');
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