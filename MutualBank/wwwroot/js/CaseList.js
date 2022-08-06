var vmCaseList = new Vue({
    el: "#caseList",
    data: {
        caseDataModel: [],
        caseViewModel: [],
        sortByType: "null"
    },
    methods: {
        getAllModel: function (e) {
            vmCaseList.addClickClass(e);
            vmCaseList.caseViewModel = vmCaseList.caseDataModel;
        },
        needOrSkillModel: function (e) {
            vmCaseList.addClickClass(e);
            var bit = event.target.dataset.bit;
            if (bit == "true") {
                vmCaseList.caseViewModel = vmCaseList.caseDataModel.filter(x => x.CaseNeedHelp == true);
                vmCaseList.orderByReleaseDate(1);
            }
            else {
                vmCaseList.caseViewModel = vmCaseList.caseDataModel.filter(x => x.CaseNeedHelp == false);
                vmCaseList.orderByReleaseDate(1);
            }
        },
        orderByReleaseDate: function (intIsDesc) {
            return vmCaseList.caseDataModel.sort(function (a, b) {
                return b.CaseReleaseDate > a.CaseReleaseDate ? intIsDesc : -intIsDesc
            })
        },
        addClickClass: function (e) {
            var btnSortCase = document.querySelectorAll(".btnSortCase");
            btnSortCase.forEach(x => x.classList.remove("active"));
            e.target.classList.add("active");
        },
        getPoseCaseForm: function () {
            dynamicLoading.css("/css/PostCase.css");
            $.ajax({
                url: "/Case/GetPostCase",
                type: "GET"
            })
                .done(function (res) {
                    $("#ajaxSection").html(res);
                    $.getScript("/js/PostCaseReady.js", function () { });
                    $.getScript("/js/PostCase.js", function () { });
                })
                .fail(function (res) {
                    console.log(res);
                });
        }
    },
    filters: {
        filterCaseNeedHelp: function (e) {
            if (e == true) {
                return "需求"
            }
            else {
                return "技能"
            }
        },
        showDate: function (e) {
            if (e.includes("T")) {
                return e.split("T")[0];
            }
        }
    },
    created() {
        $.ajax({
            url: "/Case/GetUserCaseModel",
            type: "GET"
        })
            .done(function (res) {
                vmCaseList.caseDataModel = vmCaseList.caseViewModel = JSON.parse(res);
            })
            .fail(function (res) {
                console.log(res);
            });
    }
});
