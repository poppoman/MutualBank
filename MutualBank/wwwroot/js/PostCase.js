//Vue初始化表單選項
vmPostCase = new Vue({
    el: "#postCase",
    data: {
        toastSubmitResult:"新增成功！",
        areaCity: [],
        areaTown: [],
        isAreaSelected: true,
        skillTags: [],
        userPoint: userPoint,
        title: "",
        intro: "",
        serDate: "",
        CaseSerArea: "請先選擇縣市",
        CaseNeedHelp: -1,
        CaseSkilId: -1,
        point:0
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
                url: "/Nav/_LayoutApi/GetAreaTownWithId",
                type: "GET",
                data: {
                    AreaCity: SelectedCity
                }
            }).
                done(function (res) {
                    vmPostCase.isAreaSelected = false;
                    vmPostCase.areaTown = JSON.parse(res);
                    setTimeout(() => {
                        var CaseSerArea = document.getElementById("CaseSerArea");
                        CaseSerArea.options[1].selected = true;
                        vmPostCase.CaseSerArea = parseInt(CaseSerArea.options[1].value);
                    }, 100);
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
        },
        isNumber: function (e) {
            e = (e) ? e : window.event;
            var charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                e.preventDefault();;
            } else {
                return true;
            }
        },
        updateDateRemind: function () {
            var dateRemind = document.getElementById("dateRemind");
            dateRemind.innerText = this.finalDate(dateApp.value, 14);
        },
        disableDate: function () {
            var dateApp = document.getElementById("dateApp");

            dateApp.setAttribute('disabled', 'disabled');
            dateApp.value = getDateString(new Date());//時間恢復為當天
            dateRemind.innerText = this.FinalDate(new Date(), 14);//預告下架日期
        },
        ableDate: function () {
            var dateApp = document.getElementById("dateApp");

            dateApp.removeAttribute('disabled');
        },
        previewPic: function (e) {
            maxSize_2MB = 1 * 1024 * 1024;

            if (e.files[0].size > maxSize_2MB) {
                alert('圖片大小超過2MB！無法上傳');
                e.value = "";
            }
            else {
                pic.src = URL.createObjectURL(e.files[0])
            }
        },
        getDateString: function (date) {
            let monthJs = date.getMonth();
            let monthMd = (monthJs + 1) < 10 ? `0${monthJs + 1}` : monthJs + 1 > 12 ? monthJs : monthJs + 1;
            let dateJs = date.getDate();
            let dateMd = dateJs < 10 ? `0${dateJs}` : dateJs;
            return `${date.getFullYear()}-${monthMd}-${dateMd}`;
        },
        finalDate: function (dateBegin, intCounts) {
            let selectedTimeStamp = new Date(dateBegin).getTime();
            let resultTimeStamp = selectedTimeStamp + intCounts * 1000 * 60 * 60 * 24;
            let finalDay = new Date(resultTimeStamp);
            return vmPostCase.getDateString(finalDay);
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
                 && this.CaseNeedHelp != -1 &&this.CaseSkilId!=-1)
            {
                return  false;
            }
            return true;
        },
        pointValid: function () {
            return this.point > this.userPoint ? true : false;
        }
    },
    mounted: function () {
        this.getAreaCity();
        this.getSkillTags();

        //init:get default date
        this.$nextTick(() => {
            var currentDate = new Date();
            var dateApp = document.getElementById("dateApp");
            var dateRemind = document.getElementById("dateRemind");
            dateApp.min = dateApp.value =  this.getDateString(currentDate);
            dateApp.max = this.finalDate(currentDate, 14); 
            dateRemind.innerText = dateApp.max;
            dateApp.setAttribute('disabled', 'disabled');
        });
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
        $("#msgSuccessSubmit").html("<div id='successAdd'><div class='text-center m-2'><i class='mb-3 fa-solid fa-paper-plane' style='font-size: 50px;'></i> <h4 class='fw-500 mb-0'>新增成功</h4> <p class='fw-300 fs-14'>可至「我的技能與需求」查看</p><a class='btn btn-primary m-1 text-white' href='/Home/ProfilePageAjax?page=myCase'>查看</a></div></div>");
        window.scrollTo(0, 0);
    })
        .fail(function (res) {
            console.log(res);
            vmPostCase.toastSubmitResult = "Oops...新增失敗";
            var toastTransResult = document.getElementById('ToastTransResult')
            var toast = new bootstrap.Toast(toastTransResult);
            toast.show();
        });
    return false;
});






