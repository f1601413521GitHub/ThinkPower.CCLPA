
const urlInfo = {
    query: 'Adjust/Query',
};


$(document).ready(function () {

    $('#query').on('click', function () {

        let ajaxResultInfo = callAjax('post', urlInfo.query, {
            CustomerId: $('#CustomerId').val(),
        });

        if (!ajaxResultInfo.success) {

            alert(ajaxResultInfo.errorMsg);

        } else if (ajaxResultInfo.isJson) {

            let infoModel = ajaxResultInfo.info;
            let adjustApplicationFlag = true;

            $(infoModel.ErrorCodeList.sort()).each(function (index, errorCode) {

                let tempErrorMsg = null;

                if (errorCode === '01') {

                    adjustApplicationFlag = false;
                    alert('此歸戶ID，目前專案臨調PENDING處理中，無法再做申請動作。');
                    return false;

                } else if (errorCode === '02') {

                    adjustApplicationFlag = false;
                    alert('查無相關資料，請確認是否有輸入錯誤。');
                    return false;

                } else if (errorCode === '03' && infoModel.PreAdjustInfo) {

                    tempErrorMsg = '此歸戶已有生效中的預審專案。\n' +
                        '預審專案代號：' + infoModel.PreAdjustInfo.ProjectName + '\n' +
                        '預審額度：' + infoModel.PreAdjustInfo.ProjectAmount + '\n' +
                        '截止日：' + infoModel.PreAdjustInfo.CloseDate + '\n' +
                        '您現在要切換到『專案臨調預審名單處理』畫面嗎?';

                    if (window.confirm(tempErrorMsg)) {

                        adjustApplicationFlag = false;

                        let form = $('#preAdjustLoad');

                        if (form.length > 0) {

                            form.append($("<input>", { type: 'text', name: 'CustomerId', value: $('#CustomerId').val() }));
                            form.append($("<input>", { type: 'text', name: 'NotEffectPageIndex', value: 1 }));
                            form.append($("<input>", { type: 'text', name: 'EffectPageIndex', value: 1 }));
                            form.submit();

                        }

                        return false;

                    } else {
                        adjustApplicationFlag = true;
                    }

                } else if (errorCode === '04' && infoModel.CustomerInfo) {

                    tempErrorMsg = '此歸戶已有生效中的臨調。\n' +
                        '調高原因：' + infoModel.CustomerInfo.AdjustReason + '\n' +
                        '區域：' + infoModel.CustomerInfo.AdjustArea + '\n' +
                        '起迄區間：' +
                        adjustDateFormat(infoModel.CustomerInfo.AdjustStartDate, 'yyyy/MM/dd') + '~' +
                        adjustDateFormat(infoModel.CustomerInfo.AdjustEndDate, 'yyyy/MM/dd') + '\n' +
                        '臨調後金額：' + infoModel.CustomerInfo.AdjustEffectAmount + '\n' +
                        '您現在是否要繼續做專案臨調申請?';

                    if (window.confirm(tempErrorMsg)) {
                        adjustApplicationFlag = true;
                    } else {
                        adjustApplicationFlag = false;
                    }
                }
            });

            if (adjustApplicationFlag && infoModel.CustomerInfo) {

                let msg = '您確定要做「歸戶ID：' + infoModel.CustomerInfo.AccountId +
                    '，姓名：' + infoModel.CustomerInfo.ChineseName + '」的專案臨調處理嗎?';

                if (window.confirm(msg)) {

                    let form = $('#adjustApplicationData');

                    if (form.length > 0) {

                        form.append($("<input>", { type: 'text', name: 'CustomerId', value: $('#CustomerId').val() }));
                        form.submit();
                    }
                }
            }
        }
    });

    $('#AdjustReasonSelectListItem').on('click', function () {

        let selectedItem = $(this).find(':selected');
        let reasonCode = selectedItem.val();
        if (reasonCode) {

            let reasonEffectInfo = $('.reason-effect-info').filter(function (i, e) {
                let dataList = $(e).data();
                console.log([JSON.stringify(dataList)], reasonCode);
                return (dataList.reason.toString() === reasonCode);
            });

            let reasonEffectData = reasonEffectInfo.data();

            console.log(JSON.stringify(reasonEffectData));

            let result = null;

            if (reasonEffectData) {
                if (reasonEffectData.approveAmountMax) {
                    if (reasonEffectData.approveScaleMax) {
                        // TODO 倍率計算公式?
                        result = (reasonEffectData.approveAmountMax * (reasonEffectData.approveScaleMax / 100));
                    } else {
                        result = reasonEffectData.approveAmountMax;
                    }


                } else if (reasonEffectData.approveAmountMax.toString() === '0') {
                    result = '不設限';
                }
            }

            console.log({
                result: result
            });
        }
    });
});


function adjustDateFormat(dateTime, format) {

    let tempDateTime = null;

    if (dateTime) {

        if (format === 'yyyy/MM/dd' && dateTime.length === 8) {

            tempDateTime = dateTime.substr(0, 4) + '/' + dateTime.substr(4, 2) + '/' + dateTime.substr(6, 2);
        }
    }

    return tempDateTime;
}