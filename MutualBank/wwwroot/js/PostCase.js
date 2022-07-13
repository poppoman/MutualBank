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