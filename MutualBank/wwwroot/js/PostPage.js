
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


