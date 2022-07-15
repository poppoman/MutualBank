//動態載入css、js
var dynamicLoading = {
    css: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.href = path;
        link.rel = 'stylesheet';
        link.type = 'text/css';
        head.appendChild(link);
    },
    js: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.src = path;
        script.type = 'text/javascript';
        head.appendChild(script);
    }
}

//載入首頁Case的導覽屬性(技能名稱)
function GetSkillName() {
    var SkillIds = document.getElementsByName('caseSkillId');
    SkillIds.forEach(x => {
        var skillId = parseInt(x.dataset.skillid);
        $.ajax({
            url: "/Home/GetSkillName",
            type: "GET",
            data: {
                SkillId: skillId
            },
            success: function (res) {
                x.innerText = res;
            },
            error: function (res) {
            }
        });
    });
};