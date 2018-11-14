
const urlInfo = {
    query: 'Adjust/Query',
};


$(document).ready(function () {

    $('#query').on('click', function () {

        let ajaxResultInfo = callAjax('post', urlInfo.query, {
            CustomerId: $('#CustomerId').val(),
        });

        //let json = '{"success":true,' +
        //    '"errorMsg":null,' +
        //    '"info":{' +
        //    '"ErrorCodeList":["03","04"],' +
        //    '"PreAdjustInfo":{"CampaignId":"AA20951022X99Y99Z99A","CustomerId":"A177842053","ProjectName":"生效區測試資料","ProjectAmount":30000,"CloseDate":"2099/10/22","ImportDate":"2018/10/30","ChineseName":"王小明","Kind":"AA","SmsCheckResult":null,"Status":"生效中","ProcessingDateTime":null,"ProcessingUserId":null,"DeleteDateTime":null,"DeleteUserId":null,"Remark":null,"ClosingDay":"10","PayDeadline":"25","ForceAgreeUserId":null,"MobileTel":"0933113885","RejectReasonCode":null,"CcasReplyCode":"00","CcasReplyStatus":null,"CcasReplyDateTime":null},' +
        //    '"CustomerInfo":{"AccountId":"A177842053","ChineseName":"王小明","BirthDay":null,"RiskLevel":null,"RiskRating":null,"CreditLimit":null,"AboutDataStatus":null,"IssueDate":null,"LiveCardCount":null,"Status":null,"Vocation":null,"BillAddr":null,"TelOffice":null,"TelHome":null,"MobileTel":"0933113885","Latest1Mnth":null,"Latest2Mnth":null,"Latest3Mnth":null,"Latest4Mnth":null,"Latest5Mnth":null,"Latest6Mnth":null,"Latest7Mnth":null,"Latest8Mnth":null,"Latest9Mnth":null,"Latest10Mnth":null,"Latest11Mnth":null,"Latest12Mnth":null,"Consume1":null,"Consume2":null,"Consume3":null,"Consume4":null,"Consume5":null,"Consume6":null,"Consume7":null,"Consume8":null,"Consume9":null,"Consume10":null,"Consume11":null,"Consume12":null,"PreCash1":null,"PreCash2":null,"PreCash3":null,"PreCash4":null,"PreCash5":null,"PreCash6":null,"PreCash7":null,"PreCash8":null,"PreCash9":null,"PreCash10":null,"PreCash11":null,"PreCash12":null,"CreditRating1":null,"CreditRating2":null,"CreditRating3":null,"CreditRating4":null,"CreditRating5":null,"CreditRating6":null,"CreditRating7":null,"CreditRating8":null,"CreditRating9":null,"CreditRating10":null,"CreditRating11":null,"CreditRating12":null,"ClosingDay":"10","PayDeadline":"25","ClosingAmount":null,"MinimumAmountPayable":null,"RecentPaymentAmount":null,"RecentPaymentDate":null,"OfferAmount":null,"UnpaidTotal":null,"AuthorizedAmountNotAccount":null,"AdjustReason":null,"AdjustArea":null,"AdjustStartDate":"20181010","AdjustEndDate":"20190101","AdjustEffectAmount":null,"VintageMonths":null,"StatusFlag":null,"GutrFlag":null,"DelayCount":null,"CcasUnderpaidAmount":null,"CcasUsabilityAmount":null,"CcasUnderpaidRate":null,"DataDate":null,"EligibilityForWithdrawal":null,"SystemAdjustRevFlag":null,"AutomaticDebit":null,"DebitBankCode":null,"EtalStatus":null,"TelResident":null,"SendType":null,"ElectronicBillingCustomerNote":null,"Email":null,"Industry":null,"JobTitle":null,"ResidentAddr":null,"MailingAddr":null,"CompanyAddr":null,"AnnualIncome":null,"In1":null,"In2":null,"In3":null,"ResidentAddrPostalCode":null,"MailingAddrPostalCode":null,"CompanyAddrPostalCode":null}},' +
        //    '"isJson":true}';

        //let ajaxResultInfo = JSON.parse(json);

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