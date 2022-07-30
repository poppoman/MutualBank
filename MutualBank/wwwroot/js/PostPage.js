
//新增留言



//新增回覆留言
$(".messageboard-frame").on("click", "#commentbtmre", function () {

    var reextareaVal = $("textarea:eq(1)").val()
    var fapadcontent = $("#commentbtmre").parents(".pad-content")

    fapadcontent.append("<div class='comment-bother-frame'><div class='comment-bother'><a href='' class='comment-pic-frame'><div style='width: 24px; height: 24px;'' class='avatar-frame'><img src='background.jpg' alt='無此圖片' class='comment-bother-pic'></div></a><div class='comment-brother-re'><div class='responser-name-frame'><a class='responser-name' href='''>回覆者的名字</a><div class='reply-user'><span>回覆給：</span><div class='comment-bother'><a href=''><img src='background.jpg' alt='無此圖片'class='comment-bother-pic'></a></div><a>名稱</a></div></div> <div class='text'></div><div class='response-day-button'><i class='fa-regular fa-clock'></i><span class='post-time'>XX 天前</span><span class='replay-btn'>回覆</span></div></div></div></div>")

    fapadcontent.find(".text").last().append(reextareaVal)
    $("textarea:eq(1)").val("");
    $("#commentbtmre").attr("disabled", "true");
    $(".comment-bother-frame-input").remove()

})

//按回覆後新增留言區 
$(".messageboard-frame").on("click", "[class=replay-btn]", function (e) {
    var b = $(e.target).parents(".pad-content")
    var tousername = $(e.target).parents(".response-day-button").siblings(".responser-name-frame").children(".responser-name").text()

    if ($(".textarea-box").length == 1 & b.length >= 1) {
        b.last().append("<div class='comment-bother-frame-input'><div class='comment-bother-input'><a href='' class='comment-pic-frame'><div style='width: 24px; height: 24px;' class='avatar-frame'><img src='background.jpg' alt='無此圖片' class='comment-bother-pic'></div></a><div class='textarea-box'></div><div class='col-auto-input'></div></div)</div>")
        $(".textarea-box:eq(1)").append("<textarea type='text' class='form-control' id='exampleFormControlTextarea2' placeholder='請輸入你的訊息' maxlength='200' rows='3'></textarea><div class='statics-text'>0/200字</div>")
        $(".form-control").attr("placeholder", "回覆給："+tousername)
        $(".col-auto-input").append(" <button type='button' class='btn btn-primary' disabled data-bs-toggle='button' id='commentbtmre'>留言</button>")
    }
    console.log(tousername)

 
    $(".pad-content").on("input propertychange", "[class=form-control]", function () {
        $("#commentbtmre").removeAttr("disabled");
    })
    $(".pad-content").on("keyup", "[class=form-control]", function () {
        if ($("textarea:eq(1)").val() == "") {
            $("#commentbtmre").attr("disabled", "true");
        }
    })

});


//偵測輸入=>按鈕啟動
$(".messageboard-frame").on("input propertychange", "[class=form-control]:eq(0)", function () {
    $("#commentbtn").removeAttr("disabled");
})

//偵測鍵盤UP沒有值=>按鈕關閉    
$(".messageboard-frame").on("keyup", "[class=form-control]:eq(0)", function () {
    if ($("textarea:eq(0)").val() == "") {
        $("#commentbtn").attr("disabled", "true");
    }
})


