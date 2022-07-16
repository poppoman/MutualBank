
var vmCaseList = new Vue({
    el: "#caseList",
    data: {
        caseModel: []
    },
    methods: {
        initCaseModel: function () {
            $.ajax({
                url: "/Case/UserCaseModel",
                type: "GET",
                data: {
                    NeedBit: true
                }
            })
                .done(function (res) {
                    vmCaseList.caseModel = res;
                })
                .fail(function (res) {
                    alert("Fail");
                });


        },
        needCaseModel: function () {
            var bit = event.target.dataset.bit;
            $.ajax({
                url: "/Case/UserCaseModel",
                type: "GET",
                data: {
                    NeedBit: bit
                }
            })
                .done(function (res) {
                    vmCaseList.caseModel = res;
                })
                .fail(function (res) {
                    alert("Fail");
                });

        },
        getPostCase: function () {
            dynamicLoading.css("/css/PostCase.css");
            $.ajax({
                type: "POST",
                url: "/Case/GetPostCase",
                success: function (res) {
                    $('#ajaxSection').html(res);
                    $.getScript("/js/PostCaseReady.js", function () {
                    });
                    $.getScript("/js/PostCase.js", function () {
                    });
                },
                error: function (res) {
                    console.log(res);
                }
            });

        }
    },
    filters: {
        //顯示技能名稱，而非編號
        showSkillName: function (id) {
            var skillName = "X";
            $.ajax({
                url: "/Home/GetSkillName",
                type: "GET",
                async: false,
                data: {
                    SkillId: id
                }
            })
                .done(function (res) {
                    skillName = res;
                })
                .fail(function (res) {
                    skillName = "無分類"
                });
            return skillName;
        }

    },
    created() {
        //初始頁面顯示需求Case
        this.initCaseModel();
    },
});