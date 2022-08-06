var vmEC = new Vue({
    el: "#executeList",
    data: {
        exeCaseModel:[],
        tarCaseId: "",
        tarCaseTitle: "",
        tarIsNeed:"",
        tarUserId: "",
        transPoint:0,
    },
    methods: {
        getExeCaseModel: function () {
            fetch("/Case/GetExecuteCaseModel")
                .then(res => {
                    return res.json();
                }).then(data => {
                    this.exeCaseModel = JSON.parse(data);
                }).catch(err => console.log(err))
        },
        getTargetData: function (e) {
            this.tarCaseId = e.target.dataset.caseid;
            this.tarCaseTitle = e.target.dataset.casetitle ;
            this.tarUserId = e.target.dataset.targetuserid ;
            this.transPoint = e.target.dataset.point;
            this.tarIsNeed = e.target.dataset.isneed;
        },
        caseDone: function () {
            $.ajax({
                url: "/Case/CaseDone",
                method: "POST",
                data: {
                    CaseId: vmEC.tarCaseId
                }
            })
                .done(function (res) {
                    //refresh points
                    var userPoint = document.getElementById("userPoint");
                    userPoint.innerHTML = "";
                    var iconPoint = document.createElement("i");
                    iconPoint.classList.add("fa-solid", "fa-diamond");
                    iconPoint.style.fontSize = "12px";
                    iconPoint.style.color = "var(--help)";
                    var point = document.createElement("span");
                    point.innerHTML = `&nbsp;${res.msg}`;
                    userPoint.append(iconPoint);
                    userPoint.append(point);
                    //show toast
                    $("#confirmModal").modal("hide");
                    var toastTransResult = document.getElementById('ToastTransResult')
                    var toast = new bootstrap.Toast(toastTransResult);
                    toast.show();
                })
                .fail(function (res) {
                    console.log(res);
                });
        }
    },
    filters: {
        showDate: function (e) {
            if (e.includes("T")) {
                return e.split("T")[0];
            }
        },
        isNeedFilter: function (e) {
            return e == true ? "幫助" : "技能";
        }
    },
    created: function () {
        this.getExeCaseModel();
    }
});
