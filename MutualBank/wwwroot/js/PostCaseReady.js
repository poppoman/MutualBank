
//-設定預約貼文可選取的時間(暫定14天內)
    let currentDate = new Date();
    function getDateString(date) {
        let monthJs = date.getMonth();
    let monthMd = (monthJs + 1) < 10 ? `0${monthJs + 1}` : monthJs + 1 > 12 ? monthJs : monthJs + 1;
    let dateJs = date.getDate();
    let dateMd = dateJs < 10 ? `0${dateJs}` : dateJs;
    return `${date.getFullYear()}-${monthMd}-${dateMd}`;
        }

    //計算日曆可選取時間的最大值
    function FinalDate(dateBegin, intCounts) {
    let selectedTimeStamp = new Date(dateBegin).getTime();
    let resultTimeStamp = selectedTimeStamp + intCounts * 1000 * 60 * 60 * 24;
    let finalDay = new Date(resultTimeStamp);
    return getDateString(finalDay);
        }

    //初始化日曆
    $(document).ready(function () {
        //設定日期預設顯示與最大、小值
    dateApp.min = dateApp.value = getDateString(currentDate);
    dateApp.max = FinalDate(currentDate, 14);
    //下架時間提醒文字
    dateRemind.innerText = dateApp.max;

    //預設關閉預約發佈時間
    dateApp.setAttribute('disabled', 'disabled');
        });
