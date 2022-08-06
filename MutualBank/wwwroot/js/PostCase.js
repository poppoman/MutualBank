vmPostCase = new Vue({
    el: "#postCase",
    data: {
        areaCity: [],
        areaTown: [],
        isAreaSelected: true,
        skillTags: [],
        userPoint: userPoint,
        title: "",
        intro: "",
        CaseSerDate: "",
        CaseSerArea: "default",
        CaseNeedHelp: "default",
        CaseSkilId: "default",
        point: 0,
        titleValid: false,
        isneedValid: false,
        skillIdValid: false,
        pointValid: false,
        introValid: false,
        serAreaValid: false,
        serDateValid: false
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
            dateRemind.innerText = this.finalDate(CaseReleaseDate.value, 14);
        },
        disableDate: function () {
            var CaseReleaseDate = document.getElementById("CaseReleaseDate");
            CaseReleaseDate.setAttribute('disabled', 'disabled');
            CaseReleaseDate.value = getDateString(new Date());//時間恢復為當天
            dateRemind.innerText = this.FinalDate(new Date(), 14);//預告下架日期
        },
        ableDate: function () {
            var CaseReleaseDate = document.getElementById("CaseReleaseDate");
            CaseReleaseDate.removeAttribute('disabled');
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
        },
        scrollToElement: function (strElementId) {
            var elePos = $(`#${strElementId}`).offset().top;
            $(window).scrollTop(elePos - 200);
        },
        sumbitCase: function (e) {
            //valid
            var validFail = [];
            if (this.title == "") {
                this.titleValid = true;
                validFail.push("CaseTitle");
            }
            if (this.CaseNeedHelp == "default") {
                this.isneedValid = true;
                validFail.push("CaseNeedHelp");
            }
            if (this.CaseSkilId == "default") {
                this.skillIdValid = true;
                validFail.push("CaseSkilId");
            }
            if (this.point > this.userPoint) {
                this.pointValid = true;
                validFail.push("CasePoint");
            }
            if (this.intro=="" ) {
                this.introValid = true;
                validFail.push("CaseIntroduction");
            }
            if (this.CaseSerArea == "default") {
                this.serAreaValid = true;
                validFail.push("CaseSerArea");
            }
            if (this.CaseSerDate=="" ) {
                this.serDateValid = true;
                validFail.push("CaseSerDate");
            }
            if (validFail.length != 0) {
                this.scrollToElement(validFail[0]);
                return;
            }
            //submit
            e.preventDefault();
            var caseForm = $("form")[0];
            var formData = new FormData(caseForm);
            formData.append("CaseTitle", $("#CaseTitle").val());
            formData.append("CaseNeedHelp", $("#CaseNeedHelp").val());
            formData.append("CaseSkilId", $("#CaseSkilId").val());
            formData.append("CaseTitle", $("#CaseTitle").val());
            formData.append("CasePoint", $("#CasePoint").val());
            formData.append("CaseIntroduction", $("#CaseIntroduction").val());
            formData.append("CasePhoto", $("#CasePhoto").prop("files")[0]);
            formData.append("CaseSerArea", $("#CaseSerArea").val());
            formData.append("CaseSerDate", $("#CaseSerDate").val());
            formData.append("CaseReleaseDate", $("#CaseReleaseDate").val());
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
                    var toastTransResult = document.getElementById("ToastTransResult")
                    var toast = new bootstrap.Toast(toastTransResult);
                    toast.show();
                });
            return false;
        }
    },
    watch: {
        title: function (e) {
            if (e.length > 0) this.titleValid = false;
        },
        CaseNeedHelp: function (e) {
            if (e != "dafault") this.isneedValid = false;
        },
        CaseSkilId: function (e) {
            if (e != "dafault") this.skillIdValid = false;
        },
        point: function (e) {
            this.pointValid = e > this.userPoint ? true : false;
        },
        intro: function (e) {
            if (e.length > 0) this.introValid = false;
        },
        CaseSerArea: function (e) {
            if (e != "dafault") this.serAreaValid = false;
        },
        CaseSerDate: function (e) {
            if (e.length > 0) this.serDateValid = false;
        }
    },
    mounted: function () {
        this.getAreaCity();
        this.getSkillTags();
        //init:get default date
        this.$nextTick(() => {
            var currentDate = new Date();
            var CaseReleaseDate = document.getElementById("CaseReleaseDate");
            var dateRemind = document.getElementById("dateRemind");
            CaseReleaseDate.min = CaseReleaseDate.value = this.getDateString(currentDate);
            CaseReleaseDate.max = this.finalDate(currentDate, 14);
            dateRemind.innerText = CaseReleaseDate.max;
            CaseReleaseDate.setAttribute('disabled', 'disabled');
        });
    }
});

function previewPic(e) {
    var maxSize = 2 * 1024 * 1024;
    if (e.files[0].size > maxSize) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: '圖片大小請勿超過2MB',
            footer: ''
        });
        e.value = pic.src = "";
    }
    else {
        pic.src = URL.createObjectURL(e.files[0])
    }
}