const urlInfo = {
    deleteNotEffect: 'DeleteNotEffect',
    deleteEffect: 'DeleteEffect',
    agree: 'Agree',
    forceAgree: 'ForceAgree',
};

const preAdjustStatus = {
    notEffect: '待生效',
    effect: '生效中',
    delete: '刪除',
    fail: '失敗'
};


$(document).ready(function () {
    
    showTip('show');

    $('#delete-not-effect').on('click', function () {

        let checkedItemList = getCheckedItem($('#process-table-not-effect'));

        if (checkedItemList.length == 0) {

            alert('請先於《等待區中》勾選資料後，再進行後續作業。');

        } else {

            let tipMsg = '您確定要將等待區中所勾選的' + checkedItemList.length + '筆資料，進行刪除嗎?';

            if (window.confirm(tipMsg)) {

                let preAdjustInfoList = getPreAdjustInfoList(checkedItemList);

                let ajaxResultInfo = callAjax('post', urlInfo.deleteNotEffect, {
                    Remark: $('#not-effect-remark').val(),
                    PreAdjustList: preAdjustInfoList
                });

                if (!ajaxResultInfo.success) {

                    showTip('show', '錯誤訊息', ajaxResultInfo.errorMsg);

                } else if (ajaxResultInfo.isJson) {

                    alert('《等待區中》共' + ajaxResultInfo.info.PreAdjustList.length + '筆資料，已註記為刪除狀態。');

                    refreshPage();
                }
            }
        }
    });


    $('#agree').on('click', function () {

        let checkedItemList = getCheckedItem($('#process-table-not-effect'));

        if (checkedItemList.length == 0) {

            alert('請先於《等待區中》勾選資料後，再進行後續作業。');

        } else {

            let preAdjustInfoList = getPreAdjustInfoList(checkedItemList);

            let checkedItemInfo = {
                deleteItem: caseStatusFilter(preAdjustInfoList, preAdjustStatus.delete),
                failItem: caseStatusFilter(preAdjustInfoList, preAdjustStatus.fail),
                errorMsg: [],
            };

            if (checkedItemInfo.deleteItem.length > 0) {
                checkedItemInfo.errorMsg.push(checkedItemInfo.deleteItem.length + '筆刪除');
            }

            if (checkedItemInfo.failItem.length > 0) {
                checkedItemInfo.errorMsg.push(checkedItemInfo.failItem.length + '筆失敗');
            }

            if (checkedItemInfo.errorMsg.length > 0) {
                alert('您選取的資料中包含' + checkedItemInfo.errorMsg.join('、') + '，請先確認後再進行同意生效。');

            } else {

                let tipMsg = '您確定要將等待區中所勾選的' + checkedItemList.length + '筆資料，進行同意生效嗎?';

                if (window.confirm(tipMsg)) {

                    let ajaxResultInfo = callAjax('post', urlInfo.agree, {
                        PreAdjustList: preAdjustInfoList
                    });

                    if (!ajaxResultInfo.success) {

                        showTip('show', '錯誤訊息', ajaxResultInfo.errorMsg);

                    } else if (ajaxResultInfo.isJson) {

                        let failItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.fail);
                        let effectItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.effect);

                        let tipMsgArray = [];
                        tipMsgArray.push('《生效區中》共' + ajaxResultInfo.info.PreAdjustList.length + '筆資料');

                        if (failItem.length > 0) {
                            tipMsgArray.push(failItem.length + '筆註記為失敗狀態');
                        }

                        if (effectItem.length > 0) {
                            tipMsgArray.push(effectItem.length + '筆註記為生效中並傳送CCAS完成');
                        }

                        alert(tipMsgArray.join('，') + '。');

                        refreshPage();
                    }
                }
            }
        }
    });


    $('#force-agree').on('click', function () {

        let checkedItemList = getCheckedItem($('#process-table-not-effect'));

        if (checkedItemList.length == 0) {

            alert('請先於《等待區中》勾選資料後，再進行後續作業。');

        } else {

            let preAdjustInfoList = getPreAdjustInfoList(checkedItemList);

            let checkedItemInfo = {
                deleteItem: caseStatusFilter(preAdjustInfoList, preAdjustStatus.delete),
                notEffectItem: caseStatusFilter(preAdjustInfoList, preAdjustStatus.notEffect),
                errorMsg: [],
            };

            if (checkedItemInfo.deleteItem.length > 0) {
                checkedItemInfo.errorMsg.push(checkedItemInfo.deleteItem.length + '筆刪除');
            }

            if (checkedItemInfo.notEffectItem.length > 0) {
                checkedItemInfo.errorMsg.push(checkedItemInfo.notEffectItem.length + '筆待生效');
            }

            if (checkedItemInfo.errorMsg.length > 0) {
                alert('您選取的資料中包含' + checkedItemInfo.errorMsg.join('、') + '，請先確認後再進行同意生效。');

            } else {

                let tipMsg = '您確定要將等待區中所勾選的' + checkedItemList.length + '筆資料(狀態:失敗)，進行強制同意嗎?';

                if (window.confirm(tipMsg)) {

                    let ajaxResultInfo = callAjax('post', urlInfo.forceAgree, {
                        NeedValidate: true,
                        PreAdjustList: preAdjustInfoList
                    });

                    if (!ajaxResultInfo.success) {

                        showTip('show', '錯誤訊息', ajaxResultInfo.errorMsg);

                    } else if (ajaxResultInfo.isJson) {

                        let effectItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.effect);
                        let validateFailItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.fail, true);

                        let forceAgreeMsg = null;
                        let needForceAgreeItem = validateFailItem.filter(function (i, e) {

                            forceAgreeMsg = '『' + e.CustomerId + '』強制同意的檢核條件仍為失敗' +
                                '\n 失敗原因：' + e.RejectReasonCode + '，您仍繼續進行強制同意嗎?';

                            return window.confirm(forceAgreeMsg);
                        });

                        let finalMsg = null;

                        if (needForceAgreeItem.length > 0) {

                            ajaxResultInfo = callAjax('post', urlInfo.forceAgree, {
                                NeedValidate: false,
                                PreAdjustList: needForceAgreeItem.toArray()
                            });

                            if (!ajaxResultInfo.success) {

                                showTip('show', '錯誤訊息', ajaxResultInfo.errorMsg);

                            } else if (ajaxResultInfo.isJson) {

                                let forceAgreeItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.effect);

                                finalMsg = createForceAgreeFinalMsg(effectItem.length,
                                    validateFailItem.length, forceAgreeItem.length);
                            }

                        } else {

                            finalMsg = createForceAgreeFinalMsg(effectItem.length,
                                validateFailItem.length, 0);
                        }

                        alert(finalMsg);

                        refreshPage();
                    }
                }
            }
        }
    });


    $('#delete-effect').on('click', function () {

        let checkedItemList = getCheckedItem($('#process-table-effect'));

        if (checkedItemList.length == 0) {

            alert('請先於《生效區中》勾選資料後，再進行後續作業。');

        } else {

            let tipMsg = '您確定要將生效區中所勾選的' + checkedItemList.length + '筆資料，進行刪除嗎?';

            if (window.confirm(tipMsg)) {

                let preAdjustInfoList = getPreAdjustInfoList(checkedItemList);

                let ajaxResultInfo = callAjax('post', urlInfo.deleteEffect, {
                    Remark: $('#effect-remark').val(),
                    PreAdjustList: preAdjustInfoList
                });

                if (!ajaxResultInfo.success) {

                    showTip('show', '錯誤訊息', ajaxResultInfo.errorMsg);

                } else if (ajaxResultInfo.isJson) {

                    let deleteItem = caseStatusFilter(ajaxResultInfo.info.PreAdjustList, preAdjustStatus.delete);

                    alert('《生效區中》共' + deleteItem.length + '筆資料，已註記為刪除狀態，並傳送CCAS完成。');

                    refreshPage();
                }
            }
        }
    });


    $('.pagination-container').find('a').each(function (i, e) {

        let params = $(e).prop('href').split('?')[1];

        if (params) {

            let paramsInfo = params.split(',');
            let page = paramsInfo[0];
            let type = paramsInfo[1];

            $(e).prop('href', '#');

            $(e).on('click', function () {

                let refresh = true;

                if (type === 'NotEffect') {

                    $('#NotEffectPageIndex').val(page);

                } else if (type === 'Effect') {

                    $('#EffectPageIndex').val(page);

                } else {

                    refresh = false;
                }

                if (refresh) {
                    refreshPage();
                }
            });
        }
    });


    $('#query-pre-adjust').on('click', function () {

        $('#NotEffectPageIndex').val(1);
        $('#EffectPageIndex').val(1);

        refreshPage();
    });
});


function showTip(type, title, message) {

    switch (type) {

        case "show":
            $("#tip-modal-type-show").show();
            $("#tip-modal-type-dialog").hide();
            break

        case "dialog":
            $("#tip-modal-type-show").hide();
            $("#tip-modal-type-dialog").show();
            break;

        default:
            $("#tip-modal-type-show").hide();
            $("#tip-modal-type-dialog").hide();
            break;
    }

    let tipModal = $('#tip-modal');
    if (tipModal.length > 0) {

        if (title) {
            tipModal.find('.modal-title').html(title);
        }

        if (message) {
            tipModal.find('.modal-body>p').html(message);
        }

        if (tipModal.find('.modal-title').html() &&
            tipModal.find('.modal-body>p').html()) {
            tipModal.modal('show');
        }
    }
}

function getCheckedItem(table) {

    return table.find('tr:not(:first)').filter(function () {
        return $(this).find('#pre-adjust-check-box').is(':checked');
    });
};

function createForceAgreeFinalMsg(effectItemCount, validateFailItemCont, forceAgreeItemCount) {

    let resultMessage = null;

    let validateFailCount = (validateFailItemCont - forceAgreeItemCount);
    let totalItemCount = effectItemCount + validateFailCount + forceAgreeItemCount;

    let finalMsgArray = [];

    if (totalItemCount > 0) {
        finalMsgArray.push('強制同意《等待區中》共' + totalItemCount + '筆資料');
    }

    if (effectItemCount > 0) {

        finalMsgArray.push(effectItemCount + '筆檢核條件成功');
    }

    if (validateFailCount > 0) {

        finalMsgArray.push(validateFailCount + '筆檢核條件失敗');
    }

    if (forceAgreeItemCount > 0) {

        finalMsgArray.push(forceAgreeItemCount + '筆檢核條件失敗，人工強制同意');
    }

    resultMessage = finalMsgArray.join('\n') + '。';

    return resultMessage;
};

function caseStatusFilter(preAdjustList, status, rejectReasonCodeExsit) {

    if (rejectReasonCodeExsit) {

        return $(preAdjustList).filter(function (i, e) {
            return (e.Status === status && e.RejectReasonCode);
        });

    } else {

        return $(preAdjustList).filter(function (i, e) {
            return e.Status === status;
        });
    }
}

function getPreAdjustInfoList(checkedItemList) {

    let preAdjustInfoList = [];
    let dataAttribute = null;

    checkedItemList.each(function (idx, ele) {

        dataAttribute = $(ele).data();

        preAdjustInfoList.push({
            CustomerId: dataAttribute.customerId,
            CampaignId: dataAttribute.campaignId,
            Status: dataAttribute.status,
        });
    });

    return preAdjustInfoList;
}

function refreshPage() {

    $('#form-query').submit();
}