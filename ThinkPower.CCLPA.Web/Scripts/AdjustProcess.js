
const urlInfo = {
    query: 'Adjust/Query',
    queryJcic: 'Adjust/QueryJcic',
    verifyAdjustCondition: 'Adjust/VerifyAdjustCondition',
};

const scale = 100;


$(document).ready(function () {

    showErrorMessage();
    toggleButtonShowType();

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

                        let form = $('#pre-adjust-load');

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

                    let form = $('#adjust-application-data');

                    if (form.length > 0) {

                        form.append($("<input>", { type: 'text', name: 'CustomerId', value: $('#CustomerId').val() }));
                        form.submit();
                    }
                }
            }
        }
    });

    $('#adjust-application').on('click', function (e) {

        if (validateField()) {
            let jcicElement = $('#JcicQueryDate');

            if (checkElementExist(jcicElement)) {
                let jcicQueryDate = jcicElement.val();
                let customerId = $('#CustomerId').val();
                let adjustReasonCode = $('#AdjustReasonSelectListItem :selected').val();

                if (!jcicQueryDate) {
                    queryJcic(customerId);
                }
                else {
                    verifyAdjustCondition(customerId, jcicQueryDate, adjustReasonCode);
                    toggleButtonShowType(e.target.id);
                }
            }
        }
    });

    $('#AdjustReasonSelectListItem').on('change', function () {

        let selectedItem = $(this).find(':selected');
        let reasonCode = selectedItem.val();

        if (reasonCode) {

            let reasonEffectInfo = $('.reason-effect-info').filter(function (i, e) {
                let dataList = $(e).data();
                return (dataList.reason.toString() === reasonCode);
            });

            let reasonEffectData = reasonEffectInfo.data();

            let result = null;

            if (reasonEffectData) {
                if (reasonEffectData.approveAmountMax) {
                    if (reasonEffectData.approveScaleMax) {
                        result = (reasonEffectData.approveAmountMax * (reasonEffectData.approveScaleMax / scale));
                    } else {
                        result = reasonEffectData.approveAmountMax;
                    }


                } else if (reasonEffectData.approveAmountMax.toString() === '0') {
                    result = '不設限';
                }
            }

            $('#AdjustmentAmountCeiling').val(result);
        }

        toggleReasonCodeShowObject(reasonCode);
    });

    $('#UseLocationSelectListItem').on('change', function () {

        let selectedItem = $(this).find(':selected');
        let selectedItemValue = selectedItem.val();

        toggleUseLocationShowObject(selectedItemValue);
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

function showErrorMessage() {
    let errorMsg = $('#ErrorMessage');

    if (checkElementExist(errorMsg)) {
        alert(errorMsg.val());
    }
}

function toggleButtonShowType(event) {
    $('#adjust-application').prop('disabled', true);
    $('#approved').prop('disabled', true);
    $('#refuse').prop('disabled', true);
    $('#cancel').prop('disabled', true);

    if ($('#CustomerId').val()) {
        $('#adjust-application').prop('disabled', false);
    }

    if (event === 'adjust-application') {

        if (!$('#ProjectRejectReason').text()) {
            $('#approved').prop('disabled', false);
        }

        if (!$('#ProjectResult').text()) {
            $('#refuse').prop('disabled', false);
            $('#cancel').prop('disabled', false);
        }
    }
}

function queryJcic(customerId) {

    let ajaxResultInfo = callAjax('post', urlInfo.queryJcic, {
        CustomerId: customerId,
    });

    if (!ajaxResultInfo.success) {

        alert(ajaxResultInfo.errorMsg);

    } else if (ajaxResultInfo.isJson) {
        let infoModel = ajaxResultInfo.info;
        let msg = null;

        if (infoModel.ResponseCode === '73') {
            msg = '該筆資料EJCIC On-Line送查等待中，請稍後再按『申請臨調』按鈕。';
            alert(msg);

        } else if (infoModel.ResponseCode === '72') {
            msg = '該筆資料EJCIC On-Line於' + infoModel.JcicQueryDate + '送查失敗，是否啟動重送機制?';

            if (window.confirm(msg)) {
                queryJcic(customerId);
            }
        } else if (infoModel.ResponseCode === '00') {
            $('#JcicQueryDate').val(infoModel.JcicQueryDate);

        } else {
            msg = '系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。';
            alert(msg);
        }
    }
}

function verifyAdjustCondition(customerId, jcicQueryDate, adjustReasonCode) {

    let ajaxResultInfo = callAjax('post', urlInfo.verifyAdjustCondition, {
        CustomerId: customerId,
        JcicQueryDate: jcicQueryDate,
        AdjustReasonCode: adjustReasonCode,
    });

    if (!ajaxResultInfo.success) {

        alert(ajaxResultInfo.errorMsg);

    } else if (ajaxResultInfo.isJson) {
        let infoModel = ajaxResultInfo.info;

        if (infoModel.ResponseCode === '00') {
            $('#EstimateResult').text(infoModel.EstimateResult);
            $('#RejectReason').text(infoModel.RejectReason);
            $('#ProjectResult').text(infoModel.ProjectResult);
            $('#ProjectRejectReason').text(infoModel.ProjectRejectReason);

        } else {
            alert('系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。');
        }
    }
}

function createErrorInfo(errorMsg, element) {
    let errorInfo = null;

    if (errorMsg && element && element.length > 0) {
        errorInfo = {
            errorMessage: errorMsg,
            htmlElement: element,
        };
    }

    return errorInfo;
}

function validateField() {
    let verifyInfoArray = [];
    let errorInfo = null;



    let adjustReasonSelectListItem = $('#AdjustReasonSelectListItem');

    if (checkInputFieldCanUse(adjustReasonSelectListItem)) {

        let selectedItem = adjustReasonSelectListItem.find(':selected');
        let reasonCode = selectedItem.val();

        if (!reasonCode) {
            errorInfo = createErrorInfo('請輸入「調高原因」。', adjustReasonSelectListItem);
            verifyInfoArray.push(errorInfo);
        }
    }



    let adjustReasonRemark = $('#AdjustReasonRemark');

    if (checkInputFieldCanUse(adjustReasonRemark)) {

        let value = adjustReasonRemark.val();

        if (!value) {
            errorInfo = createErrorInfo('請輸入「調高原因備註」。', adjustReasonRemark);
            verifyInfoArray.push(errorInfo);

        } else {
            let valueByte = calculationValueByte(value);
            let maxlength = adjustReasonRemark.prop('maxlength');

            if (valueByte > maxlength) {
                errorInfo = createErrorInfo('「調高原因備註」輸入資料超出限制長度，請重新確認。', adjustReasonRemark);
                verifyInfoArray.push(errorInfo);
            }
        }
    }



    let swipeAmount = $('#SwipeAmount');

    if (checkInputFieldCanUse(swipeAmount)) {

        let value = swipeAmount.val();
        let minlength = swipeAmount.data('minlength');
        let maxlength = swipeAmount.prop('maxlength');

        if (!checkValueLength(value, minlength, maxlength) ||
            !checkNumerical(value)) {
            errorInfo = createErrorInfo('「刷卡金額(不限額度)」必須輸入5~9位數金額。', swipeAmount);
            verifyInfoArray.push(errorInfo);
        }
    }



    let afterAdjustAmount = $('#AfterAdjustAmount');

    if (checkInputFieldCanUse(afterAdjustAmount)) {

        let value = afterAdjustAmount.val();
        let minlength = afterAdjustAmount.data('minlength');
        let maxlength = afterAdjustAmount.prop('maxlength');

        if (!checkValueLength(value, minlength, maxlength) ||
            !checkNumerical(value)) {
            errorInfo = createErrorInfo('「臨調後額度」必須輸入5~9位數金額。', afterAdjustAmount);
            verifyInfoArray.push(errorInfo);

        } else if (checkInputFieldCanUse(swipeAmount) &&
            value < swipeAmount.val()) {
            errorInfo = createErrorInfo('「臨調後額度」必須大於等於「刷卡金額(不含額度)」。', afterAdjustAmount);
            verifyInfoArray.push(errorInfo);

        } else {
            let creditLimit = $('#credit-limit');

            if (creditLimit) {
                let creditLimitValue = creditLimit.text().replace(',', '');

                if (value < creditLimitValue) {
                    errorInfo = createErrorInfo('「臨調後額度」必須大於等於「目前信用額度」。', afterAdjustAmount);
                    verifyInfoArray.push(errorInfo);
                }
            }
        }
    }



    let validDateStart = $('#ValidDateStart');

    if (checkInputFieldCanUse(validDateStart)) {

        let value = validDateStart.val();

        if (!value) {
            errorInfo = createErrorInfo('請輸入「有效日期(起)」。', validDateStart);
            verifyInfoArray.push(errorInfo);

        } else {
            let valueDate = new Date(value);
            let currentDateTime = new Date();
            let tempDate = [currentDateTime.getFullYear(), (currentDateTime.getMonth() + 1), currentDateTime.getDate()];
            let currentDate = new Date(tempDate.join('/'));

            if (valueDate === 'Invalid Date') {
                errorInfo = createErrorInfo('「有效日期(起)」請輸入正常日期。', validDateStart);
                verifyInfoArray.push(errorInfo);

            } else if (valueDate < currentDate) {
                errorInfo = createErrorInfo('「有效日期(起)」不得小於系統日期。', validDateStart);
                verifyInfoArray.push(errorInfo);
            }
        }
    }



    let validDateEnd = $('#ValidDateEnd');

    if (checkInputFieldCanUse(validDateEnd)) {

        let value = validDateEnd.val();

        if (!value) {
            errorInfo = createErrorInfo('請輸入「有效日期(迄)」。', validDateEnd);
            verifyInfoArray.push(errorInfo);

        } else {
            let valueDate = new Date(value);

            if (valueDate === 'Invalid Date') {
                errorInfo = createErrorInfo('「有效日期(迄)」請輸入正常日期。', validDateEnd);
                verifyInfoArray.push(errorInfo);

            } else if (checkInputFieldCanUse(validDateStart)) {

                let startDateValue = validDateStart.val();
                let startDate = new Date(startDateValue);

                if (startDateValue &&
                    startDate !== 'Invalid Date' &&
                    startDate > valueDate) {
                    errorInfo = createErrorInfo('「有效日期(起)」不可大於「有效日期(迄)」。', validDateEnd);
                    verifyInfoArray.push(errorInfo);
                }
            }
        }
    }



    let manualAuthorizationSelectListItem = $('#ManualAuthorizationSelectListItem');

    if (checkInputFieldCanUse(manualAuthorizationSelectListItem)) {

        let selectedItem = manualAuthorizationSelectListItem.find(':selected');
        let selectedItemValue = selectedItem.val();

        if (!selectedItemValue) {
            errorInfo = createErrorInfo('請輸入「是否可人工授權」。', manualAuthorizationSelectListItem);
            verifyInfoArray.push(errorInfo);
        }
    }



    let useLocationSelectListItem = $('#UseLocationSelectListItem');

    if (checkInputFieldCanUse(useLocationSelectListItem)) {

        let selectedItem = useLocationSelectListItem.find(':selected');
        let selectedItemValue = selectedItem.val();

        if (!selectedItemValue) {
            errorInfo = createErrorInfo('請輸入「使用地點」。', useLocationSelectListItem);
            verifyInfoArray.push(errorInfo);
        }
    }



    let placeOfGoingAbroad = $('#PlaceOfGoingAbroad');

    if (checkInputFieldCanUse(placeOfGoingAbroad)) {

        let value = placeOfGoingAbroad.val();
        let fullValue = ConvertHalfOrFull(value, true);
        let valueByte = calculationValueByte(fullValue);
        let maxlength = placeOfGoingAbroad.prop('maxlength');

        if (!value) {
            errorInfo = createErrorInfo('請輸入「出國地點」。', placeOfGoingAbroad);
            verifyInfoArray.push(errorInfo);

        } else if (valueByte > maxlength) {
            errorInfo = createErrorInfo('「出國地點」輸入資料超出限制長度，請重新確認。', placeOfGoingAbroad);
            verifyInfoArray.push(errorInfo);
        }
    }



    $(verifyInfoArray).each(function (i, e) {

        alert(e.errorMessage);
        e.htmlElement.focus();
        return false;
    });

    return (verifyInfoArray.length === 0);
}

function calculationValueByte(value) {

    // 計算有幾個全型字、中文字...
    let chinese = value.match(/[^ -~]/g);
    return (value.length + (chinese ? chinese.length : 0));
}

function checkNumerical(value) {
    let regular = /^[0-9]+$/;
    return regular.test(value);
}

function toggleReasonCodeShowObject(reasonCode) {
    let swipeAmount = $('#SwipeAmount');

    if (reasonCode === '12') {
        swipeAmount.prop('disabled', false);
    } else {
        swipeAmount.prop('disabled', true);
    }

    swipeAmount.val('');
}

function toggleUseLocationShowObject(selectItemValue) {
    let placeOfGoingAbroad = $('#PlaceOfGoingAbroad');

    if (selectItemValue === '1' ||
        selectItemValue === '3') {
        placeOfGoingAbroad.prop('disabled', false);
    } else {
        placeOfGoingAbroad.prop('disabled', true);
    }

    placeOfGoingAbroad.val('');
}

function checkValueLength(value, minlength, maxlength) {

    let validateResult = false;

    if (value) {

        if (minlength && maxlength) {
            validateResult = (value.length >= minlength && value.length <= maxlength);

        } else if (minlength) {
            validateResult = (value.length >= minlength);

        } else if (maxlength) {
            validateResult = (value.length <= maxlength);
        }
    }

    return validateResult;
}

function checkInputFieldCanUse(element) {

    let canUse = false;

    if (checkElementExist(element) &&
        !element.prop('disabled')) {
        canUse = true;
    }

    return canUse;
}

function checkElementExist(element) {
    return (element && element.length > 0);
}

function ConvertHalfOrFull(value, halfToFull) {

    let result = [];

    if (value) {

        //unicode編碼範圍是所有的英文字母以及各種字元
        //{"index":32,"value":" "},     +12256
        //{"index":33,"value":"!"},     +65248
        //{"index":126,"value":"~"},    +65248
        //
        //{"index":12288,"value":"　"},  -12256
        //{"index":65281,"value":"！"},  -65248
        //{"index":65374,"value":"～"},  -65248

        if (halfToFull) {
            for (let i = 0; i < value.length; i++) {

                let charUnicode = value.charCodeAt(i);

                if (charUnicode >= 33 && charUnicode <= 126) {
                    result.push(String.fromCharCode(charUnicode + 65248));

                } else if (charUnicode == 32) {
                    // Blank
                    result.push(String.fromCharCode(charUnicode + 12256));

                } else {
                    // OriginalChar
                    result.push(value.charAt(i));
                }
            }

        } else {
            for (let i = 0; i < value.length; i++) {

                let charUnicode = value.charCodeAt(i);

                if (charUnicode >= 65281 && charUnicode <= 65374) {
                    result.push(String.fromCharCode(charUnicode - 65248));

                } else if (charUnicode == 12288) {
                    // Blank
                    result.push(String.fromCharCode(charUnicode - 12256));

                } else {
                    // OriginalChar
                    result.push(value.charAt(i));
                }
            }
        }
    }

    return result.join('');
}